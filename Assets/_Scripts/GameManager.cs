using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public LCDDisplay lcd;
    public int correctNumber = -1;
    public BConsole console;

    public Animator animator;
    public AudioSource audioSource;

    public Light spotlight;

    // Use this for initialization
    void Start () {
        if (correctNumber == -1) {
            correctNumber = UnityEngine.Random.Range(0, 999999);
        }
        lcd.SetCorrectNumber(correctNumber);
        console.winString = correctNumber.ToString();
        console.StringMatched += Win;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void Win()
    {
        animator.SetBool("Kill", true);
        audioSource.Stop();
        spotlight.enabled = true;
    }
}
