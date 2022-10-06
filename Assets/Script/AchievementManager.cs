using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    public int HoopPassedInEndlessMode;
    public int HoopPassedInSinglePlay;
    public int TotalScore;
    public int TotalSwish;
    public int TotalEndless;
    public int TotalSecondChance;
    public int SwishInSinglePlay;
    public int ScoreInSinglePlay;
    public int TotalSkin;
    public int TotalChallengeCompleted;
    public int ReachSwishx4;
    public int EndGame42;
    public Skin[] skins;
    public Queue<Skin> SkinsUnlock = new Queue<Skin>();
    public GameObject NewSkinPanel;
    private NewSkinPanel newSkin;
    private void Start()
    {
        newSkin = NewSkinPanel.GetComponent<NewSkinPanel>();
        TotalSkin = PlayerPrefs.GetInt("TotalSkin", 4);
        TotalScore = PlayerPrefs.GetInt("TotalScore", 0);
        TotalSwish = PlayerPrefs.GetInt("TotalSwish", 0);
        TotalEndless = PlayerPrefs.GetInt("TotalEndless", 0);
        TotalSecondChance = PlayerPrefs.GetInt("TotalSecondChance", 0);
        HoopPassedInEndlessMode = PlayerPrefs.GetInt("HoopPassedInEndlessMode", 0);
        TotalChallengeCompleted = PlayerPrefs.GetInt("TotalChallengeCompleted", 0);
        ReachSwishx4 = PlayerPrefs.GetInt("ReachSwishx4", 0);
        EndGame42 = PlayerPrefs.GetInt("EndGame42", 0);
        SwishInSinglePlay = 0;
        ScoreInSinglePlay = 0;
        HoopPassedInSinglePlay = 0;
        
        this.RegisterListener(EventID.OnReachSwish, (param) => OnReachSwish());
        this.RegisterListener(EventID.OnAddScore, (param) => OnAddScore((int)param));
        this.RegisterListener(EventID.OnUseSecondChance, (param) => OnUseSecondChance());
        this.RegisterListener(EventID.OnPlayEndless, (param) => OnPlayEndless());
        this.RegisterListener(EventID.OnSkinUnlock, (param) => OnSkinUnlock());
        this.RegisterListener(EventID.OnHoopPassed, (param) => OnHoopPassed());
        this.RegisterListener(EventID.OnCompleteLevel, (param) => OnCompleteLevel());
        this.RegisterListener(EventID.ReachSwishx4, (param) => OnReachSwishx4());
        this.RegisterListener(EventID.EndGame42, (param) => OnEndGame42());
    }

    private void OnEndGame42()
    {
        EndGame42++;
    }

    private void OnReachSwishx4()
    {
        ReachSwishx4++;
    }

    private void OnUseSecondChance()
    {
        TotalSecondChance++;
    }

    private void OnAddScore(int score)
    {
        TotalScore += score;
        ScoreInSinglePlay += score;
    }

    private void OnReachSwish()
    {
        SwishInSinglePlay++;
        TotalSwish++;
    }

    private void OnPlayEndless()
    {
        SwishInSinglePlay = 0;
        ScoreInSinglePlay = 0;
        HoopPassedInSinglePlay = 0;
        TotalEndless++;
    }

    private void OnHoopPassed()
    {
        HoopPassedInEndlessMode++;
        HoopPassedInSinglePlay++;
    }

    private void OnSkinUnlock()
    {
        TotalSkin++;
    }
    
    public bool CheckCondition(int param, string condition)
    {
        var numInCondition = int.Parse(condition.Substring(1));
        return condition[0] switch
        {
            '>' => param > numInCondition,
            '<' => param < numInCondition,
            '=' => param == numInCondition,
            _ => false
        };
    }

    public void ShowNewSkin()
    {
        if (SkinsUnlock.Count == 0)
        {
            NewSkinPanel.SetActive(false);
            return;
        }
        NewSkinPanel.SetActive(true);   
        var s = SkinsUnlock.Dequeue();
        newSkin.SetPropertyForPanel(s);
    }

    private void OnCompleteLevel()
    {
        TotalChallengeCompleted++;
    }
    

    #region Update
    private void Update()
    {
        
        skins = PagesManager.Instance.skins;
        foreach (var skin in skins)
        {
            switch (skin.id)
            {
                case 101:
                    if (PlayerPrefs.GetInt(skin.id.ToString()) == 0 && TotalScore >= 25)
                    {
                        SkinsUnlock.Enqueue(skin);
                        PlayerPrefs.SetInt(skin.id.ToString(), 1);
                        OnSkinUnlock();
                    }
                    break;
                case 102:
                    if (PlayerPrefs.GetInt(skin.id.ToString()) == 0 && TotalEndless >= 10)
                    {
                        SkinsUnlock.Enqueue(skin);
                        PlayerPrefs.SetInt(skin.id.ToString(), 1);
                        OnSkinUnlock();
                    }
                    break;
                case 103:
                    if (PlayerPrefs.GetInt(skin.id.ToString()) == 0 && TotalEndless >= 5)
                    {
                        SkinsUnlock.Enqueue(skin);
                        PlayerPrefs.SetInt(skin.id.ToString(), 1);
                        OnSkinUnlock();
                    }
                    break;    
                case 104:
                    if (PlayerPrefs.GetInt(skin.id.ToString()) == 0 && ReachSwishx4 != 0)
                    {
                        SkinsUnlock.Enqueue(skin);
                        PlayerPrefs.SetInt(skin.id.ToString(), 1);
                        OnSkinUnlock();
                    }
                    break;
                case 105:
                    if (PlayerPrefs.GetInt(skin.id.ToString()) == 0 && SwishInSinglePlay >= 10)
                    {
                        SkinsUnlock.Enqueue(skin);
                        PlayerPrefs.SetInt(skin.id.ToString(), 1);
                        OnSkinUnlock();
                    }
                    break;
                case 106:
                    if (PlayerPrefs.GetInt(skin.id.ToString()) == 0 && TotalSwish >= 30)
                    {
                        SkinsUnlock.Enqueue(skin);
                        PlayerPrefs.SetInt(skin.id.ToString(), 1);
                        OnSkinUnlock();
                    }
                    break;
                case 107:
                    if (PlayerPrefs.GetInt(skin.id.ToString()) == 0 && TotalScore >= 100)
                    {
                        SkinsUnlock.Enqueue(skin);
                        PlayerPrefs.SetInt(skin.id.ToString(), 1);
                        OnSkinUnlock();
                    }
                    break; 
                case 201:
                    if (PlayerPrefs.GetInt(skin.id.ToString()) == 0 && HoopPassedInEndlessMode >= 20)
                    {
                        SkinsUnlock.Enqueue(skin);
                        PlayerPrefs.SetInt(skin.id.ToString(), 1);
                        OnSkinUnlock();
                    }
                    break;
                case 202:
                    if (PlayerPrefs.GetInt(skin.id.ToString()) == 0 && HoopPassedInEndlessMode >= 10)
                    {
                        SkinsUnlock.Enqueue(skin);
                        PlayerPrefs.SetInt(skin.id.ToString(), 1);
                        OnSkinUnlock();
                    }
                    break;
                case 203:
                    if (PlayerPrefs.GetInt(skin.id.ToString()) == 0 && TotalSwish >= 20)
                    {
                        SkinsUnlock.Enqueue(skin);
                        PlayerPrefs.SetInt(skin.id.ToString(), 1);
                        OnSkinUnlock();
                    }
                    break;
                case 204:
                    if (PlayerPrefs.GetInt(skin.id.ToString()) == 0 && TotalScore >= 30)
                    {
                        SkinsUnlock.Enqueue(skin);
                        PlayerPrefs.SetInt(skin.id.ToString(), 1);
                        OnSkinUnlock();
                    }
                    break;
                case 205:
                    if (PlayerPrefs.GetInt(skin.id.ToString()) == 0 && TotalSecondChance >= 10)
                    {
                        SkinsUnlock.Enqueue(skin);
                        PlayerPrefs.SetInt(skin.id.ToString(), 1);
                        OnSkinUnlock();
                    }
                    break;
                case 301:
                    if (PlayerPrefs.GetInt(skin.id.ToString()) == 0 && HoopPassedInEndlessMode >= 20)
                    {
                        SkinsUnlock.Enqueue(skin);
                        PlayerPrefs.SetInt(skin.id.ToString(), 1);
                        OnSkinUnlock();
                    }
                    break;
                case 302:
                    if (PlayerPrefs.GetInt(skin.id.ToString()) == 0 && EndGame42 > 0)
                    {
                        SkinsUnlock.Enqueue(skin);
                        PlayerPrefs.SetInt(skin.id.ToString(), 1);
                        OnSkinUnlock();
                    }
                    break;
                case 303:
                    if (PlayerPrefs.GetInt(skin.id.ToString()) == 0 && HoopPassedInSinglePlay >= 50)
                    {
                        SkinsUnlock.Enqueue(skin);
                        PlayerPrefs.SetInt(skin.id.ToString(), 1);
                        OnSkinUnlock();
                    }
                    break;
                case 304:
                    if (PlayerPrefs.GetInt(skin.id.ToString()) == 0 && TotalSkin >= 6)
                    {
                        SkinsUnlock.Enqueue(skin);
                        PlayerPrefs.SetInt(skin.id.ToString(), 1);
                        OnSkinUnlock();
                    }
                    break;
                case 305:
                    if (PlayerPrefs.GetInt(skin.id.ToString()) == 0 && TotalSkin >= 8)
                    {
                        SkinsUnlock.Enqueue(skin);
                        PlayerPrefs.SetInt(skin.id.ToString(), 1);
                        OnSkinUnlock();
                    }
                    break;
                case 401:
                    if (PlayerPrefs.GetInt(skin.id.ToString()) == 0 && HoopPassedInSinglePlay >= 30)
                    {
                        SkinsUnlock.Enqueue(skin);
                        PlayerPrefs.SetInt(skin.id.ToString(), 1);
                        OnSkinUnlock();
                    }
                    break;
                case 402:
                    if (PlayerPrefs.GetInt(skin.id.ToString()) == 0 && TotalChallengeCompleted >= 1)
                    {
                        SkinsUnlock.Enqueue(skin);
                        PlayerPrefs.SetInt(skin.id.ToString(), 1);
                        OnSkinUnlock();
                    }
                    break;
                case 403:
                    if (PlayerPrefs.GetInt(skin.id.ToString()) == 0 && TotalChallengeCompleted >= 2)
                    {
                        SkinsUnlock.Enqueue(skin);
                        PlayerPrefs.SetInt(skin.id.ToString(), 1);
                        OnSkinUnlock();
                    }
                    break;
                case 404:
                    if (PlayerPrefs.GetInt(skin.id.ToString()) == 0 && TotalChallengeCompleted >= 3)
                    {
                        SkinsUnlock.Enqueue(skin);
                        PlayerPrefs.SetInt(skin.id.ToString(), 1);
                        OnSkinUnlock();
                    }
                    break;
                case 405:
                    if (PlayerPrefs.GetInt(skin.id.ToString()) == 0 && TotalScore >= 100)
                    {
                        SkinsUnlock.Enqueue(skin);
                        PlayerPrefs.SetInt(skin.id.ToString(), 1);
                        OnSkinUnlock();
                    }
                    break;
            }
        }
    }
    #endregion

    public void WriteData()
    {
        PlayerPrefs.SetInt("TotalScore", TotalScore);
        PlayerPrefs.SetInt("TotalSwish", TotalSwish);
        PlayerPrefs.SetInt("TotalEndless", TotalEndless);
        PlayerPrefs.SetInt("TotalSecondChance", TotalSecondChance);
        PlayerPrefs.SetInt("TotalSkin", TotalSkin);
        PlayerPrefs.SetInt("HoopPassedInEndlessMode", HoopPassedInEndlessMode);
        PlayerPrefs.SetInt("TotalChallengeCompleted", TotalChallengeCompleted);
        PlayerPrefs.SetInt("ReachSwishx4", ReachSwishx4);
        PlayerPrefs.GetInt("EndGame42", EndGame42);
    }

    private void OnApplicationQuit()
    {
        WriteData();
    }
    
    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            WriteData();
        }
    }
    
    private void OnApplicationFocus(bool focus)
    {
        if (!focus)
        {
            WriteData();
        }
    }
}
