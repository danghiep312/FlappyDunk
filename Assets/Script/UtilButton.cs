using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UtilButton : MonoBehaviour
{
    private void Update()
    {
        //Debug.Log(gameObject.transform.parent.name);
        gameObject.SetActive(gameObject.name.Contains("Sound")
            ? AudioManager.Instance.soundOn
            : AudioManager.Instance.vibrationOn);
    }
}
