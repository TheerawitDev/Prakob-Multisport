using UnityEngine;

public class DamageDealer2D : MonoBehaviour
{
    public int damageAmount = 10;
    public string targetTag = "Player";
    public bool destroyOnHit = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(targetTag))
        {
            HealthSystem targetHealth = collision.GetComponent<HealthSystem>();
            if (targetHealth != null)
            {
                targetHealth.TakeDamage(damageAmount);
                Debug.Log("HIT");
                if (destroyOnHit)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}