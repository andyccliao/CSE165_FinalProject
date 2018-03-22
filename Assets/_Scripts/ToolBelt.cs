using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ToolBelt : MonoBehaviour {

    public Transform[] toolLocations;
    public ToolGrabbable[] tools;
    protected Rigidbody[] toolRb;
    protected bool[] toolIsKinematic;
    public Transform beltOffsetFromHead;
    public Transform centerEyeTransform;
    public OvrAvatar ovrAvatar;

    Quaternion prevRotation = Quaternion.identity;
    public float returnSpeed = 5.0f;

    // Use this for initialization
    void Start()
    {
        Debug.Assert(toolLocations.Length == tools.Length, "Number of tools and toolLocations do not match.");
        UpdateBeltPosition();

        toolRb = new Rigidbody[tools.Length];
        toolIsKinematic = new bool[tools.Length];

        for (int i = 0; i < tools.Length; i++) {
            if (tools[i] == null) continue;
            if (!tools[i].isGrabbed) {
                toolRb[i] = tools[i].GetComponent<Rigidbody>();
                toolRb[i].position = (toolLocations[i].position);

                toolIsKinematic[i] = toolRb[i].isKinematic;
            }
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateBeltPosition();
        UpdateAttachedToolPositions();
    }

    void UpdateBeltPosition ()
    {
        // Move Belt down
        Vector3 position = centerEyeTransform.position;
        position += beltOffsetFromHead.position;


        //Rotate following head, but have some leeway
        Vector3 rotationy = Vector3.zero;
        rotationy.y = centerEyeTransform.rotation.eulerAngles.y;

        float lerpSpeed = 4;
        float angle = Vector3.Angle(Vector3.down, centerEyeTransform.forward);
        if (angle < 45) {
            lerpSpeed *= angle / 90;
        }
        Quaternion rotation = Quaternion.Lerp(prevRotation, Quaternion.Euler(rotationy), lerpSpeed * Time.deltaTime);
        prevRotation = rotation;

        //Adjust for head tilt
        Quaternion headTilt= Quaternion.FromToRotation(Vector3.up, centerEyeTransform.up);
        Vector3 tiltOffset = headTilt * Vector3.down;
        position.x += 0.1f * tiltOffset.x;
        position.z += 0.05f * tiltOffset.z;

        transform.SetPositionAndRotation(position, rotation);
    }

    void UpdateAttachedToolPositions()
    {
        for (int i=0; i<tools.Length; i++) {
            if (tools[i] == null) continue;
            if (!tools[i].isGrabbed) {
                // if close, follow belt
                if ((tools[i].transform.position - toolLocations[i].position).magnitude < 0.5f) {
                    toolRb[i].isKinematic = true;
                    toolRb[i].MovePosition(toolLocations[i].position);
                    toolRb[i].MoveRotation(toolLocations[i].rotation);
                    toolRb[i].velocity = Vector3.zero;
                }
                else {
                    toolRb[i].isKinematic = false;

                    toolRb[i].AddForce(40 * (toolLocations[i].position - tools[i].transform.position).normalized + 60 * (toolLocations[i].position - tools[i].transform.position));
                    //tools[i].GetComponent<Rigidbody>().MovePosition(tools[i].transform.position + 0.5f * (toolLocations[i].position - tools[i].transform.position));
                }
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
