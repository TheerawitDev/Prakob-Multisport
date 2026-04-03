using UnityEngine;

public class FallingItem : MonoBehaviour
{
    public enum ItemType { Good, Bad }

    public ItemType type;
    public int scoreValue = 10;
    public int damageValue = 1;
    public float fallSpeed = 5f;
    public GameObject effectPrefab;

    private void Update()
    {
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (CatchGameManager.Instance != null)
            {
                if (type == ItemType.Good)
                {
                    CatchGameManager.Instance.AddScore(scoreValue);
                }
                else
                {
                    CatchGameManager.Instance.TakeDamage(damageValue);
                }
            }

            if (effectPrefab != null)
            {
                Instantiate(effectPrefab, transform.position, Quaternion.identity);
            }

            Destroy(gameObject);
        }
    }
}