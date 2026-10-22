using Oculus.Interaction;
using UnityEngine;

public class PinoExtintor : MonoBehaviour
{
    [SerializeField] private Transform encaixe;
    [SerializeField] private ExtintorUse extintor;
    [SerializeField] private float distanciaSoltar = 0.08f;
    private bool removido;

    [Header("Grab Poses")]
    [SerializeField] GameObject[] comTrava; 
    [SerializeField] GameObject[] semTrava;

    Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (removido) return;
        if (Vector3.Distance(transform.position, encaixe.position) > distanciaSoltar)
        {
            removido = true;
            //UnlockNewGrabPoses();
            //Debug.Log("Pino Removido!");
            extintor.PinoRemovido = true;
            transform.SetParent(null);
            
            
            RigidbodyKinematicLockerExtension.UnlockKinematic(rb);
            rb.isKinematic = false;
        }
    }

    void UnlockNewGrabPoses()
    {
        foreach(var go in comTrava)
        {
            go.SetActive(false);
        }

        foreach(var go in semTrava)
        {
            go.SetActive(true);
        }
    }
}
