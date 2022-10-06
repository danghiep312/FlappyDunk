using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChallengePages : MonoBehaviour
{
    #region Instance

    public static ChallengePages Instance;

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

    [SerializeField] private GridLayoutGroup normalButtonContainer;
    [SerializeField] private GridLayoutGroup mirrorButtonContainer;

    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private Level[] levels;

    [SerializeField] private GameObject panelToStart;
    private void Start()
    {
        foreach (var level in levels)
        {
            var button = Instantiate(buttonPrefab, normalButtonContainer.transform);
            button.GetComponent<ChallengeButton>().SetLevel(level);
        }
        transform.parent.gameObject.SetActive(false);
        
        normalButtonContainer.cellSize = Vector2.one * CalculateCellSize();
        mirrorButtonContainer.cellSize = Vector2.one * CalculateCellSize();
    }

    public void OnClickTabButton(GameObject tabButton)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        tabButton.SetActive(true);
    }
    
    public void ShowPanelToStart(Level l)
    {
        panelToStart.SetActive(true);
        panelToStart.GetComponent<PanelToStart>().SetLevel(l);      
    }

    private float CalculateCellSize()
    {
        return (transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().rect.width -
                normalButtonContainer.spacing.x * (normalButtonContainer.constraintCount - 1) - 40) /
               normalButtonContainer.constraintCount;
    }
}
