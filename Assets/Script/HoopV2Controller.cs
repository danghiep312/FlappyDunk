using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoopV2Controller : MonoBehaviour
{
    [SerializeField] private GameObject hoop;
    [SerializeField] private float speed;
    [SerializeField] private Vector3 target;
    private Transform hoopTransform;
    private HoopController hoopController;
    private void Start()
    {
        hoopTransform = hoop.transform;
        hoopController = hoop.GetComponent<HoopController>();
    }
    private void Update()
    {
        hoopTransform.localPosition = Vector3.MoveTowards(hoopTransform.localPosition, target, speed * Time.deltaTime);
        if (hoopTransform.localPosition == target)
        {
            target *= -1;
        }

        if (hoopController.IsPassed)
        {
            //hoopController.IsPassed = false;
            //ObjectPooler.Instance.ReleaseObject(gameObject);
        }
    }

    
}
