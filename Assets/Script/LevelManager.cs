using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Transform cam;
    [SerializeField] private GameObject[] hoopHolder;
    [SerializeField] private HoopController[] hoop;
    [SerializeField] private int index;
    [SerializeField] private Transform finsihLine;
    [SerializeField] private bool isLevelComplete;
    [SerializeField] private GameObject ball;
    [SerializeField] private BallController ballController;
    [SerializeField] private GameObject sensor;
    [SerializeField] private bool canJump;
    [SerializeField] private bool isNoticed;

    private WaitForSecondsRealtime wait = new WaitForSecondsRealtime(0.15f);
    private void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").transform;
        hoopHolder = new GameObject[transform.childCount - 2];
        hoop = new HoopController[hoopHolder.Length];
        for (int i = 0; i < hoopHolder.Length; i++)
        {
            hoopHolder[i] = transform.GetChild(i).gameObject;
            if (hoopHolder[i].name.Contains("v2"))
            {
                hoop[i] = hoopHolder[i].transform.GetChild(0).GetComponent<HoopController>();
            }
            else
            {
                hoop[i] = hoopHolder[i].GetComponent<HoopController>();
                hoopHolder[i].transform.localScale = hoop[i].OriginalScale;
            }
        }
        gameObject.SetActive(false);
        ball = GameObject.FindGameObjectWithTag("Player");
        ballController = ball.GetComponent<BallController>();
    }

    private void OnEnable()
    {
        index = 0;
        isLevelComplete = false;
        isNoticed = false;
        canJump = true;
        if (hoopHolder.Length > 0)
        {
            foreach (var item in hoopHolder)
            {
                if (item.gameObject.name.Contains("v2"))
                {
                    item.transform.GetChild(0).gameObject.SetActive(true);
                    item.transform.GetChild(0).GetComponent<HoopController>().NonCurrentHoop();
                }
                else
                {
                    item.SetActive(true);
                    item.GetComponent<HoopController>().NonCurrentHoop();
                }
            }
        }

        transform.position = cam.position + Vector3.forward * 10f;
        
        if (hoop.Length > 0)
        {
            hoop[0].IsCurrentHoop();
        }
    }

    private void Update()
    {
        if (GameManager.Instance.GameOver) return;
        if (hoop[index].IsPassed)
        {
            if (index < hoop.Length - 1)
            {
                index++;
                hoop[index].IsCurrentHoop();
            }
        }
        
        if (ball.transform.position.x > finsihLine.position.x && !GameManager.Instance.GameOver)
        {
            GameManager.Instance.CanTap = false;
            if (!isNoticed)
            {
                CanvasManager.Instance.Hud(false);
                ballController.ResetVelocity();
                isNoticed = true;
                AudioManager.Instance.Play("NewBestScore");
                GameManager.Instance.CompleteLevelNotification.SetActive(true);
                GameManager.Instance.SetStatusOfParticlesSystem(true, GameManager.Instance.CompleteLevelParticles);
            }
            isLevelComplete = true;
            if (PlayerPrefs.GetInt(gameObject.name) == 0)
            {
                this.PostEvent(EventID.OnCompleteLevel);
                PlayerPrefs.SetInt(gameObject.name, 1);
            }
        }

        if (isLevelComplete)
        {
            AfterFinish();
            GameManager.Instance.GameOver = false;
        }

        if (finsihLine.transform.position.x < cam.position.x - GameManager.Instance.HorizontalScreen / 2f - 0.5f 
            && GameManager.Instance.PlayChallenge && isLevelComplete)
        {
            GameManager.Instance.CompletedChallenge = true;
        }
        
        if (ball.transform.position.x > cam.position.x + GameManager.Instance.HorizontalScreen / 2f + 0.5f)
        {
            GameManager.Instance.GoHomeFromChallenge();
        }

        //Debug.Log("Complete: " + isLevelComplete + ", CanJump: " + canJump);
    }

    private void AfterFinish()
    {
        if (sensor.transform.position.y > ball.transform.position.y && canJump)
        {
            ballController.Jump();
            canJump = false;
            StartCoroutine(AfterJump());
        }
    }

    IEnumerator AfterJump()
    {
        yield return wait;
        canJump = true;
    }

    public void SpawnLastHoop()
    {
        for (int i = 0; i < hoopHolder.Length; i++)
        {
            if (Math.Abs(hoopHolder[i].transform.position.x - GameManager.Instance.PosOfLastHoop.x) < 0.1)
            {
                if (hoopHolder[i].gameObject.name.Contains("v2"))
                {
                    hoopHolder[i].transform.GetChild(0).gameObject.SetActive(true);
                }
                else 
                    hoopHolder[i].SetActive(true);
                hoop[i].IsCurrentHoop();
            }
        }
    }

    public void SetSpriteBeforePlay()
    {
        foreach (var h in hoop)
        {
            h.ChangeSprite(GameManager.Instance.HoopSprite);
        }
    }
}
