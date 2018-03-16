using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolMain : ToolScript {
    // Stick tilting threshold
    public float tiltThreshold = 0.7f;

    private ToolGrabbable tg;
    private Rigidbody rb;
    public GameObject tip;

    public float scaleSpeed = 2.0f;
    public Transform poleScale;
    public Vector3 shrunkenScale = new Vector3(0.1f, 0.001f, 0.1f);
    public Vector3 extendedScale = Vector3.one;


    public GameObject player;
    CharacterController playerCC; 
    private Vector3 movement;
    public float pushMagnitude = 10.0f;
    private float gravity = 9.8f;

    private float triggerTimer = 0.0f;

    private void Awake()
    {
        tg = GetComponent<ToolGrabbable>();
        rb = GetComponent<Rigidbody>();
        poleScale.localScale = shrunkenScale;
        playerCC = player.GetComponent<CharacterController>();
    }
    private void OnEnable () {

	}

    private void OnDisable()
    {
        poleScale.localScale = shrunkenScale;
    }

    void Update () {
        if (tg.GrabbedBy == null) {
            Debug.Log("GrabbedBy is null");
            return;
        }
        OVRInput.Controller controller = tg.GrabbedBy.controller;
        Vector2 stickTilt = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, controller);

        if (stickTilt.y > tiltThreshold) {      //Tilt up

        }
        else if (stickTilt.y < -tiltThreshold) {//Tilt down
            poleScale.localScale = Vector3.Lerp(poleScale.localScale, extendedScale, scaleSpeed * Time.deltaTime);
        }
        else {
            poleScale.localScale = Vector3.Lerp(poleScale.localScale, shrunkenScale, scaleSpeed * Time.deltaTime);
        }

        if (movement.magnitude > 0.01) {
            if (playerCC.isGrounded) {
                movement *= 0.7f;
                if(movement.y < 0) movement.y = 0;
            }
            playerCC.Move(movement * Time.deltaTime);
            movement *= 0.99f;
            movement.y -= gravity * Time.deltaTime;
        }
        if (triggerTimer > 0) triggerTimer -= Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (triggerTimer <= 0.0f) {
            if (other.CompareTag("Ground")) {
                //Debug.Log(other.ToString());
                //Debug.Log(rb.velocity);
                //https://docs.unity3d.com/ScriptReference/Rigidbody.GetPointVelocity.html
                Vector3 tipVelocity = rb.GetRelativePointVelocity(tip.transform.position);
                Debug.Log(tipVelocity);

                movement = transform.TransformDirection(-tipVelocity * pushMagnitude);
                //rb.AddTorque(transform.up * new Vector2(-rb.velocity.x, -rb.velocity.z).magnitude);

                triggerTimer = 0.2f;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        
    }

}
