using UnityEngine;

public class ItemCollector : MonoBehaviour
{
    public string collectorTag = "Player";
    public int scoreValue = 10;
    public int healValue = 0;
    public GameObject collectEffect;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(collectorTag))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddScore(scoreValue);
            }

            if (healValue > 0)
            {
                HealthSystem health = other.GetComponent<HealthSystem>();
                if (health != null)
                {
                    health.Heal(healValue);
                }
            }

            if (collectEffect != null)
            {
                Instantiate(collectEffect, transform.position, Quaternion.identity);
            }

            Destroy(gameObject);
        }
    }
}