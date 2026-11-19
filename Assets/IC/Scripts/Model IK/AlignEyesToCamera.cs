using UnityEngine;

[DefaultExecutionOrder(1000)] // roda depois do CharacterRetargeter
public class AlignEyesToCamera : MonoBehaviour
{
    [SerializeField] Transform centerEye;   // OVRCameraRig/TrackingSpace/CenterEyeAnchor
    [SerializeField] Transform headBone;    // osso da cabeça do personagem
    [SerializeField] Vector3 eyeLocalOffset = new Vector3(0f, 0.08f, 0.09f); // ajuste no Inspector
    [SerializeField] bool hideHead = true;

    void LateUpdate()
    {
        // posição dos "olhos" do personagem, ignorando a escala do osso
        Vector3 eyes = headBone.position + headBone.rotation * eyeLocalOffset;
        transform.position += centerEye.position - eyes;

        if (hideHead) headBone.localScale = Vector3.zero;
    }
}
