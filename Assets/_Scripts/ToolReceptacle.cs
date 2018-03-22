using System;
using System.Collections.Generic;
using UnityEngine;

public class ToolReceptacle : MonoBehaviour {

    public event Action OnActivate;

    private void Start()
    {
    }

    void FixedUpdate () {
        //Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position, transform.localScale / 2, Quaternion.identity, LayerMask.NameToLayer("Receptacle"));
    }

    private void Update()
    {
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Plug")) {
            var toolMain = other.GetComponentInParent<ToolMain>();
            Debug.Assert(toolMain != null, "Plug is not attached to a ToolMain!");

            if (toolMain.lightningActivated) {
                if (OnActivate != null) {
                    OnActivate();
                }
            }
        }
    }
}
