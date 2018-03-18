using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class Flicker : MonoBehaviour {

    Light light;
    [Range(0.0f, 1.0f)]
    public float min = 0.0f;
    [Range(0.0f, 1.0f)]
    public float max = 1.0f;
    [Range(1, 50)]
    public int smoothness = 2;

    // Array of random values for the intensity.
    private Queue<float> smoothing = new Queue<float>();


    // Use this for initialization
    void Start () {
        light = GetComponent<Light>();
        // Initialize the array.
         for (int i = 0; i < smoothness; i++) {
            smoothing.Enqueue(0.0f);
        }
    }
	
	// Update is called once per frame
	void Update () {
        // Inspired by https://answers.unity.com/questions/34739/how-to-make-a-light-flicker.html
        if (light.isActiveAndEnabled) {
            if (smoothing.Count >= smoothness) {
                smoothing.Dequeue();
            }
            if (smoothing.Count <= smoothness) {
                var randVal = Random.Range(min, max);
                smoothing.Enqueue(randVal);
            }
            light.intensity = smoothing.Average();
        }
	}
}
