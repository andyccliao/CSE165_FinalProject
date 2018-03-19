using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolHandle : ToolScript
{
    Material material;
    Color originalColor;
    public static float gripTime = 5.0f;
    ToolGrabbable tg;

    public CharacterController playerCC;


    // Use this for initialization
    void Start () {
        material = GetComponent<Renderer>().material;
        originalColor = material.color;
        tg = GetComponent<ToolGrabbable>();
	}
	
	// Update is called once per frame
	void Update () {
        // Make new class for Handle, and new code for ToolGrabber
        // Probably let ToolGrabber move the player
        // ** DEFAULT OVRGRABBER GRABBING MOVES THE OBJECT, I DON'T WANT THAT FOR HANDLES **

        //if (tg.GrabbedBy) {
        //    playerCC.Move(tg.GrabbedBy.transform.parent.TransformVector(OVRInput.GetLocalControllerVelocity(tg.GrabbedBy.Controller)));
        //}
	}

    private void OnEnable()
    {
        //material.color
    }
}
