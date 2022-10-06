using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.Mathematics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class HoopSpawner : MonoBehaviour
{
    #region Instance

    public static HoopSpawner Instance;

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

    [SerializeField] private Camera cam;
    [SerializeField] private GameObject currentHoop;
    [SerializeField] private GameObject nextHoop;
    
    [SerializeField] public float minHoopY;
    [SerializeField] public float maxHoopY;
    
    [SerializeField] private string[] tagOfHoop;
    

    private GameObject GenerateHoop()
    {
        var choice = 0;
        if (ScoreManager.Instance.CurrentScore >= 30)
        {
            choice = Random.Range(0, 100) < 70 ? 0 : 1;
        }
        var newHoop = ObjectPooler.Instance.Spawn(tagOfHoop[choice]);
        if (choice == 1)
        {
            newHoop.transform.GetChild(0).GetComponent<HoopController>().ResetTransform();
        }
        else
        {
            newHoop.GetComponent<HoopController>().ResetTransform();
        }
        return newHoop;
    }

    private void SetTransform(GameObject hoop, float x)
    {
        var curScore = ScoreManager.Instance.CurrentScore;
        if (curScore >= 15)
        {
            // position
            hoop.transform.position = new Vector3(x, Random.Range(minHoopY, maxHoopY), 0);
            if (hoop.gameObject.name.Contains("v2"))
            {
                hoop.transform.position = new Vector3(x, Random.Range(minHoopY + 0.5f, maxHoopY - 0.5f), 0);
            }
        }
        else
        {
            hoop.transform.position = new Vector3(x, Random.Range(-0.7f, 0.7f), 0);
            hoop.transform.rotation = Quaternion.Euler(Vector3.zero);
        }
        
        // rotation
        if (curScore >= 15)
        {
            int val, val2;
            val = Random.Range(0, 2);
            hoop.transform.rotation = Quaternion.Euler(val == 0 ? Vector3.zero : Vector3.forward * 15f);
            if (curScore >= 30 && val == 1)
            {
                val2 = Random.Range(0, 2);
                hoop.transform.rotation = Quaternion.Euler(val2 == 0 ? Vector3.forward * 15f : Vector3.forward * 30f);
            }

            if (curScore >= 50 && val == 1)
            {
                val2 = Random.Range(0, 2);
                if (val2 == 1)
                {
                    hoop.transform.rotation = Quaternion.Euler(hoop.transform.rotation.eulerAngles * -1);
                }
            }
        }
    }

    public void SpawnNewWave()
    {
        currentHoop = nextHoop;
        if (currentHoop.gameObject.CompareTag("Hoopv2"))
        {
            currentHoop.transform.GetChild(0).GetComponent<HoopController>().IsCurrentHoop();
        }
        else
        {
            currentHoop.GetComponent<HoopController>().IsCurrentHoop();
        }
        nextHoop = GenerateHoop();
        SetTransform(nextHoop, currentHoop.transform.position.x + GameManager.Instance.HorizontalScreen);
        
        if (nextHoop.gameObject.CompareTag("Hoopv2"))
        {
            nextHoop.transform.GetChild(0).GetComponent<HoopController>().NonCurrentHoop();
        }
        else
        {
            nextHoop.GetComponent<HoopController>().NonCurrentHoop();
        }

    }

    public void CallStart()
    {
        ObjectPooler.Instance.ResetStatus();
        currentHoop = GenerateHoop();
        currentHoop.GetComponent<HoopController>().NonCurrentHoop();
        SetTransform(currentHoop, 1.5f + cam.transform.position.x);
        currentHoop.transform.position -= Vector3.up * (currentHoop.transform.position.y + 0.15f);
        nextHoop = GenerateHoop();
        nextHoop.GetComponent<HoopController>().NonCurrentHoop();
        SetTransform(nextHoop, currentHoop.transform.position.x + GameManager.Instance.HorizontalScreen);
        GameManager.Instance.PosOfLastHoop = currentHoop.transform.position;
    }

    public void PlayGame()
    {
        currentHoop.GetComponent<HoopController>().IsCurrentHoop();
    }
    
    
    
}
