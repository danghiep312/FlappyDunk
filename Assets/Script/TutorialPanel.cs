using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPanel : MonoBehaviour
{
    public GameObject DTCeiling;
    public GameObject DTFloor;
    public GameObject TapToJump;
    public GameObject ArrowInDTCeiling;
    public GameObject ArrowInDTFLoor;
    public GameObject GreenArrow;
    public GameObject WhiteArrow;
    public GameObject CrossInWhiteArrow;
    private void OnEnable()
    {
        DTCeiling.GetComponent<RectTransform>().DOAnchorPosY(600f, 0.5f).SetUpdate(true).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            DTFloor.GetComponent<RectTransform>().DOAnchorPosY(-570f, 0.5f).SetUpdate(true).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                TapToJump.GetComponent<TextMeshProUGUI>().DOFade(1, 0.5f).SetUpdate(true).OnComplete(() =>
                {
                    GameManager.Instance.CanTap = true;
                });
            });
            DTFloor.GetComponent<TextMeshProUGUI>().DOFade(1, 0.5f).SetUpdate(true);
            ArrowInDTFLoor.GetComponent<Image>().DOFade(1, 0.5f).SetUpdate(true);
        });
        DTCeiling.GetComponent<TextMeshProUGUI>().DOFade(1, 0.5f).SetUpdate(true);
        ArrowInDTCeiling.GetComponent<Image>().DOFade(1, 0.5f).SetUpdate(true);

        GreenArrow.GetComponent<Image>().DOFade(1, 0.5f).SetUpdate(true).SetDelay(1f);
        WhiteArrow.GetComponent<Image>().DOFade(1, 0.5f).SetUpdate(true).SetDelay(1f);
        CrossInWhiteArrow.GetComponent<Image>().DOFade(1, 0.5f).SetDelay(1f).SetUpdate(true);
    }

    private void OnDisable()
    {
        DTCeiling.GetComponent<RectTransform>().anchoredPosition = new Vector2(-65, 0);
        DTCeiling.GetComponent<TextMeshProUGUI>().color = Color.clear;
        ArrowInDTCeiling.GetComponent<Image>().color = new Color(1, 0, 0, 0);
        DTFloor.GetComponent<RectTransform>().anchoredPosition = new Vector2(-58, 0);
        DTFloor.GetComponent<TextMeshProUGUI>().color = Color.clear;
        ArrowInDTFLoor.GetComponent<Image>().color = new Color(1, 0, 0, 0);
        TapToJump.GetComponent<TextMeshProUGUI>().color = Color.clear;
        GreenArrow.GetComponent<Image>().color = new Color(0, 1, 0, 0);
        WhiteArrow.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        CrossInWhiteArrow.GetComponent<Image>().color = new Color(1, 0, 0, 0);
    }
}
