using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolHandle : ToolScript
{
    Material material;
    Color originalColor;
    public static float gripTime = 5.0f;
    ToolGrabbable tg;
    AudioSource audio;

    public CharacterController playerCC;
    Vector3 grabbedPosition = Vector3.zero;


    // Use this for initialization
    void Awake () {
        material = GetComponent<Renderer>().material;
        originalColor = material.color;
        tg = GetComponent<ToolGrabbable>();
        audio = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        // ** DEFAULT OVRGRABBER GRABBING MOVES THE OBJECT, I DON'T WANT THAT FOR HANDLES **

        if (tg.GrabbedBy != null) {
            //playerCC.Move(-tg.GrabbedBy.Velocity * Time.deltaTime);
            playerCC.Move(tg.grabbedTransform.position - tg.grabbedBy.transform.position);
        }
	}

    private void OnEnable()
    {
        tg.canMoveMe = false;
        audio.Play();
        //material.color
    }

    private void OnDisable()
    {
        //tg.canMoveMe = true;
    }
}
