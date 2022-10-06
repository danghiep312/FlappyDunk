using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowScore : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    private void Update()
    {
        scoreText.text = ScoreManager.Instance.CurrentScore.ToString();
        if (GameManager.Instance.PlayChallenge)
        {
            scoreText.text = "<alpha=#00>" + ScoreManager.Instance.CurrentScore;
        }
    }
}
