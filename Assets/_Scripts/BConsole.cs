using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class BConsole : MonoBehaviour {

    public BButton One;
    public BButton Two;
    public BButton Three;
    public BButton Four;
    public BButton Five;
    public BButton Six;
    public BButton Seven;
    public BButton Eight;
    public BButton Nine;
    public BButton Zero;
    public BButton Clr;

    static string failString = "XXXXXX";
    static string defaultString = "------";
    string displayedString = defaultString;

    public Text uiText;
    int place = 0;
    public int numDigits = 6;

    float timeCountdown = 0;
    public float waitTime = 0.5f;

    public string winString = "528491";
    private bool match = false;

    public event Action StringMatched;
    public AudioSource consoleWithKeyPressSound;
    public AudioClip correctSound;

    // Use this for initialization
    void Start () {
        One.SingleFireButtonAction += OnePressed;
        Two.SingleFireButtonAction += TwoPressed;
        Three.SingleFireButtonAction += ThreePressed;
        Four.SingleFireButtonAction += FourPressed;
        Five.SingleFireButtonAction += FivePressed;
        Six.SingleFireButtonAction += SixPressed;
        Seven.SingleFireButtonAction += SevenPressed;
        Eight.SingleFireButtonAction += EightPressed;
        Nine.SingleFireButtonAction += NinePressed;
        Zero.SingleFireButtonAction += ZeroPressed;
        Clr.SingleFireButtonAction += ClrPressed;
	}

    // Update is called once per frame
    void Update()
    {
        if (match) {
            return;
        }
        if (displayedString.Equals(winString)) {
            uiText.text = displayedString;
            //WIN!
            if (StringMatched != null)
                StringMatched();
            match = true;
            consoleWithKeyPressSound.PlayOneShot(correctSound);
            Color tempColor;
            ColorUtility.TryParseHtmlString("#00A2E8FF", out tempColor);
            uiText.color = tempColor;
            One.SingleFireButtonAction -= OnePressed;
            Two.SingleFireButtonAction -= TwoPressed;
            Three.SingleFireButtonAction -= ThreePressed;
            Four.SingleFireButtonAction -= FourPressed;
            Five.SingleFireButtonAction -= FivePressed;
            Six.SingleFireButtonAction -= SixPressed;
            Seven.SingleFireButtonAction -= SevenPressed;
            Eight.SingleFireButtonAction -= EightPressed;
            Nine.SingleFireButtonAction -= NinePressed;
            Zero.SingleFireButtonAction -= ZeroPressed;
            Clr.SingleFireButtonAction -= ClrPressed;
        }
        if (timeCountdown <= 0) {
            if (place < numDigits) {
                uiText.text = displayedString;
            }
            else {
                place = 0;
                timeCountdown = waitTime;
            }
        }
        else {
            timeCountdown -= Time.deltaTime;
            displayedString = defaultString;
            uiText.text = failString;
        }
    }

    private void ClrPressed()
    {
        displayedString = defaultString;
        place = 0;
    }

    private void changeDisplayedString (char num)
    {
        if (timeCountdown <= 0) {
            displayedString = displayedString.ReplaceAt(place, num);
            place++;
            consoleWithKeyPressSound.Play();
        }
    }

    private void ZeroPressed()
    {
        changeDisplayedString('0');
    }

    private void NinePressed()
    {
        changeDisplayedString('9');
    }

    private void EightPressed()
    {
        changeDisplayedString('8');
    }

    private void SevenPressed()
    {
        changeDisplayedString('7');
    }

    private void SixPressed()
    {
        changeDisplayedString('6');
    }

    private void FivePressed()
    {
        changeDisplayedString('5');
    }

    private void FourPressed()
    {
        changeDisplayedString('4');
    }

    private void ThreePressed()
    {
        changeDisplayedString('3');
    }

    private void TwoPressed()
    {
        changeDisplayedString('2');
    }
    void OnePressed()
    {
        changeDisplayedString('1');
    }
}
