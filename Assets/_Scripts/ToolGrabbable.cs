using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ToolGrabbable : OVRGrabbable {

    [SerializeField]
    protected ToolScript ts;
    [SerializeField]
    protected ToolBelt tb;

    protected override void Start()
    {
        base.Start();
        Debug.AssertFormat(ts != null, "{0} needs a ToolScript.", this.ToString());
        ts.enabled = false;

        //tb.
    }

    private void Update()
    {
        
    }

    override public void GrabBegin(OVRGrabber hand, Collider grabPoint)
    {
        //tb.
        base.GrabBegin(hand, grabPoint);
        ts.enabled = true;
    }

    override public void GrabEnd(Vector3 linearVelocity, Vector3 angularVelocity)
    {
        base.GrabEnd(linearVelocity, angularVelocity);
        ts.enabled = false;
    }

    public ToolGrabber GrabbedBy
    {
        get { return (m_grabbedBy is ToolGrabber) ? (ToolGrabber)m_grabbedBy : null; }
    }
}
