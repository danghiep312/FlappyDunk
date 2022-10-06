using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MakeColorProgress : MonoBehaviour
{
    public GameObject ShopPageHolder;
    public GameObject ChallengePageHolder;
    public Image fill;
    private void Start()
    {
        fill = GetComponent<Image>();
        ShopPageHolder = GameObject.FindGameObjectWithTag("ShopPageHolder");
        ChallengePageHolder = GameObject.FindGameObjectWithTag("ChallengePageHolder");
    }

    private void Update()
    {
        
    }
}
