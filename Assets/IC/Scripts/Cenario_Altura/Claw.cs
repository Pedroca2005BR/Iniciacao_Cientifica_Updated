using Oculus.Interaction.HandGrab;
using UnityEngine;

public class Claw : MonoBehaviour, IHandGrabUseDelegate
{
    [SerializeField] Transform _trigger;
    [SerializeField] AnimationCurve _triggerRotationCurve;
    [SerializeField][Range(0f, 1f)] float _fireThresold;
    [SerializeField][Range(0f, 1f)] float _releaseThresold;
    [SerializeField] float _triggerSpeed;
    [SerializeField] AnimationCurve _strengthCurve;

    private bool _wasFired = false;
    private float _dampedUseStrength = 0;
    private float _lastUseTime;

    public void BeginUse()
    {
        _dampedUseStrength = 0f;
        _lastUseTime = Time.time;
    }

    public float ComputeUseStrength(float strength)
    {
        float delta = Time.time - _lastUseTime;
        _lastUseTime = Time.time;
        if (strength > _dampedUseStrength)
        {
            _dampedUseStrength = Mathf.Lerp(_dampedUseStrength, strength, _triggerSpeed * delta);
        }
        else
        {
            _dampedUseStrength = strength;
        }
        float progress = _strengthCurve.Evaluate(_dampedUseStrength);
        UpdateTriggerProgress(progress);
        return progress;
    }

    public void EndUse()
    {
        
    }

    private void UpdateTriggerProgress(float progress)
    {
        UpdateTriggerRotation(progress);

        if (progress >= _fireThresold && !_wasFired)
        {
            _wasFired = true;
            Debug.Log("FIREEEEEE!");
        }
        else if (progress <= _releaseThresold)
        {
            _wasFired = false;
            Debug.Log("WATERRRRR!");
        }
    }

    private void UpdateTriggerRotation(float progress)
    {
        float value = _triggerRotationCurve.Evaluate(progress);
        Vector3 angles = _trigger.localEulerAngles;
        _trigger.localEulerAngles = angles;
    }
}
