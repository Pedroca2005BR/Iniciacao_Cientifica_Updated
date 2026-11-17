using Oculus.Interaction;
using UnityEngine;

public class SnapInteractorReleaseOnGrab : SnapInteractor
{
    [SerializeField] private bool logCount = false;

    protected override bool ComputeShouldUnselect()
    {
        if (base.ComputeShouldUnselect()) return true;

        var pe = PointableElement as Oculus.Interaction.PointableElement;
        if (pe == null) return false;

        if (logCount) Debug.LogError($"[{name}] SelectingPointsCount={pe.SelectingPointsCount}");

        // o ponto do próprio snap conta como 1; qualquer ponto a mais é uma mão agarrando
        return pe.SelectingPointsCount > 1;
    }
}