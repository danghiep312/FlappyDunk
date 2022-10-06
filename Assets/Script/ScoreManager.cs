using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{
    public int CurrentScore;
    private void Start()
    {
        CurrentScore = 0;
    }

    public void AddScore(int n)
    {
        if (GameManager.Instance.PlayChallenge) return;
        this.PostEvent(EventID.OnAddScore, n);
        CurrentScore += n;
    }

    public void ResetScore()
    {
        CurrentScore = 0;
    }
}
