using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using MoreMountains.NiceVibrations;

public class BallController : MonoBehaviour
{
    [SerializeField] private float jumpForce;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private int streak;
    [SerializeField] private bool didTap;
    [SerializeField] private float forwardSpeed;
    [SerializeField] private bool isDead;
    [SerializeField] private ParticleSystem smokeEffect;
    [SerializeField] private ParticleSystem[] swishEffects;
    [SerializeField] private WingController[] wingControllers = new WingController[2];

    [SerializeField] private SpriteRenderer[] offsetSprites;
    private int c = 0;
    private int cx4 = 0;
    
    #region GetSet
    
    public float JumpForce
    {
        get => jumpForce;
        set => jumpForce = value;
    }

    public int Streak
    {
        get => streak;
        set => streak = value;
    }

    public bool DidTap
    {
        get => didTap;
        set => didTap = value;
    }

    public bool IsDead
    {
        get => isDead;
        set => isDead = value;
    }
    #endregion

    private void Start()
    {
        isDead = false;
        streak = 1;
        didTap = false;
        for (int i = 0; i < 2; i++)
        {
            wingControllers[i] = transform.GetChild(i).GetComponent<WingController>();
        }
    }

    private void OnEnable()
    {
        streak = 1;
    }

    private void Update()// Input
    {
        if (GameManager.Instance.IsPlayingLevel3())
        {
            JumpForce = 14f;
        }
        else
        {
            jumpForce = 7f;
            rb.gravityScale = 2f;
            if (ScoreManager.Instance.CurrentScore > 40)
            {
                jumpForce = 8f;
            }
        }

        if (GameManager.Instance.GameOver)
        {
            forwardSpeed = 3f;
        }
        else
        {
            forwardSpeed = 7f;
            rb.gravityScale = 2f;
        }
        
        if (Input.GetMouseButtonDown(0) && GameManager.Instance.CanTap && GameManager.Instance.IsPlaying && !GameManager.Instance.IsMouseOverUI())
        {
            if (CanvasManager.Instance.CheckIsPausing())
            {
                CanvasManager.Instance.Resume();
            }
            Time.timeScale = 1;
            didTap = true;
            
        }

        if (streak == 4)
        {
            cx4++;
            if (cx4 == 1)
            {
                this.PostEvent(EventID.ReachSwishx4);
            }
        }
        else
        {
            cx4 = 0;
        }

        if (streak < 2)
        {
            smokeEffect.Stop();
        }
        
        isDead = GameManager.Instance.GameOver;
    }

    private void FixedUpdate() // Movement
    {
        if (GameManager.Instance.InHome) return;
        rb.AddForce(Vector2.right * forwardSpeed);

        if (didTap)
        {
            Jump();
        }
    }

    public void Jump()
    {
        AudioManager.Instance.Play("Flap");
        rb.velocity = new Vector2(rb.velocity.x, Vector2.up.y * jumpForce);
        didTap = false;
        foreach (var w in wingControllers)
        {
            w.FlapWing();
        }
    }
    
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
//        Debug.Log(rb.velocity.magnitude);
//        Debug.Log(collision.gameObject.name);
        if (streak >= 3)
        {
            PlaySwishEffect(false);
        }

        streak = 1;
        
        if (collision.gameObject.CompareTag("Hoop"))
        {
            rb.velocity *= 0.75f;
//            Debug.Log(rb.velocity);
        }
        
        if (collision.gameObject.CompareTag("Platform"))
        {
            rb.velocity *= 0.8f;
           
            if (rb.velocity.magnitude < 0.5f)
            {
                rb.velocity = Vector2.zero;
            }

            if (collision.relativeVelocity.magnitude > 1.5f)
            {
                AudioManager.Instance.Play("Bounce");
            }
            c++;

            if (c == 1)
            {
                AudioManager.Instance.Play("Crash");
                if (collision.transform.position.y > 0)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        wingControllers[i].WingFall(0);
                    }
                }
                else
                {
                    if (!GameManager.Instance.GameOver)
                    {
                        rb.AddForce(Vector2.up * jumpForce * 60);
                    }
                    for (int i = 0; i < 2; i++)
                    {
                        wingControllers[i].WingFall(1);
                    }
                }
            }
            
            if (!GameManager.Instance.IsPlaying || GameManager.Instance.GameOver || GameManager.Instance.InHome) return;
            GameManager.Instance.GameOver = true;
        }

        
    }

    public void Respawn()
    {
        c = 0;
        transform.rotation = Quaternion.Euler(Vector3.zero);
        rb.velocity = Vector2.zero;
        isDead = false;
        rb.angularVelocity = 0;
        foreach (var wingController in wingControllers)
        {
            wingController.ResetPosition();
        }
    }

    public void PlaySwishEffect(bool status)
    {
//        Debug.Log("Play " + status);
        InSwishX3(status);
        foreach (var p in swishEffects)
        {
            if (status)
            {
                p.Play();
            }
            else
            {   
                p.Stop();
            }
        }
    }

    public void PlaySmokeParticle()
    {
        smokeEffect.Play();
    }
    
    public void ResetVelocity()
    {
        rb.velocity = Vector2.zero;
    }

    public void InSwishX3(bool status)
    {
        if (status)
        {
            var c = swishEffects[1].main.startColor.color;
            foreach (var s in offsetSprites)
            {
                s.color = new Color(c.r, c.g, c.b, 1);
            }
        }
        else
        {
            foreach (var s in offsetSprites)
            {
                s.DOFade(0, 0.5f);
            }
        }
    }

    public void OffSpriteSwish()
    {
        foreach (var s in offsetSprites)
        {
            s.color = new Color(1, 1, 1, 0);
        }
    }
}
