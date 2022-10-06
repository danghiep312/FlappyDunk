using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class WingController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator anim;
    [SerializeField] private Vector3 startPos;

    [SerializeField] private Vector3 targetRotation;

    [SerializeField] private bool isFalling;

    private void Start()
    {
        targetRotation = new Vector3(0, 0, 120f);
        startPos = transform.localPosition;
    }

    private void Update()
    {
        if (GameManager.Instance.GameOver)
        {
            DOTween.Kill(transform);
        }
    }

    public void FlapWing()
    {
        DOTween.Kill(transform);
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        transform.DOLocalRotate(targetRotation, 0.1f).OnComplete(() =>
        {
            transform.DOLocalRotate(Vector3.zero, 0.4f).SetEase(Ease.OutQuad);
        });
    }
    
    public void ResetPosition()
    {
        rb.angularVelocity = 0;
        isFalling = false;
        transform.localPosition = startPos;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.gravityScale = 0;
        rb.velocity = Vector2.zero;
    }

    public void WingFall(int mode) //mode = 0 -> collision with ceil
    {
        if (isFalling) return;
        isFalling = true;
        if (transform.parent == null) return;
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 1;
        if (mode == 0)
        {
            rb.AddForce(transform.localPosition.x < 0
                ? new Vector2(Random.Range(-40, -10), 0)
                : new Vector2(Random.Range(10, 40), 0));
        }
        else
        {
            rb.AddForce(transform.localPosition.x < 0
                ? new Vector2(Random.Range(-60, -30), Random.Range(220, 300))
                : new Vector2(Random.Range(30, 60), Random.Range(220, 300)));
        }

        rb.angularVelocity = Random.Range(150f, 200f);
        if (Random.Range(0, 2) == 1)
        {
            rb.angularVelocity *= -1;
        }
    }
    
    
}
