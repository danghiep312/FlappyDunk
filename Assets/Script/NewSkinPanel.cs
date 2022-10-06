using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewSkinPanel : MonoBehaviour
{
    [SerializeField] Image mainImage;
    [SerializeField] Image secondaryImage;
    [SerializeField] private TextMeshProUGUI descriptionText;
    
    public void SetPropertyForPanel(Skin skin)
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
                mainImage.color = new Color(c.r, c.g, c.b, 1);
            }
            else
            {
                mainImage.color = Color.white;
            }
        }
        else
        {
            mainImage.color = secondaryImage.color = Color.white;
            mainImage.sprite = skin.sprite;
            secondaryImage.sprite = skin.secondSprite;
            secondaryImage.gameObject.SetActive(true);
            rt.localScale = rt2.localScale = Vector3.one * 1.4f;
            rt.sizeDelta = new Vector2(200, 34);
            rt2.sizeDelta = new Vector2(160.5f, 52);
            rt.localPosition = Vector3.up * 13f;
            rt2.localPosition = Vector3.up * -30f;
        }
    }
}
