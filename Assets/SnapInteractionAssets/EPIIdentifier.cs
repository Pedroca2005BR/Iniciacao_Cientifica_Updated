using Oculus.Interaction;
using Pedroca2005BR.Utilities;
using UnityEngine;

public class EPIIdentifier : MonoBehaviour
{
    public EPIType epiType;
    public SnapInteractor snapInteractor;

    public void Select()
    {
        Debug.LogError("Selected!");
        EventManager.TriggerEvent(ConstantEventNames.ITEM_PICKED_UP, this);
    }

    public void Deselect()
    {
        Debug.LogError("Deselected!");
        EventManager.TriggerEvent(ConstantEventNames.ITEM_DROPPED, this);
    }
}
