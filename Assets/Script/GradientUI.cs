using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GradientUI : MonoBehaviour
{
    [SerializeField]
    private Image fill;

    [SerializeField] private Gradient grad;
    public float timeElapsed;

    public void OnEnable()
    {
        timeElapsed = 5f;
    }

    private void FixedUpdate()
    {
        timeElapsed -= timeElapsed == 0 ? 0 : Time.deltaTime;
        fill.fillAmount = timeElapsed / 5f;
        fill.color = grad.Evaluate(timeElapsed / 5f);
        if (timeElapsed < 0)
        {
            timeElapsed = 0;
            GameManager.Instance.GoHome();
        }
    }
    
}
