using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FlightController : MonoBehaviour
{
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    private void FixedUpdate()
    {
        if (GameInputManager.Instance == null) return;
        if (FlightGameManager.Instance == null || !FlightGameManager.Instance.isGameActive) return;

        Vector2 currentPos = rb.position;
        float speed = FlightGameManager.Instance.playerMoveSpeed;
        float minY = FlightGameManager.Instance.playerMinY;
        float maxY = FlightGameManager.Instance.playerMaxY;

        if (GameInputManager.Instance.IsActionPressed(InputActionType.MoveUp))
        {
            currentPos.y += speed * Time.fixedDeltaTime;
        }
        else if (GameInputManager.Instance.IsActionPressed(InputActionType.MoveDown))
        {
            currentPos.y -= speed * Time.fixedDeltaTime;
        }

        currentPos.y = Mathf.Clamp(currentPos.y, minY, maxY);
        rb.MovePosition(currentPos);
    }
}