using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverProcess : MonoBehaviour
{
    [SerializeField] private GameObject buttonInGameOver;
    private WaitForSecondsRealtime waitForSecondsRealtime = new WaitForSecondsRealtime(2f);
    private void OnEnable()
    {
        buttonInGameOver.SetActive(false);
        StartCoroutine(ActiveButton());
    }
    
    IEnumerator ActiveButton()
    {
        yield return waitForSecondsRealtime;
        if (GameManager.Instance.PlayTime == 1)
        {
            buttonInGameOver.SetActive(true);
        }
        else
        {
            GameManager.Instance.GoHome();
        }
    }
}
