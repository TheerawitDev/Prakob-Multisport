using UnityEngine;

public class FinishPoint : MonoBehaviour
{
    public string goodAntTag = "Ant";
    public string badAntTag = "BadAnt";

    public int scoreReward = 10;
    public int damagePenalty = 10;

    public GameObject successEffect;
    public GameObject damageEffect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(goodAntTag))
        {
            if (BridgeGameManager.Instance != null)
            {
                BridgeGameManager.Instance.AddScore(scoreReward);
            }

            if (successEffect != null)
            {
                Instantiate(successEffect, transform.position, Quaternion.identity);
            }

            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag(badAntTag))
        {
            if (BridgeGameManager.Instance != null)
            {
                BridgeGameManager.Instance.TakeDamage(damagePenalty);
            }

            if (damageEffect != null)
            {
                Instantiate(damageEffect, transform.position, Quaternion.identity);
            }

            Destroy(collision.gameObject);
        }
    }
}