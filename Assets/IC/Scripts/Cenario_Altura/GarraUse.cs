using Oculus.Interaction.HandGrab;
using Pedroca2005BR.Utilities;
using UnityEngine;

public class GarraUse : MonoBehaviour, IHandGrabUseDelegate
{
    [SerializeField] Transform parteMovel;
    [SerializeField] float maxAngle = -45f;

    // Runtime variables
    bool wasPickedUp = false;

    public void BeginUse() { }

    public float ComputeUseStrength(float strength)
    {
        parteMovel.localRotation = Quaternion.Euler(0, 0, maxAngle * strength);

        // Depending on the strength of the grip, it triggers a picked up or dropped event, heard by the phantom counterparts
        if (strength > 0.3f && !wasPickedUp)
        {
            //EventManager.TriggerEvent(ConstantEventNames.ITEM_PICKED_UP, this);
            Debug.LogError("Picked!");
            wasPickedUp = true;
        }
        else if (strength <= 0.3f && wasPickedUp)
        {
            //EventManager.TriggerEvent(ConstantEventNames.ITEM_DROPPED, this);
            Debug.LogError("Dropped :3!");
            wasPickedUp = false;
        }

        return strength;
    }

    public void EndUse()
    {
        if (wasPickedUp)
        {
            //EventManager.TriggerEvent(ConstantEventNames.ITEM_DROPPED, this);
            Debug.LogError("Dropped :3!");
            wasPickedUp = false;
        }

        parteMovel.localRotation = Quaternion.Euler(0, 0, 0);
    }
}
