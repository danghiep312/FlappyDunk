using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HighlightText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] buttonText;
    [SerializeField] private string[] originalText;
    private void Start()
    {
        originalText = new string[buttonText.Length];
        for (int i = 0; i < buttonText.Length; i++)
        {
            originalText[i] = buttonText[i].text;
        }

        buttonText[0].text = "<color=#1180FB><line-height=80%>" + originalText[0] + "\n<u><alpha=#00>" + originalText[0];
    }

    public void Click(TextMeshProUGUI button)
    {
        string currentText = "";
        for (int i = 0; i < buttonText.Length; i++)
        {
            buttonText[i].text = originalText[i];
            if (button.text == buttonText[i].text)
            {
                currentText = buttonText[i].text;
            }
        }

        button.text = "<color=#1180FB><line-height=70%>" + currentText + "\n<u><alpha=#00>" + currentText;
        // button.text = <color=#1180FB><cspace=-0.01em>BALLS</cspace>
        //     <u><alpha=#00><cspace=-0.01em>BALLS</cspace>
    }
    

}
