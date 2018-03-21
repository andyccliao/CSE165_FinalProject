using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolWayfindingController : MonoBehaviour {

    public ToolMain left;
    public ToolMain right;
    private float timeElapsed = 0;
    public DigitalRuby.LightningBolt.LightningBoltScript lightning;
    public AudioSource lightningSound;

    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        if (left.lightningActivated && right.lightningActivated) {
            if (left.wayfindingOn && right.wayfindingOn) {
                lightning.Trigger();
                lightningSound.transform.position = (lightning.StartPosition + lightning.EndPosition) / 2;
                if (timeElapsed <= 0) {
                    timeElapsed = Random.Range(0.2f, 0.6f);
                    lightningSound.Play();
                }
                else {
                    timeElapsed -= Time.deltaTime;
                }

            }
            else {
                timeElapsed = 0;
                lightningSound.Stop();
            }
        }
        else {
            lightningSound.Stop();
        }
    }
}
