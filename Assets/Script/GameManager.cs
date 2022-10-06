using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    #region Instance
    public static GameManager Instance;
    
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
    public ParticleSystem[] NewBestScoreNoti;
    public ParticleSystem[] HoopSwishParticles;
    public ParticleSystem[] CompleteLevelParticles;
    public GameObject CompleteLevelNotification;
    public GameObject NewBestScoreNotification;
    public int TryId;
    public bool PlayChallenge;
    public bool CompletedChallenge;
    public bool HaveNewSkin;
    public BackgroundScroll bg;
    public int PlayTime;
    public bool NewBestScore;
    public GameObject Ball;
    public AchievementManager Achievement;
    public Camera cam;
    public bool ClickSecondChance;
    [SerializeField] private GameObject notiSwish;
    
    [Header("Status of the game")]
    public bool GameOver;
    public bool InHome;
    public bool IsPlaying;
    public float HorizontalScreen;
    public float VerticalScreen;
    public bool CanTap;
    public Vector3 PosOfLastHoop;

    [Header("Sprite in GamePlay")]
    //Sprite for gameplay
    public Sprite BallSprite;
    public Sprite[] HoopSprite; // 0 is back 1 is front
    public Sprite WingSprite;
    public Color ColorOfFlame;
    
    
    [Header("Contain Level Prefabs")]
    public GameObject[] LevelsHolder;
    
    public int c = 0;//controller game over
    private void Start()
    {
        
        PlayTime = 0;
        NewBestScore = false;
        InHome = true;
        IsPlaying = false;
        GameOver = false;
        CanTap = true;
        Vector2 edgeVector = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
        HorizontalScreen = edgeVector.x * 2;
        VerticalScreen = edgeVector.y * 2;

        bg = GameObject.FindGameObjectWithTag("BackgroundHolder").GetComponent<BackgroundScroll>();
        Application.targetFrameRate = 60;
    }

    private void Update()
    {
        Ball.SetActive(IsPlaying);
        if (InHome)
        {
            if (HaveNewSkin && !NewBestScore)
            {
                AudioManager.Instance.Play("NewBestScore");
                HaveNewSkin = false;
            }
            if (NewBestScore)
            {
                AudioManager.Instance.Play("NewBestScore");
                NewBestScoreNotification.SetActive(true);
                SetStatusOfParticlesSystem(true, NewBestScoreNoti);
                NewBestScore = false;
            }
        }
    }

    public void LateUpdate()
    {
        if (GameOver)
        {
            c++;
            if (c == 1)
            {
                if (!PlayChallenge)
                {
                    if (ScoreManager.Instance.CurrentScore == 42)
                    {
                        this.PostEvent(EventID.EndGame42);
                    } 
                    PlayerPrefs.SetInt("LastScore", ScoreManager.Instance.CurrentScore);
                    if (ScoreManager.Instance.CurrentScore > PlayerPrefs.GetInt("BestScore"))
                    {
                        NewBestScore = true;
                        PlayerPrefs.SetInt("BestScore", ScoreManager.Instance.CurrentScore);
                    }
                }
                CanTap = false;
                CanvasManager.Instance.GameOver();
                AudioManager.Instance.Play("Wrong");
            }
        }
    }

    public void Respawn()
    {
        c = 0;
        PlayTime++;
        Ball.GetComponent<BallController>().Respawn();
        Ball.transform.position = new Vector3(PosOfLastHoop.x - 3f, 0, 0);
        if (PlayChallenge)
        {
            for (int i = 0; i < LevelsHolder.Length; i++)
            {
                if (LevelsHolder[i].activeSelf)
                {
                    LevelsHolder[i].GetComponent<LevelManager>().SpawnLastHoop();
                }
            }
        }
        Time.timeScale = 0;
        GameOver = false;
        CanTap = true;
        ClickSecondChance = true;
        CanvasManager.Instance.Hud(true);
        //bg.ResetPositionByGameObject(Ball);
        cam.transform.DOMoveX(Ball.transform.position.x + 1.5f, 0.5f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            ClickSecondChance = false;
        }).SetUpdate(true);
        this.PostEvent(EventID.OnUseSecondChance);
    }

    public void PlayGame()
    {
        if (!CanTap) return;
        Ball.GetComponent<BallController>().OffSpriteSwish();
        c = 0;
        ClickSecondChance = false;
        this.PostEvent(EventID.OnPlayEndless);
        PagesManager.Instance.SetSpriteById();
        SetSpriteForGameplay();
        Time.timeScale = 0;
        PlayTime++;
        InHome = false;
        IsPlaying = true;
        CanTap = true;
        GameOver = false;
        CanvasManager.Instance.PlayGame();
        Ball.GetComponent<BallController>().Respawn();
        Ball.transform.position = new Vector3(-1.5f + cam.transform.position.x, 0, 0);
        bg.FadeOutWhenPlaying();
        
        HoopSpawner.Instance.CallStart();
        HoopSpawner.Instance.PlayGame();
        AudioManager.Instance.Play("Whistle");
        foreach (var level in LevelsHolder)
        {
            level.SetActive(false);
        }
    }
    
    public void PlayGameChallenge(GameObject levelPrefab)
    {
        if (!CanTap) return;
        Ball.GetComponent<BallController>().OffSpriteSwish();
        c = 0;
        ClickSecondChance = false;
        Ball.GetComponent<BallController>().Respawn();
        Ball.transform.position = new Vector3(-1.5f + cam.transform.position.x, 0, 0);
        bg.FadeOutWhenPlaying();
        AudioManager.Instance.Play("Whistle");
        PagesManager.Instance.SetSpriteById();
        SetSpriteForGameplay();
        CanvasManager.Instance.PlayChallengeLevel();
        foreach (var level in LevelsHolder)
        {
            if (level.gameObject.name == levelPrefab.gameObject.name)
            {
                level.SetActive(true);
                level.GetComponent<LevelManager>().SetSpriteBeforePlay();
            }
            else
            {
                level.SetActive(false);
            }
        }
        Time.timeScale = 0;
        PlayTime++;
        InHome = false;
        IsPlaying = true;
        CanTap = true;
        PlayChallenge = true;
    }

    public void GoHome()
    {
        
        CanTap = false;
        if (PlayChallenge)
        {
            GoHomeFromChallenge();
            return;
        }
        PlayChallenge = false;
        PlayTime = 0;
        InHome = true;
        GameOver = false;
        IsPlaying = false;
        
        ScoreManager.Instance.ResetScore();
        CanvasManager.Instance.GoHome();
        bg.FadeInHome();
        if (Achievement.SkinsUnlock.Count > 0)
        {
            HaveNewSkin = true;
        }
        Achievement.ShowNewSkin();
    }
    
    public void GoHomeFromChallenge()
    {
        foreach (var lh in LevelsHolder)
        {
            lh.SetActive(false);
        }
        PlayChallenge = false;
        PlayTime = 0;
        InHome = true;
        GameOver = false;
        IsPlaying = false;
        Ball.transform.position = new Vector3(-1.5f + cam.transform.position.x, 0, 0);
        CanvasManager.Instance.GoHome();
        CanvasManager.Instance.ShowChallengePanel();
        bg.FadeInHome();
        Achievement.ShowNewSkin();
    }
    
    public bool IsMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject(0);
    }

    public void ShowPerfectNotification(int streak)
    {
        notiSwish.GetComponent<NotiSwish>().SetText(streak);
        notiSwish.SetActive(true);
    }
    
    public void SetSpriteForGameplay()
    {
        SetColorForBlast(HoopSwishParticles[0], ColorOfFlame);
        Ball.GetComponent<SpriteRenderer>().sprite = BallSprite;
        for (var i = 0; i < 2; i++)
        {
            Ball.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = WingSprite;
        }  
        ChangeHoopSprite();
        var main = Ball.transform.GetChild(4).GetChild(0).GetComponent<ParticleSystem>();
        main.startColor = ColorOfFlame;
    }

    public void ChangeHoopSprite()
    {
        foreach (var p in HoopSwishParticles)
        {
            if (p.gameObject.name.Contains("Star"))
            {
                p.textureSheetAnimation.SetSprite(0, PagesManager.Instance.FindSkinById(PlayerPrefs.GetInt("HoopID")).starOfHoop);  
            }
        }
        for (int i = 0; i < ObjectPooler.Instance.pooledGameObjects.Count; i++)
        {
            var g = ObjectPooler.Instance.pooledGameObjects[i];
            g.SetActive(true);
            if (g.CompareTag("Hoop"))
            {
                g.GetComponent<HoopController>().ChangeSprite(HoopSprite);
            }

            if (g.CompareTag("Hoopv2"))
            {
                g.transform.GetChild(0).GetComponent<HoopController>().ChangeSprite(HoopSprite);
            }
            g.SetActive(false);
        }
    }
    

    public bool IsPlayingLevel3()
    {
        return LevelsHolder.Any(l => l.gameObject.name.Contains("Level3") && l.activeSelf);
    }

    public void TrySkin()
    {
        PlayGame();
        PagesManager.Instance.TrySkin(TryId);
        SetSpriteForGameplay();
        SetColorForBlast(HoopSwishParticles[0], ColorOfFlame);
        HoopSpawner.Instance.CallStart();
        HoopSpawner.Instance.PlayGame();
    }

    public void SetStatusOfParticlesSystem(bool status, ParticleSystem[] p)
    {
        for (int i = 0; i < p.Length; i++)
        {
            p[i].gameObject.SetActive(status);
            if (status)
            {
                var tmp = p[i].main;
                tmp.startDelay = 0.3f * i;
                p[i].Play();
            }
        }
    }

    private bool ParticlesSystemIsPlaying()
    {
        return CompleteLevelParticles.Any(p => p.isPlaying);
    }

    public void SwishParticlePlay(Vector3 pos)
    {
        foreach (var p in HoopSwishParticles)
        {
            p.transform.position = pos;
            p.Play();
        }
    }

    public void SetColorForBlast(ParticleSystem p, Color c)
    {
        var tmp = p.main;
        tmp.startColor = new Color(c.r, c.g, c.b, 1);       
    }

    
}
