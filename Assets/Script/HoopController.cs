using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using MoreMountains.NiceVibrations;

public class HoopController : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private SpriteRenderer backHoop;
    [SerializeField] private SpriteRenderer frontHoop;
    [SerializeField] private Collider2D[] colliderHolder;
    [SerializeField] private bool touchedHoop;
    [SerializeField] private GameObject belowPoint;
    [SerializeField] private GameObject abovePoint;
    [SerializeField] private bool isPassed;
    [SerializeField] private Vector3 originalScale;
    
    [SerializeField] private GameObject ball;
    private BallController ballController;
    private bool gotPoint;

    public bool IsPassed { get => isPassed; set => isPassed = value; }
    public Vector3 OriginalScale { get => originalScale; set => originalScale = value; }

    private void OnEnable()
    {
        originalScale = transform.localScale;
        gotPoint = touchedHoop = false;
    }

    private void Start()
    {
        ball = GameObject.FindGameObjectWithTag("Player");
        ballController = ball.GetComponent<BallController>();
    }

    private void Update()
    {

        if (!IsPassed && transform.position.x < ball.transform.position.x - 2f)
        {
            GameManager.Instance.GameOver = true;
        }

    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (isPassed) return;
        if (!col.gameObject.CompareTag("Player")) return;
        //Debug.Log("Hit");

        if (col.relativeVelocity.magnitude > 2)
        {
            AudioManager.Instance.Play("Bounce");
        }
        touchedHoop = true;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.gameObject.CompareTag("Player")) return;
        if (IsPassed) return;
        var allowDirection = belowPoint.transform.position - abovePoint.transform.position;

        var direction = Vector2.Angle(allowDirection, ball.GetComponent<Rigidbody2D>().velocity);

        if (transform.localRotation == Quaternion.Euler(Vector3.forward * 90f))
        {
            return;
        }
         
        if (direction >= 90f)
        {
            GameManager.Instance.GameOver = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (GameManager.Instance.GameOver) return;
        if (!col.gameObject.CompareTag("Player")) return;

        var disToBelowPoint = Vector2.Distance(ball.transform.position, belowPoint.transform.position);
        var disToAbovePoint = Vector2.Distance(ball.transform.position, abovePoint.transform.position);
        
        if (disToBelowPoint > disToAbovePoint) return;
        
        if (!gotPoint)
        {
            gotPoint = true;
            if (!touchedHoop) // reach perfect
            {
                PlayParticles();
                ballController.Streak += 1;
                if (ballController.Streak == 2)
                {
                    ballController.PlaySmokeParticle();
                } 
                else if (ballController.Streak >= 3)
                {
                    this.PostEvent(EventID.Shake);
                }

                if (ballController.Streak == 3)
                {
                    ballController.PlaySwishEffect(true);
                }
                GameManager.Instance.ShowPerfectNotification(ballController.Streak);
                switch (ballController.Streak)
                {
                    case 1:
                        AudioManager.Instance.Play("Pass");
                        break;
                    case 2:
                        AudioManager.Instance.Play("X2");
                        break;
                    case 3:
                        AudioManager.Instance.Play("X3");
                        break;
                    case 4:
                        AudioManager.Instance.Play("X4");
                        break;
                    default:
                        AudioManager.Instance.Play("X4");
                        break;
                }

                
                this.PostEvent(EventID.OnReachSwish);
                if (AudioManager.Instance.vibrationOn)
                {
                    MMVibrationManager.TransientHaptic(0.2f, 0.5f);
                }
            }
            else
            {
                AudioManager.Instance.Play("Pass"); 
            }
            
        }

        SetStatusCollider(false);
        if (!GameManager.Instance.GameOver)
        {
            ScoreManager.Instance.AddScore(ballController.Streak);
            
            if (!GameManager.Instance.PlayChallenge)
            {
                this.PostEvent(EventID.OnHoopPassed);
            }
        }

        anim.enabled = true;
        if (transform.parent.name.Contains("Level2"))
        {
            anim.Play("HoopDisappearInLevel2");
        }
        else
        {
            anim.Play(transform.parent.gameObject.CompareTag("Hoopv2") ? "HoopDisappearV2" : "HoopDisappear");
        }
        IsPassed = true;
        touchedHoop = false;
        gotPoint = false;
        if (!GameManager.Instance.PlayChallenge)
        {
            HoopSpawner.Instance.SpawnNewWave();
        }
        
    }

    
    public void NonCurrentHoop()
    {
        backHoop.color = new Color(1f, 1f, 1f, 0.5f);
        frontHoop.color = new Color(1f, 1f, 1f, 0.5f);
    }

    [Button("Gido")]
    public void IsCurrentHoop()
    {
        GameManager.Instance.PosOfLastHoop = transform.position;
        backHoop.color = Color.white;
        frontHoop.color = Color.white;
        SetStatusCollider(true);
        IsPassed = false;
    }
    
    public void SetStatusCollider(bool status)
    {
        foreach (var col in colliderHolder)
        {
            col.enabled = status;
        }
    }

    public void Release()
    {
        anim.enabled = false;
        transform.localScale = originalScale;
        if (GameManager.Instance.PlayChallenge)
        {
            gameObject.SetActive(false);
            return;
        }
        if (transform.parent.name.Contains("Hoopv2"))
        {
            ObjectPooler.Instance.ReleaseObject(transform.parent.gameObject);
            return;
        }
        ObjectPooler.Instance.ReleaseObject(gameObject);
    }

    public void ChangeSprite(Sprite[] sprites)
    {
        for (int i = 0; i < 2; i++)
        {
            transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = sprites[i];
        }
    }

    public void PlayParticles()
    {
        GameManager.Instance.SwishParticlePlay(transform.position);
    }

    public void ResetTransform()
    {
        transform.localScale = originalScale;
        isPassed = false;
        gotPoint = false;
        touchedHoop = false;
    }
    
}
