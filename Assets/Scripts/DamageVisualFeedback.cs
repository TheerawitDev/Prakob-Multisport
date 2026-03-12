using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(HealthSystem))]
public class DamageVisualFeedback : MonoBehaviour
{
    public SpriteRenderer targetSprite;
    public float fadeDuration = 0.2f;
    public float cameraShakeDuration = 0.3f;
    public float cameraShakeStrength = 5f;

    private HealthSystem healthSystem;

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
    }

    private void OnEnable()
    {
        healthSystem.OnDamageTaken.AddListener(PlayFeedback);
    }

    private void OnDisable()
    {
        healthSystem.OnDamageTaken.RemoveListener(PlayFeedback);
    }

    private void PlayFeedback()
    {
        if (targetSprite != null && healthSystem.invincibilityDuration > 0)
        {
            int loops = Mathf.FloorToInt(healthSystem.invincibilityDuration / fadeDuration);
            if (loops % 2 == 0) loops++;

            targetSprite.DOComplete();
            targetSprite.DOFade(0.2f, fadeDuration).SetLoops(loops, LoopType.Yoyo).OnComplete(() => targetSprite.DOFade(1f, 0f));
        }

        if (Camera.main != null)
        {
            Camera.main.transform.DOComplete();
            Camera.main.transform.DOShakeRotation(cameraShakeDuration, new Vector3(0, 0, cameraShakeStrength));
        }
    }
}