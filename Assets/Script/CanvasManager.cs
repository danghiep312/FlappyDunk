using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CanvasManager : MonoBehaviour
{
    #region Instance

    public static CanvasManager Instance;
    
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
    
    [SerializeField] private RectTransform homePanel;
    [SerializeField] private RectTransform gameOverPanel;    
    [SerializeField] private RectTransform hudPanel;
    [SerializeField] private RectTransform pausePanel;
    [SerializeField] private RectTransform challengePanel;
    [SerializeField] private RectTransform panelToStartChallenge;
    [SerializeField] private RectTransform shopPanel;
    [SerializeField] private RectTransform tutorialPanel;
    [SerializeField] float vertical, horizontal;

    private void Start()
    {
        var sizeDelta = gameOverPanel.sizeDelta;
        vertical = sizeDelta.y;
        horizontal = sizeDelta.x;
    }

    private void Update()
    {
        if (tutorialPanel.gameObject.activeSelf)
        {
            if (Time.timeScale != 0)
            {
                tutorialPanel.gameObject.SetActive(false);
                hudPanel.gameObject.SetActive(true);
            }
        }
    }

    public void PauseGame()
    {
        hudPanel.gameObject.SetActive(false);
        pausePanel.gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    public void Resume()
    {
        pausePanel.gameObject.SetActive(false);
        hudPanel.gameObject.SetActive(true);
        Time.timeScale = 1;
    }

    public void GameOver()
    {
        hudPanel.gameObject.SetActive(false);
        gameOverPanel.gameObject.SetActive(true);
    }
    
    public void GoHome()
    {
        homePanel.gameObject.SetActive(true);
        homePanel.DOAnchorPos(Vector2.up * vertical, 0.01f).SetUpdate(true);
        if (challengePanel.gameObject.activeSelf)
        {
            challengePanel.DOAnchorPos(Vector2.zero, 0.5f).SetUpdate(true);
            homePanel.DOAnchorPos(Vector2.left * horizontal, 0.01f).SetUpdate(true);
        }
        else
        {
            homePanel.DOAnchorPos(Vector2.zero, 0.5f).SetUpdate(true);
        }
        gameOverPanel.gameObject.SetActive(false);
    }

    public void Hud(bool status)
    {
        hudPanel.gameObject.SetActive(status);
    }

    public void PlayGame()
    {
        hudPanel.gameObject.SetActive(true);
        
        gameOverPanel.gameObject.SetActive(false);
        homePanel.DOAnchorPos(Vector2.right * horizontal, 0.3f).SetUpdate(true).OnComplete(() =>
        {
            homePanel.gameObject.SetActive(false);
            if (PlayerPrefs.GetInt("BestScore") == 0)
            {
                tutorialPanel.gameObject.SetActive(true);   
                hudPanel.gameObject.SetActive(false);
            }
        });
    }

    public bool CheckIsPausing()
    {
        return pausePanel.gameObject.activeSelf;
    }

    public void PlayChallengeLevel()
    {
        challengePanel.DOAnchorPos(Vector2.up * vertical, 0.3f).SetUpdate(true);
        hudPanel.gameObject.SetActive(true);
        gameOverPanel.gameObject.SetActive(false);
    }
    
    public void ShowChallengePanel()
    {
        challengePanel.gameObject.SetActive(true);
        if (GameManager.Instance.CompletedChallenge)    
        {
            panelToStartChallenge.gameObject.SetActive(false);
            panelToStartChallenge.GetComponent<PanelToStart>().Retry("Start");
            GameManager.Instance.CompletedChallenge = false;
            
        }
        // challengePanel.GetComponent<RectTransform>().localPosition = Vector3.left * 2000f; 
        // challengePanel.GetComponent<RectTransform>().DOAnchorPos(Vector2.zero, 0.5f);
        hudPanel.gameObject.SetActive(false);
        homePanel.gameObject.SetActive(false);
    }
    
    public void GoToShop()
    {
        shopPanel.gameObject.SetActive(true);
        shopPanel.DOAnchorPos(Vector2.right * horizontal, 0.01f).OnComplete(() =>
        {
            shopPanel.DOAnchorPos(Vector3.zero, 0.3f);
        });
        homePanel.DOAnchorPos(Vector2.left * horizontal, 0.31f, true).OnComplete(() =>
        {
            homePanel.gameObject.SetActive(false);
        });
    }
    
    public void GoToChallenge()
    {
        challengePanel.DOAnchorPos(Vector2.right * horizontal, 0.01f).OnComplete(() =>
        {
            challengePanel.DOAnchorPos(Vector3.zero, 0.3f);
        });
        challengePanel.gameObject.SetActive(true);
        homePanel.DOAnchorPos(Vector2.left * horizontal, 0.31f, true).OnComplete(() =>
        {
            homePanel.gameObject.SetActive(false);
        });
    }

    public void ClickOk()
    {
        homePanel.gameObject.SetActive(true);
        homePanel.DOAnchorPos(Vector2.zero, 0.3f);
        if (shopPanel.gameObject.activeSelf)
        {
            shopPanel.DOAnchorPos(Vector2.right * horizontal, 0.3f, true).OnComplete(() =>
            {
                shopPanel.gameObject.SetActive(false);
            });
        }

        if (challengePanel.gameObject.activeSelf)
        {
            challengePanel.DOAnchorPos(Vector2.right * horizontal, 0.3f, true).OnComplete(() =>
            {
                challengePanel.gameObject.SetActive(false);
            });
        }
    }
}
