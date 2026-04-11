using UnityEngine;

public class FlightInteractable : MonoBehaviour
{
    public enum InteractableType { Good, Bad }

    public InteractableType type;
    public GameObject effectPrefab;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (FlightGameManager.Instance != null)
            {
                if (type == InteractableType.Good)
                {
                    FlightGameManager.Instance.AddScore(FlightGameManager.Instance.goodItemScore);
                }
                else
                {
                    FlightGameManager.Instance.TakeDamage(FlightGameManager.Instance.badItemDamage);
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