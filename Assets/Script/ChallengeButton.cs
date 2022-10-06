using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeButton : MonoBehaviour
{
    [SerializeField] private Level level;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Image fill;
    [SerializeField] private bool isCompleted;
    [SerializeField] private bool isClicked;
    [SerializeField] private GameObject newChallenge;
    
    public bool IsCompleted => isCompleted;

    private void OnEnable()
    {
        if (Time.frameCount < 3) return;
        isClicked = PlayerPrefs.GetInt("ClickedLevel" + level.level, 0) == 1;
        newChallenge.SetActive(!isClicked);
    }

    public void SetLevel(Level lv)
    {
        level = lv;
        levelText.text = "<color=green>" + level.level;
    }

    public void ButtonPressed()
    {
        AudioManager.Instance.Play("Click");
        if (!isClicked)
        {
            isClicked = true;
            PlayerPrefs.SetInt("ClickedLevel" + level.level, 1);
            newChallenge.SetActive(false);
        }
        ChallengePages.Instance.ShowPanelToStart(level);
    }

    private void Update()
    {
        if (PlayerPrefs.GetInt("Level" + level.level) == 1)
        {
            fill.color = Color.green;
            levelText.text = "<color=white>" + level.level;
            isCompleted = true;
        }
    }
}
