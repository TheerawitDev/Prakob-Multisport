using UnityEngine;

public class FallingItem : MonoBehaviour
{
    public enum ItemType { Good, Bad }

    public ItemType type;
    public int scoreValue = 10;
    public int damageValue = 1;
    public float fallSpeed = 5f;

    [Header("Animation")]
    public float rotationSpeed = 180f; // ความเร็วในการหมุนติ้วๆ (องศาต่อวินาที)

    public GameObject effectPrefab;

    private void Update()
    {
        // เลื่อนลงล่างอย่างเดียว (ต้องใส่ Space.World บังคับไว้ เพื่อให้มันร่วงลงพื้นเสมอ แม้ตัวมันจะหมุนเอียงอยู่ก็ตาม)
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime, Space.World);

        // หมุนตัวรอบแกน Z
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
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