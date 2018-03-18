using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ToolBelt : MonoBehaviour {

    public Transform[] toolLocations;
    public ToolGrabbable[] tools;
    public Transform beltOffsetFromHead;
    public Transform centerEyeTransform;
    public OvrAvatar ovrAvatar;

	// Use this for initialization
	void Start () {
        Debug.Assert(toolLocations.Length == tools.Length);
	}
	
	// Update is called once per frame
	void Update () {
        UpdateBeltPosition();
        UpdateAttachedToolPositions();
    }

    void UpdateBeltPosition ()
    {
        // Move Belt down
        Vector3 position = centerEyeTransform.position;
        position += beltOffsetFromHead.position;

        //Rotate following head
        Vector3 rotationy = Vector3.zero;
        rotationy.y = centerEyeTransform.rotation.eulerAngles.y;
        Quaternion rotation = Quaternion.Euler(rotationy);

        //Adjust for head tilt
        Quaternion headTilt= Quaternion.FromToRotation(Vector3.up, centerEyeTransform.up);
        Vector3 tiltOffset = headTilt * Vector3.down;
        position.x += tiltOffset.x;
        position.z += tiltOffset.z;

        transform.SetPositionAndRotation(position, rotation);
    }

    void UpdateAttachedToolPositions()
    {
        for (int i=0; i<tools.Length; i++) {
            if (!tools[i].isGrabbed) {
                tools[i].grabbedRigidbody.MovePosition(toolLocations[i].position);
                tools[i].grabbedRigidbody.MoveRotation(toolLocations[i].rotation);
            }
        }
    }
}

//Part of OvrAvatar, if needed, add to its script
//public Transform GetBodyTransform()
//{
//    OvrAvatarComponent component;
//    if (trackedComponents.TryGetValue("body", out component)) {
//        if (component.RenderParts.Count > 0) {
//            Debug.Log("Body has renderpart(s)");
//            return component.RenderParts[0].transform;
//        }
//        else {
//            Debug.Log("OKAOKA");
//        }
//    }

//    return null;
//}
