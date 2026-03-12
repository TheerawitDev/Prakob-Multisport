using UnityEngine;

public class DeathZone : MonoBehaviour
{
    public string targetTag = "Player";
    public int deathDamage = 9999;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(targetTag))
        {
            HealthSystem targetHealth = collision.GetComponent<HealthSystem>();
            if (targetHealth != null)
            {
                targetHealth.TakeDamage(deathDamage, true);
            }
        }
    }
}