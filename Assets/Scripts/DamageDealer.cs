using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    public int damageAmount = 10;
    public string targetTag = "Enemy";
    public bool destroyOnHit = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            HealthSystem targetHealth = other.GetComponent<HealthSystem>();
            if (targetHealth != null)
            {
                targetHealth.TakeDamage(damageAmount);

                if (destroyOnHit)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}