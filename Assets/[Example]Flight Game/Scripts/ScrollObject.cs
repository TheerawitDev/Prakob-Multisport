using UnityEngine;

public class ScrollObject : MonoBehaviour
{
    private void Update()
    {
        if (FlightGameManager.Instance == null || !FlightGameManager.Instance.isGameActive) return;

        float speed = FlightGameManager.Instance.scrollSpeed;
        float destroyX = FlightGameManager.Instance.destroyX;

        transform.Translate(Vector3.left * speed * Time.deltaTime, Space.World);

        if (transform.position.x < destroyX)
        {
            Destroy(gameObject);
        }
    }
}