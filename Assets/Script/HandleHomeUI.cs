using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class HandleHomeUI : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private TextMeshProUGUI bestScore;
    [SerializeField] private TextMeshProUGUI lastScore;

    private void OnEnable()
    {
        bestScore.text = "<cspace=-0.05em>BEST: <color=#047604>" +  PlayerPrefs.GetInt("BestScore", 0) + "</color>";
        lastScore.text = "LAST: " + PlayerPrefs.GetInt("LastScore", 0);
        // if (Time.frameCount < 60) // when play game the first
        // {
        //     return;
        // }
        //rectTransform.localPosition = Vector3.left * 2000f; 
        //rectTransform.DOAnchorPos(Vector2.zero, 0.5f);
    }
}
