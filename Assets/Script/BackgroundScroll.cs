using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;

public class BackgroundScroll : MonoBehaviour
{
    [SerializeField] private Transform[] bg;
    [SerializeField] private Transform cam;
    [SerializeField] private Transform makeColor;
    [SerializeField] private float distance = 10.25f;
    [SerializeField] private GameObject[] offsetBg;
    [SerializeField] private SpriteRenderer[] offsetBgSprite;
    [SerializeField] private Animator[] offsetBgAnimtor;
    [SerializeField] private SpriteRenderer mainBg;

    private void Start()
    {
        offsetBgSprite = new SpriteRenderer[offsetBg.Length];
        offsetBgAnimtor = new Animator[offsetBg.Length];
        for (int i = 0; i < offsetBg.Length; i++)
        {
            offsetBgSprite[i] = offsetBg[i].GetComponent<SpriteRenderer>();
            offsetBgAnimtor[i] = offsetBg[i].GetComponent<Animator>();
        }
    }
    
    private void Update()
    {
        if (!GameManager.Instance.IsPlaying)
        {
            return;
        } 
        
        if (CheckCamInside(bg[0]))
        {
            bg[1].position = bg[0].position + Vector3.right * distance;
        }
        
        if (CheckCamInside(bg[1]))
        {
            bg[0].position = bg[1].position + Vector3.right * distance;
        }

        makeColor.position = cam.position + Vector3.forward * distance;
        
    }
    

    public bool CheckCamInside(Transform bg)
    {
        return cam.position.x - GameManager.Instance.HorizontalScreen / 2 > bg.position.x - 5f 
               && cam.position.x + GameManager.Instance.HorizontalScreen / 2 < bg.position.x + 5f;
    }

    public void ResetPositionByGameObject(GameObject g)
    {
        for (int i = 0; i < 2; i++)
        {
            bg[i].position = g.transform.position + Vector3.right * distance * i + Vector3.forward * distance;
        }
    }

    public void ResetPosition()
    {
        for (int i = 0; i < 2; i++)
        {
            bg[i].position = new Vector3(i * distance, 0, 10);
        }
    }

    public void FadeInHome()
    {
        for (int i = 0; i < offsetBg.Length; i++)
        {
            if (offsetBgSprite[i].gameObject.name.Contains("MakeColor"))
            {
                offsetBgSprite[i].DOFade(130/255f, 1f);
                continue;
            }
            offsetBgSprite[i].DOFade(1, 1f).OnComplete(() =>
            {
                GameManager.Instance.CanTap = true;
            });
        }
    }

    public void FadeOutWhenPlaying()
    {
        if (PlayerPrefs.GetInt("BestScore") == 0)
        {
            foreach (var sr in offsetBgSprite)
            {
                sr.DOFade(0, 0.01f).SetUpdate(true);
            }
            return;
        }
        
        for (int i = 0; i < offsetBg.Length; i++)
        {
            offsetBgAnimtor[i].enabled = true;
            offsetBgAnimtor[i].Play(offsetBgAnimtor[i].gameObject.name.Contains("MakeColor")
                ? "FadeOut"
                : "BackgroundFadeOut", -1, 0);
        }

        mainBg.DOFade(0, 0.01f).OnComplete(() =>
        {
            mainBg.DOFade(0.5f, 0.3f).SetUpdate(true).SetDelay(0.28f);
        }).SetUpdate(true);
        
        StartCoroutine(AfterFade(1.1f));
    }

    IEnumerator AfterFade(float seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);
        for (int i = 0; i < offsetBgAnimtor.Length; i++)
        {
            offsetBgAnimtor[i].enabled = false;
        }
    }
}
