    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.UI;

    public class PagesManager : MonoBehaviour
{
    #region Instance

    public static PagesManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion
    
    //Sprite inside the page
    public int IdOfBallSprite;
    public int IdOfWingSprite;
    public int IdOfHoopSprite;
    public int IdOfFlameSprite;
    
    [SerializeField] private GridLayoutGroup hoopsButtonContainer;
    [SerializeField] private GridLayoutGroup wingsButtonContainer;
    [SerializeField] private GridLayoutGroup ballsButtonContainer;
    [SerializeField] private GridLayoutGroup flamesButtonContainer;
    [SerializeField] private GameObject buttonPrefab;

    [SerializeField] private GameObject hintPanel;
    public Skin[] skins;
    public Image[] ImagesInPlayButton;
    
    
    private void Start()
    {
        IdOfBallSprite = PlayerPrefs.GetInt("BallID", 100);
        IdOfWingSprite = PlayerPrefs.GetInt("WingID", 200);
        IdOfHoopSprite = PlayerPrefs.GetInt("HoopID", 300);
        IdOfFlameSprite = PlayerPrefs.GetInt("FlameID", 400);

        hoopsButtonContainer.cellSize = Vector2.one * CalculateCellSize();
        wingsButtonContainer.cellSize = Vector2.one * CalculateCellSize();
        ballsButtonContainer.cellSize = Vector2.one * CalculateCellSize();
        flamesButtonContainer.cellSize = Vector2.one * CalculateCellSize();
        
        foreach (var skin in skins)
        {
            GameObject g;
            switch (skin.tag)
            {
                case "Ball":
                    g = Instantiate(buttonPrefab, ballsButtonContainer.transform, true);
                    g.transform.localScale = Vector3.one;
                    g.GetComponent<SkinButton>().SetSkin(skin);
                    break;
                case "Wing":
                    g = Instantiate(buttonPrefab, wingsButtonContainer.transform, true);
                    g.transform.localScale = Vector3.one;
                    g.GetComponent<SkinButton>().SetSkin(skin);
                    break;
                case "Flame":
                    g = Instantiate(buttonPrefab, flamesButtonContainer.transform, true);
                    g.transform.localScale = Vector3.one;
                    g.GetComponent<SkinButton>().SetSkin(skin);
                    break;
                case "Hoop":
                    g = Instantiate(buttonPrefab, hoopsButtonContainer.transform, true);
                    g.transform.localScale = Vector3.one;
                    g.GetComponent<SkinButton>().SetSkin(skin);
                    break;
            }
        }
        OnClickOkButton();
        transform.parent.gameObject.SetActive(false);
    }

    public void OnClickTabButton(GameObject tabButton)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        tabButton.SetActive(true);
    }
    
    

    public void OnClickOkButton()
    {
        foreach (var s in skins)
        {
            if (s.tag == "Ball" && s.id == IdOfBallSprite)
            {
                PlayerPrefs.SetInt("BallID", s.id);
            }
            
            if (s.tag == "Wing" && s.id == IdOfWingSprite)
            {
                PlayerPrefs.SetInt("WingID", s.id);
            }
            
            if (s.tag == "Hoop" && s.id == IdOfHoopSprite)
            {
                PlayerPrefs.SetInt("HoopID", s.id);
            }
            
            if (s.tag == "Flame" && s.id == IdOfFlameSprite)
            {
                PlayerPrefs.SetInt("FlameID", s.id);
            }
        }

        foreach (var image in ImagesInPlayButton)
        {
            if (image.gameObject.name.Contains("Body"))
            {
                image.sprite = FindSpriteById(PlayerPrefs.GetInt("BallID", 100));
            }

            if (image.gameObject.name.Contains("Wing"))
            {
                image.sprite = FindSpriteById(PlayerPrefs.GetInt("WingID", 200));
            }
        }

    }
    
    public void ShowHintPanel(Skin skin)
    {
        hintPanel.SetActive(true);
        hintPanel.GetComponent<HintPanel>().SetPropertyForHintPanel(skin);
    }
    

    public void SetSpriteById()
    {
        foreach (var s in skins)
        {
            if (s.id == PlayerPrefs.GetInt("BallID") && s.tag == "Ball")
            {
                GameManager.Instance.BallSprite = s.sprite;
            }

            if (s.id == PlayerPrefs.GetInt("WingID") && s.tag == "Wing")
            {
                GameManager.Instance.WingSprite = s.sprite;
            }

            if (s.id == PlayerPrefs.GetInt("HoopID") && s.tag == "Hoop")
            {
                GameManager.Instance.HoopSprite[0] = s.sprite;
                GameManager.Instance.HoopSprite[1] = s.secondSprite;
            }

            if (s.id == PlayerPrefs.GetInt("FlameID") && s.tag == "Flame")
            {
                GameManager.Instance.ColorOfFlame = s.colorOfFlame;
            }
        }
    }

    public void TrySkin(int id)
    {
        foreach (var s in skins)
        {
            if (s.id != id) continue;
            switch (s.tag)
            {
                case "Ball":
                    GameManager.Instance.BallSprite = s.sprite;
                    break;
                case "Wing":
                    GameManager.Instance.WingSprite = s.sprite;
                    break;
                case "Flame":
                    GameManager.Instance.ColorOfFlame = s.colorOfFlame;
                    break;
                case "Hoop":
                    GameManager.Instance.HoopSprite[0] = s.sprite;
                    GameManager.Instance.HoopSprite[1] = s.secondSprite;
                    break;
            }

            return;
        }
    }
    
    public Sprite FindSpriteById(int id)
    {
        foreach (var s in skins)
        {
            if (s.id == id)
            {
                return s.sprite;
            }
        }

        return null;
    }
    
    private float CalculateCellSize()
    {
        return (transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().rect.width - 40
               - hoopsButtonContainer.spacing.x * (hoopsButtonContainer.constraintCount - 1)) /
               hoopsButtonContainer.constraintCount;
    }

    public Skin FindSkinById(int id)
    {
        return skins.FirstOrDefault(s => s.id == id);
    }
}
