using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SkinButton : MonoBehaviour
{
     [SerializeField] private Skin skin;
     [SerializeField] private GameObject selected;
     [SerializeField] private Image image;
     [SerializeField] private Image image2;
     [SerializeField] private GameObject blur;
     [SerializeField] private GameObject newSkin;
     [SerializeField] private bool isUnlocked;
     [SerializeField] private bool isClicked;

     private void OnEnable()
     {
          if (Time.frameCount < 3) return;   
          isUnlocked = PlayerPrefs.GetInt(skin.id.ToString(), 0) == 1 || skin.id % 100 == 0;
          isClicked = PlayerPrefs.GetInt(skin.id + "Clicked", 0) == 1 || skin.id % 100 == 0;
          blur.SetActive(!isUnlocked);
          newSkin.SetActive(!isClicked);
          if (!isUnlocked) newSkin.SetActive(false);
          
     }
     

     private void Update()
     {
          if (transform.position.z < 0)
          {
               transform.DOLocalMoveZ(0, 0.01f);
          }
          selected.SetActive(skin.id == PagesManager.Instance.IdOfBallSprite 
                             || skin.id == PagesManager.Instance.IdOfWingSprite
                             || skin.id == PagesManager.Instance.IdOfHoopSprite 
                             || skin.id == PagesManager.Instance.IdOfFlameSprite);
     }

     public void ButtonPressed()
     {
          if (!selected.activeSelf)
          {
               AudioManager.Instance.Play("Click");
          }
          if (!isClicked && isUnlocked)
          {
               isClicked = true;
               newSkin.SetActive(false);
               PlayerPrefs.SetInt(skin.id + "Clicked", 1);
          }
          
          if (!isUnlocked)
          {
               PagesManager.Instance.ShowHintPanel(skin);
               GameManager.Instance.TryId = skin.id;
               return;
          }
          
          switch (skin.tag)
          {
               case "Ball":
                    PagesManager.Instance.IdOfBallSprite = skin.id;
                    break;
               case "Hoop":
                    PagesManager.Instance.IdOfHoopSprite = skin.id;
                    break;
               case "Wing":
                    PagesManager.Instance.IdOfWingSprite = skin.id;
                    break;
               case "Flame":
                    PagesManager.Instance.IdOfFlameSprite = skin.id;
                    break;
          }
     }

     public void SetSkin(Skin s)
     {
          skin = s;
          image.sprite = skin.sprite;
          if (s.tag == "Flame")
          {
               image.GetComponent<RectTransform>().sizeDelta = Vector2.one * 200f;
               if (s.id % 100 != 0)
               {
                    var tmp = skin.colorOfFlame;
                    image.color = new Color(tmp.r, tmp.g, tmp.b, 1);
               }
          }
          if (s.tag != "Hoop") return;
          var rt = image.GetComponent<RectTransform>();
          var rt2 = image2.GetComponent<RectTransform>();    
          image2.gameObject.SetActive(true);
          image2.sprite = skin.secondSprite;
          rt.localScale = rt2.localScale = Vector3.one * 0.8f;
          rt.sizeDelta = new Vector2(200, 34);
          rt2.sizeDelta = new Vector2(160, 52);
          rt.localPosition = Vector3.up * 8.4f;
          rt2.localPosition = Vector3.up * -16f;
          
     }
     
}
