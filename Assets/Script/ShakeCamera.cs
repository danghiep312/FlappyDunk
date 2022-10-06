using System;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;
using Sirenix.OdinInspector;
public class ShakeCamera : MonoBehaviour
{
    // Transform of the camera to shake. Grabs the gameObject's transform
    // if null.
    public Transform camTransform;
	
    // How long the object should shake for.
    public float shakeDuration;

    public float duration;
    // Amplitude of the shake. A larger value shakes the camera harder.
    public float shakeAmount = 0.7f;
    public float decreaseFactor = 1.0f;
    public bool allowShake;
    
    Vector3 originalPos;

    private void Start()
    {
        this.RegisterListener(EventID.Shake, (param) => OnShake());        
    }

    [Button("ShakeTest")]
    private void OnShake()
    {
        allowShake = true;
    }

    private void Update()
    {
        if (allowShake)
        {
            Shake();
            originalPos = camTransform.position;
        }
    }
    
    
    private void Shake()
    {
        if (shakeDuration > 0)
        {
            var shakePos = Random.insideUnitCircle * shakeAmount;
            camTransform.localPosition = transform.localPosition + new Vector3(shakePos.x, shakePos.y, -10 - transform.localPosition.z);
            shakeDuration -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            shakeDuration = duration;
            allowShake = false;
            camTransform.position = new Vector3(camTransform.position.x, 0, -10);
        }
    }
    
    
}