using Oculus.Interaction;
using UnityEngine;

public class DebugPointable : MonoBehaviour
{
    [SerializeField] private Grabbable grabbable;
    [SerializeField] private SnapInteractor snapInteractor;

    void OnEnable()
    {
        grabbable.WhenPointerEventRaised += OnPointer;
        if (snapInteractor) snapInteractor.WhenStateChanged += OnSnap;
        Debug.Log($"[{name}] ENABLED f={Time.frameCount}");
    }

    void OnDisable()
    {
        grabbable.WhenPointerEventRaised -= OnPointer;
        if (snapInteractor) snapInteractor.WhenStateChanged -= OnSnap;
        Debug.Log($"[{name}] DISABLED f={Time.frameCount}");
    }

    void OnPointer(PointerEvent e)
    {;
    }

    //Debug.Log($"<color=yellow>[{name}] POINTER {e.Type} id={e.Identifier} f={Time.frameCount}</color>");

    void OnSnap(InteractorStateChangeArgs a) =>
        Debug.Log($"<color=green>[{name}] SNAP {a.PreviousState}->{a.NewState} f={Time.frameCount}</color>");
}