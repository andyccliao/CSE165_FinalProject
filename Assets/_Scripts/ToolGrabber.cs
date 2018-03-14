using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolGrabber : OVRGrabber {

    protected ToolScript lastTool = null;

    protected override void GrabBegin()
    {
        base.GrabBegin();

        if (lastTool != null) {
            lastTool.enabled = false;
        }

        lastTool = m_grabbedObj.GetComponent<ToolScript>();

        if (lastTool != null) {
            lastTool.enabled = true;
        }
    }
}
