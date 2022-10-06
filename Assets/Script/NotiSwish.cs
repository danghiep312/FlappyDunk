using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class NotiSwish : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private TextMeshProUGUI noti;
    [SerializeField] private Gradient grad;
    private Color startColor;
    private void OnEnable()
    {
        rectTransform.localPosition = Vector3.up * 250f;
        rectTransform.DOAnchorPos(Vector2.up * 450f, 0.5f).SetEase(Ease.OutCirc).OnComplete(() =>
        {
            noti.DOFade(0, 0.5f).SetEase(Ease.OutCirc).OnComplete(HideNotification);
        });
    }

    public void SetText(int streak)
    {
        noti.text = "SWISH!\nX" + streak;
        noti.color = new Color(0, 0, 0, 255);
        noti.color = grad.Evaluate(streak / 5f);
    }

    public void HideNotification()
    {
        gameObject.SetActive(false);
    }
}
