using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LCDDisplay : MonoBehaviour {

    private bool giveCorrectNumber = false;
    [Range(-1, 999999)]
    public int correctNumber = -1;

    public float numberChangeRate = 0.1f;
    private float timePassed = 0;

    public Text text;
    private bool displayedCorrectNumber;
    public AudioSource sound;
    public AudioClip done;

    // Use this for initialization
    void Start () {
		if (correctNumber == -1) {
            correctNumber = UnityEngine.Random.Range(0, 999999);
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (displayedCorrectNumber || giveCorrectNumber) {
            DisplayCorrectNumber();
        }
        else {
            if (timePassed > numberChangeRate) {
                DisplayRandomNumber();
                timePassed = 0;
            }
            timePassed += Time.deltaTime;
        }
	}

    private void DisplayCorrectNumber()
    {
        text.text = String.Format("{0:D6}", correctNumber);
        displayedCorrectNumber = true;
    }

    public void ShowNumber ()
    {
        giveCorrectNumber = true;
        sound.PlayOneShot(done);
    }

    private void DisplayRandomNumber()
    {
        //https://docs.microsoft.com/en-us/dotnet/standard/base-types/how-to-pad-a-number-with-leading-zeros
        text.text = String.Format("{0:D6}", UnityEngine.Random.Range(0, 999999));
        sound.Play();
    }
}
