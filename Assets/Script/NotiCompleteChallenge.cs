using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class NotiCompleteChallenge : MonoBehaviour
{
    [SerializeField] private RectTransform rt;
    [SerializeField] private TextMeshProUGUI noti;

    private void OnEnable()
    {
        rt.localPosition = Vector3.zero;
        rt.DOLocalMove(Vector3.up * 400f, 0.5f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            noti.DOFade(0, 0.2f).SetDelay(0.5f).OnComplete(OffNoti);
        });
        noti.DOFade(0, 0.01f).OnComplete(() =>
        {
            noti.DOFade(1, 0.3f);
        });
        
    }

    void OffNoti()
    {
        gameObject.SetActive(false);
    }
}
