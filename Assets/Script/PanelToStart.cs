using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PanelToStart : MonoBehaviour
{
    [SerializeField] private Level currentLevel;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private TextMeshProUGUI startOrRetry;

    

    public void SetLevel(Level l)
    {
        currentLevel = l;
        title.text = "CHALLENGE " + l.level;
        description.text = l.description;
    }

    public void PressPlay()
    {
        GameManager.Instance.PlayGameChallenge(currentLevel.levelPrefab);
    }
    
    public void Retry(string text)
    {
        startOrRetry.SetText(text);
    }

}
