using UnityEngine;

public class EnemyReward : MonoBehaviour
{
    public int scoreValue = 10;
    public GameObject deathEffect;

    public void GiveRewardAndDie()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddScore(scoreValue);
        }

        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}