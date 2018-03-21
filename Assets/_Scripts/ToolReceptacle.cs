using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolReceptacle : MonoBehaviour {

    public LCDDisplay lcd;
    public Vector3 offset;

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
                lcd.ShowNumber();
            }
        }
    }
}
