using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Edited from:-
// https://answers.unity.com/questions/1375915/how-to-implement-oculus-vr-haptic-feedback.html

public enum VibrationForce
{
    Light,
    Medium,
    Hard,
}


public class OculusHaptics : MonoBehaviour
{

    private OVRHapticsClip clipLight;
    private OVRHapticsClip clipMedium;
    private OVRHapticsClip clipHard;

    public float LowViveHaptics { get; private set; }
    public float MediumViveHaptics { get; private set; }
    public float HardViveHaptics { get; private set; }


    private void Start()
    {
        InitializeOVRHaptics();
    }

    private void InitializeOVRHaptics()
    {

        int cnt = 10;
        clipLight = new OVRHapticsClip(cnt);
        clipMedium = new OVRHapticsClip(cnt);
        clipHard = new OVRHapticsClip(cnt);
        for (int i = 0; i < cnt; i++) {
            clipLight.Samples[i] = i % 2 == 0 ? (byte)0 : (byte)45;
            clipMedium.Samples[i] = i % 2 == 0 ? (byte)0 : (byte)100;
            clipHard.Samples[i] = i % 2 == 0 ? (byte)0 : (byte)180;
        }

        clipLight = new OVRHapticsClip(clipLight.Samples, clipLight.Samples.Length);
        clipMedium = new OVRHapticsClip(clipMedium.Samples, clipMedium.Samples.Length);
        clipHard = new OVRHapticsClip(clipHard.Samples, clipHard.Samples.Length);
    }


    void OnEnable()
    {
        InitializeOVRHaptics();
    }

    public void Vibrate(VibrationForce vibrationForce, OVRInput.Controller controllerMask)
    {
        var channel = OVRHaptics.RightChannel;
        if (controllerMask == OVRInput.Controller.LTouch)
            channel = OVRHaptics.LeftChannel;

        switch (vibrationForce) {
            case VibrationForce.Light:
                channel.Preempt(clipLight);
                break;
            case VibrationForce.Medium:
                channel.Preempt(clipMedium);
                break;
            case VibrationForce.Hard:
                channel.Preempt(clipHard);
                break;
        }
    }



    public IEnumerator VibrateTime(VibrationForce force, float time, OVRInput.Controller controllerMask)
    {
        var channel = OVRHaptics.RightChannel;
        if (controllerMask == OVRInput.Controller.LTouch)
            channel = OVRHaptics.LeftChannel;

        for (float t = 0; t <= time; t += Time.deltaTime) {
            switch (force) {
                case VibrationForce.Light:
                    channel.Queue(clipLight);
                    break;
                case VibrationForce.Medium:
                    channel.Queue(clipMedium);
                    break;
                case VibrationForce.Hard:
                    channel.Queue(clipHard);
                    break;
            }
        }
        yield return new WaitForSeconds(time);
        channel.Clear();
        yield return null;
    }
}