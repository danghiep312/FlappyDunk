using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

public class NewBestScoreNotification : MonoBehaviour
{
    [SerializeField] private RectTransform rt;
    [SerializeField] private RectTransform cur;
    private void OnEnable()
    {
        DOTween.Kill(transform);
        rt.localScale = Vector3.zero;
        cur.localScale = Vector3.zero;
        rt.DOScale(1, 0.5f).OnComplete(Shake);
        cur.DOScale(1, 0.5f);
    }

    private void Shake()
    {
        rt.DORotate(Vector3.forward * 6f, 1f).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            rt.DORotate(Vector3.forward * -6f, 1f).SetEase(Ease.InOutSine);
        }).SetLoops(3, LoopType.Yoyo).OnComplete(() =>
        {
            rt.DORotate(Vector3.forward * -6f, 1f).SetEase(Ease.InOutSine).OnComplete(() =>
            {
                cur.DOScale(0, 0.3f);
                rt.DOScale(0, 0.3f).OnComplete(() =>
                {
                    gameObject.SetActive(false);
                });
            });
        });
    }
}
