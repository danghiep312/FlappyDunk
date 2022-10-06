using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraController : MonoBehaviour
{
    private Transform ball;
    private float offsetX;
    private Transform cam;
    public float speed;
    private int c;
    public float dur;
    private void Start()
    {
        c = 0;
        cam = transform;
        var go = GameObject.FindGameObjectWithTag("Player");
        if (go == null) return;
        ball = go.transform;
        offsetX = transform.position.x - ball.position.x;
    }

    private void LateUpdate()
    {
        if (GameManager.Instance.InHome || GameManager.Instance.CompletedChallenge) return;
        if (GameManager.Instance.GameOver)
        {
            if (c == 0)
            {
                speed = ball.GetComponent<Rigidbody2D>().velocity.x;
                DOVirtual.Float(speed, 0, dur, value => speed = value).SetEase(Ease.OutQuad);
            }
            c++;
        }
        else
        {
            
            speed = 0;
            c = 0;
            var pos = cam.position;
            if (ball == null) return;
            if (GameManager.Instance.ClickSecondChance) return;
            pos.x = ball.position.x + offsetX;
            pos.z = -10f;
            cam.position = pos;
        }

        
        cam.Translate(Vector3.right * (speed * Time.deltaTime));
        
    }
}
