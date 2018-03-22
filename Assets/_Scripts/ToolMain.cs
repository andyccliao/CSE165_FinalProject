using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolMain : ToolScript {
    // Stick tilting threshold
    public float tiltThreshold = 0.7f;

    private ToolGrabbable tg;
    //private Rigidbody rb;
    public GameObject tip;

    //Electricity
    public bool lightningActivated = false;
    public Transform lightningScale;
    public Vector3 offScale = new Vector3(1.0f, 0.001f, 1.0f);
    public Vector3 onScale = Vector3.one;
    public Light lightningLight;
    public AudioSource lightningSound;

    //Pole
    public float scaleSpeed = 2.0f;
    public Transform poleScale;
    public Vector3 shrunkenScale = new Vector3(0.1f, 0.001f, 0.1f);
    public Vector3 extendedScale = Vector3.one;
    private bool poleExtended;

    public CharacterController playerCC;
    public OVRPlayerController playerOVR;
    public Transform centerEyeTransform;
    public float pushMagnitude = 10.0f;
    private Vector3 movement;
    public float gravity = 9.8f;

    private Vector3 touchPosition = Vector3.zero;
    protected Dictionary<Collider, int> colliders = new Dictionary<Collider, int>();

    public AudioClip lowHit;
    public AudioSource groundHit;

    public Material neonBlue;

    public bool wayfindingOn = false;

    private void Awake()
    {
        tg = GetComponent<ToolGrabbable>();
        poleScale.localScale = shrunkenScale;
        lightningLight.enabled = false;
        lightningScale.localScale = offScale;
        lightningSound.enabled = false;

        playerOVR.PreCharacterMove += CalculateToolMovement;
    }
    private void OnEnable () {

	}

    private void OnDisable()
    {
        poleScale.localScale = shrunkenScale;
        LightningOff();
    }

    void Update () {
        if (tg.GrabbedBy == null) {
            Debug.Log("GrabbedBy is null");
            return;
        }
        OVRInput.Controller controller = tg.GrabbedBy.Controller;
        Vector2 stickTilt = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, controller);

        if (stickTilt.y > tiltThreshold) {      //Tilt up
            LightningOn();
        }
        else if (stickTilt.y < -tiltThreshold) {//Tilt down
            PoleOn();
        }
        else {
            LightningOff();
            PoleOff();
        }
        { 
            //if (playerCC.isGrounded) {
            //    movement -= 3.0f * movement * Time.deltaTime;
            //    if (movement.y < 0) movement.y = 0;
            //}
            //playerCC.Move(movement * Time.deltaTime);
            //movement.y -= gravity * Time.deltaTime;
        }
        //if (triggerTimer > 0) triggerTimer -= Time.deltaTime;
    }

    private void CalculateToolMovement()
    {
        //if (isActiveAndEnabled) {
            if (movement.magnitude > 0.1) {
                if (playerCC.isGrounded)
                    movement -= 0.5f * movement * Time.deltaTime;
                if (tg.isGrabbed) {
                    playerCC.Move(movement * Time.deltaTime);
                    
                }
            }
        //}
    }

    private void PoleOff()
    {
        poleScale.localScale = Vector3.Lerp(poleScale.localScale, shrunkenScale, scaleSpeed * Time.deltaTime);
        poleExtended = false;
    }

    private void PoleOn()
    {
        poleScale.localScale = Vector3.Lerp(poleScale.localScale, extendedScale, scaleSpeed * Time.deltaTime);
        poleExtended = true;
    }

    private void LightningOff()
    {
        lightningActivated = false;
        lightningScale.localScale = Vector3.Lerp(lightningScale.localScale, offScale, scaleSpeed * Time.deltaTime);

        lightningLight.enabled = false;
        lightningLight.intensity = 0f;

        lightningSound.enabled = false;
        lightningSound.pitch = 0.1f;
    }

    private void LightningOn()
    {
        lightningActivated = true;
        lightningScale.localScale = Vector3.Lerp(lightningScale.localScale, onScale, scaleSpeed * Time.deltaTime);

        lightningLight.enabled = true;
        lightningLight.intensity *= 1f - (lightningScale.localScale - onScale).magnitude;

        lightningSound.enabled = true;
        lightningSound.Play();
        if (lightningSound.pitch < 0.4) lightningSound.pitch += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground")) {
            groundHit.Play();
            if (poleExtended && colliders.Count == 0) {
                touchPosition = tip.transform.position;
            }
            int refCount = 0;
            colliders.TryGetValue(other, out refCount);
            colliders[other] = refCount + 1;

            OVRHapticsClip hapticsClip = new OVRHapticsClip(lowHit);
            if (tg.GrabbedBy != null && tg.GrabbedBy.Controller == OVRInput.Controller.RTouch) OVRHaptics.RightChannel.Preempt(hapticsClip);
            if (tg.GrabbedBy != null && tg.GrabbedBy.Controller == OVRInput.Controller.LTouch) OVRHaptics.LeftChannel.Preempt(hapticsClip);
        }

        if (other.CompareTag("Plug")) {
            wayfindingOn = true;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (poleExtended && other.CompareTag("Ground")) {
            //if (lastPosition.Equals(touchPosition)) {
            movement += (touchPosition - tip.transform.position);
            movement /= 1.25f;
            //movement.y *= 1.5f;
            //}
            //else {
            //    movement += (touchPosition - tip.transform.position);
            //}
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ground")) {
            int refCount = 0;
            bool found = colliders.TryGetValue(other, out refCount);
            if (!found) return;

            if (refCount > 1) {
                colliders[other] = refCount - 1;
            }
            else {
                colliders.Remove(other);

                //playerRB.velocity = (-(tip.transform.position - lastPosition) / Time.deltaTime);
            }
        }

        if (other.CompareTag("Plug")) {
            wayfindingOn = false;
        }
    }
}
