using Oculus.Interaction.HandGrab;
using UnityEngine;

public class ExtintorUse : MonoBehaviour, IHandGrabUseDelegate
{
    [SerializeField] private ParticleSystem jato;
    [SerializeField] private Transform alavanca;      // visual
    [SerializeField] private float anguloMax = 20f;

    public bool PinoRemovido { get; set; }

    public void BeginUse() { }
    public void EndUse() { jato.Stop(); }

    public float ComputeUseStrength(float strength)
    {
        float s = PinoRemovido ? strength : 0f;   // sem pino, não aperta
        alavanca.localRotation = Quaternion.Euler(0, 0, anguloMax * s);

        if (s > 0.3f && !jato.isPlaying) jato.Play();
        else if (s <= 0.3f && jato.isPlaying) jato.Stop();
        return s;
    }
}