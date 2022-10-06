using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProcessFill : MonoBehaviour
{
    [SerializeField] private Gradient grad;
    [SerializeField] private Image fill;
    public int Value;
    public int Total;
    public TextMeshProUGUI progress;

    private void Update()
    {
        if (gameObject.name.Contains("Skin"))
        {
            Value = GameManager.Instance.Achievement.TotalSkin;
            Total = PagesManager.Instance.skins.Length;
        }
        else
        {
            Value = GameManager.Instance.Achievement.TotalChallengeCompleted;
            Total = GameManager.Instance.LevelsHolder.Length;
        }
        fill.fillAmount = (float) Value / Total;     
        fill.color = grad.Evaluate(fill.fillAmount);
        if (progress == null) return;
        progress.text = Value + " / " + Total;
    }
}
