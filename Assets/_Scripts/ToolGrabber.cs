using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolGrabber : OVRGrabber {

    protected ToolScript lastTool = null;
    //public MoveScript moveScript;

    protected override void GrabBegin()
    {
        base.GrabBegin();
        if (m_grabbedObj == null) {

            return;
        }
        if (lastTool != null) {
            lastTool.enabled = false;
        }

        lastTool = m_grabbedObj.GetComponent<ToolScript>();

        if (lastTool != null) {
            lastTool.enabled = true;
        }
    }

    public OVRInput.Controller Controller
    {
        get { return m_controller; }
    }

    public void GetVelocityAndAngularVelocity(out Vector3 linearVelocity, out Vector3 angularVelocity)
    {
        OVRPose localPose = new OVRPose { position = OVRInput.GetLocalControllerPosition(m_controller), orientation = OVRInput.GetLocalControllerRotation(m_controller) };
        OVRPose offsetPose = new OVRPose { position = m_anchorOffsetPosition, orientation = m_anchorOffsetRotation };
        localPose = localPose * offsetPose;

        OVRPose trackingSpace = transform.ToOVRPose() * localPose.Inverse();
        linearVelocity = trackingSpace.orientation * OVRInput.GetLocalControllerVelocity(m_controller);
        angularVelocity = trackingSpace.orientation * OVRInput.GetLocalControllerAngularVelocity(m_controller);
    }

    public Vector3 Velocity
    {
        get {
            OVRPose localPose = new OVRPose { position = OVRInput.GetLocalControllerPosition(m_controller), orientation = OVRInput.GetLocalControllerRotation(m_controller) };
            OVRPose offsetPose = new OVRPose { position = m_anchorOffsetPosition, orientation = m_anchorOffsetRotation };
            localPose = localPose * offsetPose;

            OVRPose trackingSpace = transform.ToOVRPose() * localPose.Inverse();
            return trackingSpace.orientation * OVRInput.GetLocalControllerVelocity(m_controller);
        }
    }
}
