using Oculus.Interaction.HandGrab;
using UnityEngine;

public class ExtintorUse : MonoBehaviour, IHandGrabUseDelegate
{
    [SerializeField] private ParticleSystem jato;
    [SerializeField] private Transform alavanca;      // visual
    [SerializeField] private float anguloMax = 20f;

    public bool PinoRemovido { get; set; }
    bool isActive = false;

    public void BeginUse() { }
    public void EndUse()
    {
        if (isActive)
        {
            jato.Stop();
            isActive = false;
        }

        alavanca.localRotation = Quaternion.Euler(0, 0, 0); 
    }

    public float ComputeUseStrength(float strength)
    {
        float s = PinoRemovido ? strength : 0f;   // sem pino, não aperta
        alavanca.localRotation = Quaternion.Euler(0, 0, anguloMax * s);

        if (s > 0.3f && !isActive)
        {
            isActive = true;
            jato.Play();
        }
        else if (s <= 0.3f && isActive)
        {
            isActive = false;
            jato.Stop();
        }
        return s;
    }
}