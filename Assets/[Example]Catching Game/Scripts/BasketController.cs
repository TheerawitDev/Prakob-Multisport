using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BasketController : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpineAnimationController animController;
    private bool isWalking = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        animController = GetComponentInChildren<SpineAnimationController>();
    }

    private void Start()
    {
        if (animController != null)
        {
            animController.PlayIdle();
        }
    }

    private void FixedUpdate()
    {
        if (GameInputManager.Instance == null) return;
        if (CatchGameManager.Instance == null || !CatchGameManager.Instance.isGameActive) return;

        Vector2 currentPos = rb.position;
        bool isPressingMove = false;

        float speed = CatchGameManager.Instance.basketMoveSpeed;
        float minX = CatchGameManager.Instance.basketMinX;
        float maxX = CatchGameManager.Instance.basketMaxX;

        if (GameInputManager.Instance.IsActionPressed(InputActionType.MoveLeft))
        {
            currentPos.x -= speed * Time.fixedDeltaTime;
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            isPressingMove = true;
        }
        else if (GameInputManager.Instance.IsActionPressed(InputActionType.MoveRight))
        {
            currentPos.x += speed * Time.fixedDeltaTime;
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            isPressingMove = true;
        }

        currentPos.x = Mathf.Clamp(currentPos.x, minX, maxX);
        rb.MovePosition(currentPos);

        if (isPressingMove && !isWalking)
        {
            isWalking = true;
            if (animController != null) animController.PlayWalk();
        }
        else if (!isPressingMove && isWalking)
        {
            isWalking = false;
            if (animController != null) animController.PlayIdle();
        }
    }
}