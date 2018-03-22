using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolHandle : ToolScript
{
    Material material;
    Color originalColor;
    public static float gripTime = 5.0f;
    ToolGrabbable tg;
    new AudioSource audio;
    float gravityModifierCC;

    public CharacterController playerCC;
    public OVRPlayerController playerOVR;
    Vector3 grabbedPosition = Vector3.zero;

    static ToolHandle mainHandle;


    // Use this for initialization
    void Awake () {
        material = GetComponent<Renderer>().material;
        originalColor = material.color;
        tg = GetComponent<ToolGrabbable>();
        audio = GetComponent<AudioSource>();
        gravityModifierCC = playerOVR.GravityModifier;
        //Debug.Log(gravityModifierCC);

        playerOVR.PreCharacterMove += MoveCharacterPosition;
	}

    private void MoveCharacterPosition()
    {
        if (isActiveAndEnabled) {
            if (tg.isGrabbed && mainHandle == this) {
                playerOVR.GravityModifier = 0;
                playerOVR.ZeroFallSpeed();
                playerCC.Move(tg.grabbedTransform.position - tg.grabbedBy.transform.position);
            }
        }
    }

    // Update is called once per frame
    void Update () {

        // ** DEFAULT OVRGRABBER GRABBING MOVES THE OBJECT, I DON'T WANT THAT FOR HANDLES **
        //playerOVR.GravityModifier = 0;

        //if (tg.GrabbedBy != null) {
        //    //playerCC.Move(-tg.GrabbedBy.Velocity * Time.deltaTime);

        //    playerCC.Move(tg.grabbedTransform.position - tg.grabbedBy.transform.position);
        //    //Debug.Log("Target: " + tg.grabbedTransform.position);
        //    //Debug.Log("ControllerPos: " + tg.grabbedBy.transform.position);
        //}
    }

    private void OnEnable()
    {
        tg.canMoveMe = false;
        audio.Play();

        mainHandle = this;
        //material.color
    }

    private void OnDisable()
    {
        if (mainHandle == this) {
            playerOVR.GravityModifier = gravityModifierCC;
            mainHandle = null;
        }
        //tg.canMoveMe = true;
    }

    public override void GrabEnd(Vector3 linearVelocity, Vector3 angularVelocity)
    {
        //if (mainHandle == this) {
        //    playerOVR.Jump(-linearVelocity);
        //}
    }
}
