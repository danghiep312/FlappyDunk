using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    public GameObject ball;
    private void Update()
    {
        if (GameManager.Instance.GameOver) return;
        var pos = ball.transform.position.x;
        transform.position = Vector3.right * pos;
        transform.position += gameObject.name.Equals("Ground") ? new Vector3(1.5f, -GameManager.Instance.VerticalScreen / 2, 1) 
            : new Vector3(1.5f, GameManager.Instance.VerticalScreen / 2, 1);
    }
}
