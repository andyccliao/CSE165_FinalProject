using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolWayfindingController : MonoBehaviour {

    public ToolMain left;
    public ToolMain right;
    public ToolReceptacle receptacle;
    private float timeCountdown = 0;
    private float timeCountdownLeft = 0;
    private float timeCountdownRight = 0;
    public DigitalRuby.LightningBolt.LightningBoltScript lightning;
    public AudioSource lightningSound;
    public AudioSource receptacleSound;

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
                if (timeCountdown <= 0) {
                    timeCountdown = Random.Range(0.2f, 0.6f);
                    lightningSound.Play();
                }
                else {
                    timeCountdown -= Time.deltaTime;
                }

            }
            else {
                timeCountdown = 0;
                lightningSound.Stop();
            }
        }
        else {
            lightningSound.Stop();

            if (left.lightningActivated) {
                float leftDist = Vector3.Distance(left.transform.position, receptacle.transform.position);
                if (leftDist < 1) {
                    if (timeCountdownLeft <= 0) {
                        timeCountdownLeft = leftDist/2;
                        GameObject saveStart = lightning.StartObject;
                        GameObject saveEnd = lightning.EndObject;

                        lightning.StartObject = left.gameObject;
                        lightning.EndObject = receptacle.gameObject;
                        lightning.Trigger();
                        receptacleSound.pitch = Random.Range(0.7f, 1.3f);
                        receptacleSound.Play();

                        lightning.StartObject = saveStart;
                        lightning.EndObject = saveEnd;
                    }
                    else {
                        timeCountdownLeft -= Time.deltaTime;
                    }
                }
            }
            if (right.lightningActivated) {
                float rightDist = Vector3.Distance(right.transform.position, receptacle.transform.position);
                if (rightDist < 1) {
                    if (timeCountdownRight <= 0) {
                        timeCountdownRight = rightDist/2;
                        GameObject saveStart = lightning.StartObject;
                        GameObject saveEnd = lightning.EndObject;

                        lightning.StartObject = right.gameObject;
                        lightning.EndObject = receptacle.gameObject;
                        lightning.Trigger();
                        receptacleSound.pitch = Random.Range(0.7f, 1.3f);
                        receptacleSound.Play();

                        lightning.StartObject = saveStart;
                        lightning.EndObject = saveEnd;
                    }
                    else {
                        timeCountdownRight -= Time.deltaTime;
                    }
                }
            }
        }
    }
}
