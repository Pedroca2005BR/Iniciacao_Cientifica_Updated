using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Oculus.Interaction;
using Pedroca2005BR.Utilities;
using UnityEngine;

public class SnapInteractableVisuals_Phantom : MonoBehaviour
{
    [Header("Phantom Mechanics")]
    public EPIType type;
    public Material offRangeMaterial;
    public Material onRangeMaterial;

    [Tooltip("Tempo ocioso antes de desativar o SnapInteractable após soltar o item.")]
    [SerializeField] private float disableGrace = 0.5f;

    [SerializeField] private SnapInteractable snapInteractable;

    private class Ghost
    {
        public GameObject go;
        public readonly List<MeshRenderer> renderers = new();
        public bool inRange;
    }

    private readonly Dictionary<EPIIdentifier, Ghost> _ghosts = new();
    private int _selectingCount;           // itens efetivamente encaixados
    private Coroutine _disableRoutine;

    private void Start()
    {
        _selectingCount = snapInteractable.SelectingInteractors.Count();
        UpdateSnapEnabled();
    }

    private void OnEnable()
    {
        if (snapInteractable != null)
        {
            snapInteractable.WhenInteractorViewAdded += OnHoverAdded;
            snapInteractable.WhenInteractorViewRemoved += OnHoverRemoved;
            snapInteractable.WhenSelectingInteractorViewAdded += OnSelectAdded;
            snapInteractable.WhenSelectingInteractorViewRemoved += OnSelectRemoved;
        }
        EventManager.Subscribe(ConstantEventNames.ITEM_DROPPED, OnItemDropped);
        EventManager.Subscribe(ConstantEventNames.ITEM_PICKED_UP, OnItemPickedUp);
    }

    private void OnDisable()
    {
        if (snapInteractable != null)
        {
            snapInteractable.WhenInteractorViewAdded -= OnHoverAdded;
            snapInteractable.WhenInteractorViewRemoved -= OnHoverRemoved;
            snapInteractable.WhenSelectingInteractorViewAdded -= OnSelectAdded;
            snapInteractable.WhenSelectingInteractorViewRemoved -= OnSelectRemoved;
        }
        EventManager.Unsubscribe(ConstantEventNames.ITEM_DROPPED, OnItemDropped);
        EventManager.Unsubscribe(ConstantEventNames.ITEM_PICKED_UP, OnItemPickedUp);
        _disableRoutine = null;   // a coroutine é parada pelo Unity ao desativar
    }

    // ---------- eventos do jogo ----------
    private void OnItemPickedUp(object obj)
    {
        var item = obj as EPIIdentifier;
        if (item == null || item.epiType != type) return;
        if (_ghosts.ContainsKey(item)) return;

        _ghosts[item] = BuildGhost(item);
        UpdateSnapEnabled();
        RefreshVisuals();
    }

    private void OnItemDropped(object obj)
    {
        var item = obj as EPIIdentifier;
        if (item == null || !_ghosts.TryGetValue(item, out var ghost)) return;

        if (ghost.go != null) Destroy(ghost.go);
        _ghosts.Remove(item);

        RefreshVisuals();
        UpdateSnapEnabled();   // não desativa na hora: espera o snap acontecer
    }

    // ---------- eventos do snap ----------
    private bool TryGetGhost(IInteractorView view, out Ghost ghost)
    {
        foreach (var kv in _ghosts)
        {
            if (ReferenceEquals(view, kv.Key.snapInteractor))
            {
                ghost = kv.Value;
                return true;
            }
        }
        ghost = null;
        return false;
    }

    private void OnHoverAdded(IInteractorView v)
    {
        if (TryGetGhost(v, out var g)) { g.inRange = true; RefreshVisuals(); }
    }

    private void OnHoverRemoved(IInteractorView v)
    {
        if (TryGetGhost(v, out var g)) { g.inRange = false; RefreshVisuals(); }
    }

