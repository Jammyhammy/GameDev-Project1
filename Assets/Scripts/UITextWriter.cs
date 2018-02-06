using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System;

public class UITextWriter : MonoBehaviour {
    public Text text;
    public bool playOnAwake = true;
    public float delay;
    public float delayBetweenChars = 0.125f;
    public float delayAfterPunctuation = 0.5f;

    private string typetext;
    private float originDelayBetweenChars;
    private bool lastCharPunctuation = false;
    private char charComma;
    private char charPeriod;

    void Awake()
    {
        text = GetComponent<Text>();
        originDelayBetweenChars = delayBetweenChars;

        charComma = Convert.ToChar(44);
        charPeriod = Convert.ToChar(46);

        if (playOnAwake)
        {
            ChangeText(text.text, delay);
        }
     }

    public void ChangeText(string textContent, float delayBetweenChars = 0f)
    {
        StopCoroutine(PlayText());
        typetext = textContent;
        text.text = "";
        Invoke("Start_PlayText", delayBetweenChars);
    }

    void Start_PlayText()
    {
        StartCoroutine(PlayText());
    }

    IEnumerator PlayText()
    {

        foreach (char c in typetext)
        {
            delayBetweenChars = originDelayBetweenChars;

            if (lastCharPunctuation)  //If previous character was a comma/period, pause typing
            {
                yield return new WaitForSeconds(delayBetweenChars = delayAfterPunctuation);
                lastCharPunctuation = false;
            }
         
            if (c == charComma || c == charPeriod)
            {
                lastCharPunctuation = true;
            }

            text.text += c;
            yield return new WaitForSeconds(delayBetweenChars);
        }
    }

}
