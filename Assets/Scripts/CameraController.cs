using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform cameraTransform;
    private static float shakeDuration = 0f;
    private static float shakeAmount = 0.7f;
    public bool shake = false;

    private float vel;
    private Vector3 vel2 = Vector3.zero;

    private Vector3 originalPos;

    void Awake()
    {
        if (cameraTransform == null)
        {
            cameraTransform = transform;
        }

        originalPos = cameraTransform.localPosition;
    }

    public void setShakeTime(float length, float strength)
    {
        shakeDuration = length;
        shakeAmount = strength;
    }

    // Update is called once per frame
    void Update()
    {
        if (shake)
        {
            Vector3 newPos = originalPos + Random.insideUnitSphere * shakeAmount;

            cameraTransform.localPosition = Vector3.SmoothDamp(cameraTransform.localPosition, newPos, ref vel2, 0.05f);

            shakeDuration -= Time.deltaTime;
            shakeAmount = Mathf.SmoothDamp(shakeAmount, 0, ref vel, 20f);
        }
        else
        {
            cameraTransform.localPosition = originalPos;
        }

    }

    public void ToggleShaking(bool go)
    {
        shake = go;
    }
}
