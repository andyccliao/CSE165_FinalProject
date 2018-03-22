using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ToolGrabbable))]
public abstract class ToolScript : MonoBehaviour {

    public virtual void GrabEnd(Vector3 linearVelocity, Vector3 angularVelocity)
    {

    }
}
