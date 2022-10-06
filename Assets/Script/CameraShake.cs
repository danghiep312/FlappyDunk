using System;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;
using Sirenix.OdinInspector;
public class CameraShake : MonoBehaviour
{
    // Transform of the camera to shake. Grabs the gameObject's transform
    // if null.
    public Transform camTransform;
	
    // How long the object should shake for.
    public float shakeDuration;

    public float strength;
    // public float duration;
    // // Amplitude of the shake. A larger value shakes the camera harder.
    // public float shakeAmount = 0.7f;
    // public float decreaseFactor = 1.0f;
    // public bool allowShake;
    

    private void Start()
    {
        this.RegisterListener(EventID.Shake, (param) => OnShake());        
    }
    
    private void OnShake()
    {
        StartCoroutine(Shake(shakeDuration, strength));
    }
    
    // private void Shake()
    // {
    //     if (shakeDuration > 0)
    //     {
    //         var shakePos = Random.insideUnitSphere * shakeAmount;
    //         camTransform.localPosition = transform.localPosition + shakePos;
    //         shakeDuration -= Time.deltaTime * decreaseFactor;
    //     }
    //     else
    //     {
    //         shakeDuration = duration;
    //         allowShake = false;
    //         camTransform.localPosition = Vector3.zero;
    //     }
    // }
    
    private IEnumerator Shake(float duration, float magnitude)
    {
        float timer = 0f;
        while (timer < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude * (duration - timer);
            float y = Random.Range(-1f, 1f) * magnitude * (duration - timer);
            transform.localPosition = new Vector3(x, y, 0);
 
            timer += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = Vector3.zero;
    }
    
}