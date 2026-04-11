using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    private Transform cameraTransform;
    private Vector3 originalPosition;
    public float defaultShakeDuration = 0.2f;
    public float defaultShakeAmount = 0.1f;
    public float decreaseFactor = 1.0f;

    private void Awake()
    {
        if (cameraTransform == null)
        {
            cameraTransform = GetComponent<Transform>();
        }
    }

    private void OnEnable()
    {
        originalPosition = cameraTransform.localPosition;
    }

    public void Shake(float duration, float amount)
    {
        StopAllCoroutines();
        StartCoroutine(DoShake(duration, amount));
    }

    private IEnumerator DoShake(float duration, float amount)
    {
        float currentDuration = duration;
        while (currentDuration > 0)
        {
            cameraTransform.localPosition = originalPosition + Random.insideUnitSphere * amount;
            currentDuration -= Time.deltaTime * decreaseFactor;
            yield return null;
        }
        cameraTransform.localPosition = originalPosition;
    }
}