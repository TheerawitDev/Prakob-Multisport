using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class PlayerController2D : MonoBehaviour
{
    public float runSpeed = 5f;
    public float jumpForce = 10f;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    public Transform visualTransform;

    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    public bool isGrounded;
    public bool isCrouching;

    private Vector2 originalColliderSize;
    private Vector2 originalColliderOffset;
    private Vector3 originalVisualScale;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();

        originalColliderSize = boxCollider.size;
        originalColliderOffset = boxCollider.offset;

        if (visualTransform != null)
        {
            originalVisualScale = visualTransform.localScale;
        }
    }

    private void OnEnable()
    {
        if (GameInputManager.Instance != null)
        {
            GameInputManager.Instance.OnJumpPressed += Jump;
            GameInputManager.Instance.OnCrouchStart += Crouch;
            GameInputManager.Instance.OnCrouchEnd += StandUp;
        }
    }

    private void OnDisable()
    {
        if (GameInputManager.Instance != null)
        {
            GameInputManager.Instance.OnJumpPressed -= Jump;
            GameInputManager.Instance.OnCrouchStart -= Crouch;
            GameInputManager.Instance.OnCrouchEnd -= StandUp;
        }
    }

    private void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        rb.linearVelocity = new Vector2(runSpeed, rb.linearVelocity.y);
    }

    private void Jump()
    {
        Debug.Log("PlayerController2D: Jump Called!");

        if (isGrounded && !isCrouching)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    private void Crouch()
    {
        if (!isCrouching)
        {
            isCrouching = true;
            boxCollider.size = new Vector2(originalColliderSize.x, originalColliderSize.y / 2f);
            boxCollider.offset = new Vector2(originalColliderOffset.x, originalColliderOffset.y - (originalColliderSize.y / 4f));

            if (visualTransform != null)
            {
                visualTransform.localScale = new Vector3(originalVisualScale.x, originalVisualScale.y / 2f, originalVisualScale.z);
            }
        }
    }

    private void StandUp()
    {
        if (isCrouching)
        {
            isCrouching = false;
            boxCollider.size = originalColliderSize;
            boxCollider.offset = originalColliderOffset;

            if (visualTransform != null)
            {
                visualTransform.localScale = originalVisualScale;
            }
        }
    }
}