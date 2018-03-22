using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BPresser : MonoBehaviour {

    public Collider indexVolume;
    [SerializeField]
    protected OVRInput.Controller controller;
    public OculusHaptics haptics;

    // Use this for initialization
    void Start () {
        Debug.Assert(indexVolume != null, "Need indexVolume!");
    }
	
	// Update is called once per frame
	void Update () {
        // If index finger is up (near touch is up, so is false), activate collider
		if (OVRInput.Get(OVRInput.NearTouch.PrimaryIndexTrigger, controller) == false) {
            indexVolume.enabled = true;
        }

        else {
            indexVolume.enabled = false;
        }    
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BButton")) {
            haptics.Vibrate(VibrationForce.Light, controller);
        }
    }
}
