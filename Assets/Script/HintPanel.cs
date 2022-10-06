using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HintPanel : MonoBehaviour
{
    [SerializeField] Image mainImage;
    [SerializeField] Image secondaryImage;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI progress;
    
    public void SetPropertyForHintPanel(Skin skin)
    {
        var rt = mainImage.GetComponent<RectTransform>();
        var rt2 = secondaryImage.GetComponent<RectTransform>();    
        descriptionText.text = skin.description;
        if (skin.tag != "Hoop")
        {
            mainImage.sprite = skin.sprite;
            secondaryImage.gameObject.SetActive(false);
            mainImage.GetComponent<RectTransform>().sizeDelta = new Vector2(200f, 200f);
            rt.localScale = Vector3.one;
            if (skin.tag == "Flame")
            {
                var c = skin.colorOfFlame;
                mainImage.color = new Color(c.r, c.g, c.b, 1f);
            }
            else
            {
                mainImage.color = Color.white;
            }
        }
        else
        {
            mainImage.sprite = skin.sprite;
            mainImage.color = secondaryImage.color = Color.white;
            secondaryImage.sprite = skin.secondSprite;
            secondaryImage.gameObject.SetActive(true);
            secondaryImage.sprite = skin.secondSprite;
            rt.localScale = rt2.localScale = Vector3.one * 1.4f;
            rt.sizeDelta = new Vector2(200, 34);
            rt2.sizeDelta = new Vector2(160.5f, 52);
            rt.localPosition = Vector3.up * 13f;
            rt2.localPosition = Vector3.up * -30f;
        }
    }
}