    private void OnSelectAdded(IInteractorView v)
    {
        _selectingCount++;
        RefreshVisuals();
    }

    private void OnSelectRemoved(IInteractorView v)
    {
        _selectingCount = Mathf.Max(0, _selectingCount - 1);
        RefreshVisuals();
        UpdateSnapEnabled();
    }

    // ---------- ativar/desativar o snap ----------
    private void UpdateSnapEnabled()
    {
        if (_ghosts.Count > 0)
        {
            if (_disableRoutine != null) { StopCoroutine(_disableRoutine); _disableRoutine = null; }
            snapInteractable.enabled = true;
        }
        else if (snapInteractable.enabled && _disableRoutine == null && isActiveAndEnabled)
        {
            _disableRoutine = StartCoroutine(DisableWhenIdle());
        }
    }

    private IEnumerator DisableWhenIdle()
    {
        float idle = 0f;
        while (idle < disableGrace)
        {
            var s = snapInteractable.State;
            bool busy = _ghosts.Count > 0
                     || s == InteractableState.Hover
                     || s == InteractableState.Select
                     || _selectingCount > 0;
            idle = busy ? 0f : idle + Time.deltaTime;
            yield return null;
        }

        if (_ghosts.Count == 0 && _selectingCount == 0)
            snapInteractable.enabled = false;

        _disableRoutine = null;
    }

    // ---------- visual ----------
    private void RefreshVisuals()
    {
        bool occupied = _selectingCount > 0;                       // alvo já ocupado
        bool anyInRange = _ghosts.Values.Any(g => g.inRange);

        foreach (var g in _ghosts.Values)
        {
            if (g.go == null) continue;

            // se algum item está no alcance, só ele aparece (evita dois fantasmas sobrepostos)
            bool show = !occupied && (!anyInRange || g.inRange);
            g.go.SetActive(show);

            Material mat = g.inRange ? onRangeMaterial : offRangeMaterial;
            foreach (var r in g.renderers)
            {
                var mats = new Material[r.sharedMaterials.Length];
                for (int i = 0; i < mats.Length; i++) mats[i] = mat;
                r.sharedMaterials = mats;
            }
        }
    }

    private Ghost BuildGhost(EPIIdentifier item)
    {
        var ghost = new Ghost();

        Transform interactorT = item.snapInteractor.transform;
        Transform itemRoot = interactorT.parent;
        Transform target = snapInteractable.transform;
        Quaternion invI = Quaternion.Inverse(interactorT.rotation);

        ghost.go = new GameObject("Ghost_" + itemRoot.name);
        ghost.go.transform.SetParent(transform, false);

        foreach (var mf in itemRoot.GetComponentsInChildren<MeshFilter>())
        {
            if (mf.sharedMesh == null) continue;

            var go = new GameObject(mf.name);
            go.transform.SetParent(ghost.go.transform, false);

            // pose do mesh relativa ao interactor, reaplicada no alvo do snap
            Vector3 relPos = invI * (mf.transform.position - interactorT.position);
            Quaternion relRot = invI * mf.transform.rotation;
            go.transform.SetPositionAndRotation(
                target.position + target.rotation * relPos,
                target.rotation * relRot);

            // escala de mundo igual à do mesh original
            Vector3 w = mf.transform.lossyScale;
            Vector3 p = go.transform.parent.lossyScale;
            go.transform.localScale = new Vector3(
                p.x != 0 ? w.x / p.x : w.x,
                p.y != 0 ? w.y / p.y : w.y,
                p.z != 0 ? w.z / p.z : w.z);

            go.AddComponent<MeshFilter>().sharedMesh = mf.sharedMesh;
            var mr = go.AddComponent<MeshRenderer>();
            mr.sharedMaterials = new Material[mf.sharedMesh.subMeshCount];
            mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            ghost.renderers.Add(mr);
        }

        return ghost;
    }
}