using UnityEngine;

public class AbyssZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ant") || collision.CompareTag("BadAnt"))
        {
            Destroy(collision.gameObject);
        }
    }
}