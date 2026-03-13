using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(HealthSystem))]
public class EnemyKnockback : MonoBehaviour
{
    public float knockbackDistance = 1f;
    public float knockbackDuration = 0.2f;

    private HealthSystem healthSystem;
    private EnemyWalk2D walkScript;

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
        walkScript = GetComponent<EnemyWalk2D>();
    }

    private void OnEnable()
    {
        healthSystem.OnDamageTaken.AddListener(ApplyKnockback);
    }

    private void OnDisable()
    {
        healthSystem.OnDamageTaken.RemoveListener(ApplyKnockback);
    }

    private void ApplyKnockback()
    {
        if (walkScript != null) walkScript.enabled = false;

        transform.DOKill();

        Vector3 direction = (transform.position.x > 0) ? Vector3.right : Vector3.left;

        transform.DOMove(transform.position + (direction * knockbackDistance), knockbackDuration)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                if (walkScript != null && healthSystem != null)
                {
                    walkScript.enabled = true;
                }
            });
    }
}