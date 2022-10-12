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
            if (gameObject.name.Contains("extra"))
            {
                target = new Vector3(target.x, target.y * -1, target.z);
            }
            else
            {
                target *= -1;
            }
        }

        if (hoopController.IsPassed)
        {
            //hoopController.IsPassed = false;
            //ObjectPooler.Instance.ReleaseObject(gameObject);
        }
    }

    
}
