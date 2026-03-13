using UnityEngine;

public class DamageDealer2D : MonoBehaviour
{
    public int damageAmount = 10;
    public string targetTag = "Player";
    public bool destroyOnHit = false;
    public GameObject hitEffect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(targetTag))
        {
            HealthSystem targetHealth = collision.GetComponent<HealthSystem>();
            if (targetHealth != null)
            {
                targetHealth.TakeDamage(damageAmount);

                if (hitEffect != null)
                {
                    Instantiate(hitEffect, transform.position, Quaternion.identity);
                }

                if (destroyOnHit)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}