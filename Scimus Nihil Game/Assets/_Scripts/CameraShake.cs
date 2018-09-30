using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

    public Transform camTransform;

    public float shakeDuration = 1f;
    public float shakeDivision = 20f;
    public float shakeAmount = 0f;
    public float decreaseFactor = 1.0f;
    
    Vector3 originalPos;

    void Awake()
    {
        if (camTransform == null)
        {
            camTransform = GetComponent(typeof(Transform)) as Transform;
        }
    }

    public void OnEnable()
    {
        originalPos = camTransform.localPosition;
    }

    void Update()
    {
        shakeAmount = ((float)GameObject.FindGameObjectWithTag("Player").GetComponent<player1Controller>().nearCount) /*/ shakeDivision*/;
        if (shakeDuration > 0)
        {
            camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
            
            shakeDuration -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            shakeDuration = 1f;
            camTransform.localPosition = originalPos;
        }
    }
}
