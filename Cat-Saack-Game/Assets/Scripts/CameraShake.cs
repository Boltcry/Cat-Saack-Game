using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField]
    private float SHAKE_AMOUNT_MODIFIER = 1.0f;

    [SerializeField] 
    private float maxShakeOffset = 0.3f;

    [Header("Shake Defaults")]
    [SerializeField]
    private float defaultAmount = 2f;
    [SerializeField]
    private float defaultDuration = 0.2f;

    private float shakeAmount;
    private float startAmount;
    private float shakeDuration;
    private float startDuration;
    private bool isRunning = false;


    public void ShakeCamera(float amount, float duration)
    {
        shakeAmount += amount;
        shakeDuration += duration;

        if (!isRunning)
        {
            StartCoroutine(Shake());
        }
    }

    public void ShakeCamera()
    {
        ShakeCamera(defaultAmount, defaultDuration);
    }

    IEnumerator Shake()
    {
        isRunning = true;

        while (shakeDuration > 0f)
        {
            Vector3 shakeVector = new Vector3(
                Random.Range(-1f, 1f) * Mathf.Clamp(shakeAmount * SHAKE_AMOUNT_MODIFIER, 0, maxShakeOffset),
                Random.Range(-1f, 1f) * Mathf.Clamp(shakeAmount * SHAKE_AMOUNT_MODIFIER, 0, maxShakeOffset),
                0f // Lock Z position
            );

            // Apply shake as an offset relative to the current position
            transform.localPosition += shakeVector;

            shakeDuration -= Time.deltaTime;
            shakeAmount = Mathf.Lerp(shakeAmount, 0, Time.deltaTime * 10f);

            // restore original position
            yield return null;
            transform.localPosition -= shakeVector;
        }

        isRunning = false;
    }
}
