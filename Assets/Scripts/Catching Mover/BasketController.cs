using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BasketController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float minX = -8f;
    public float maxX = 8f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    private void FixedUpdate()
    {
        if (GameInputManager.Instance == null) return;

        Vector2 currentPos = rb.position;

        if (GameInputManager.Instance.IsActionPressed(InputActionType.MoveLeft))
        {
            currentPos.x -= moveSpeed * Time.fixedDeltaTime;
        }
        else if (GameInputManager.Instance.IsActionPressed(InputActionType.MoveRight))
        {
            currentPos.x += moveSpeed * Time.fixedDeltaTime;
        }

        currentPos.x = Mathf.Clamp(currentPos.x, minX, maxX);
        rb.MovePosition(currentPos);
    }
}