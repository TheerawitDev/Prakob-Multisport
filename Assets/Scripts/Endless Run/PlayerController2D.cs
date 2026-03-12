using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class PlayerController2D : MonoBehaviour
{
    public float runSpeed = 5f;
    public float jumpForce = 500f;
    public float fallMultiplier = 2.5f;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    public Transform visualTransform;

    public bool IsGrounded { get; private set; }
    public bool IsCrouching { get; private set; }
    public Vector2 Velocity => rb.linearVelocity;

    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;

    private Vector2 originalColliderSize;
    private Vector2 originalColliderOffset;
    private Vector3 originalVisualScale;
    private Vector3 originalVisualPosition;

    private bool wasGrounded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();

        originalColliderSize = boxCollider.size;
        originalColliderOffset = boxCollider.offset;

        if (visualTransform != null)
        {
            originalVisualScale = visualTransform.localScale;
            originalVisualPosition = visualTransform.localPosition;
        }
    }

    private void Start()
    {
        if (GameInputManager.Instance == null) return;

        GameInputManager.Instance.OnJumpPressed += Jump;
        GameInputManager.Instance.OnCrouchStart += Crouch;
        GameInputManager.Instance.OnCrouchEnd += StandUp;
    }

    private void OnDestroy()
    {
        if (GameInputManager.Instance == null) return;

        GameInputManager.Instance.OnJumpPressed -= Jump;
        GameInputManager.Instance.OnCrouchStart -= Crouch;
        GameInputManager.Instance.OnCrouchEnd -= StandUp;
    }

    private void Update()
    {
        wasGrounded = IsGrounded;
        IsGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (!wasGrounded && IsGrounded && !IsCrouching && visualTransform != null)
        {
            visualTransform.DOComplete();
            visualTransform.DOScale(new Vector3(originalVisualScale.x * 1.3f, originalVisualScale.y * 0.7f, originalVisualScale.z), 0.15f)
                .OnComplete(() => visualTransform.DOScale(originalVisualScale, 0.15f));
        }

        rb.linearVelocity = new Vector2(runSpeed, rb.linearVelocity.y);

        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
    }

    private void Jump()
    {
        if (IsGrounded && !IsCrouching)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

            if (visualTransform != null)
            {
                visualTransform.DOComplete();
                visualTransform.DOScale(new Vector3(originalVisualScale.x * 0.7f, originalVisualScale.y * 1.3f, originalVisualScale.z), 0.15f)
                    .OnComplete(() => visualTransform.DOScale(originalVisualScale, 0.15f));
            }
        }
    }

    private void Crouch()
    {
        if (!IsCrouching)
        {
            IsCrouching = true;
            boxCollider.size = new Vector2(originalColliderSize.x, originalColliderSize.y / 2f);
            boxCollider.offset = new Vector2(originalColliderOffset.x, originalColliderOffset.y - (originalColliderSize.y / 4f));

            if (visualTransform != null)
            {
                visualTransform.DOComplete();
                visualTransform.localScale = new Vector3(originalVisualScale.x, originalVisualScale.y / 2f, originalVisualScale.z);
                visualTransform.localPosition = new Vector3(originalVisualPosition.x, originalVisualPosition.y - (originalVisualScale.y / 4f), originalVisualPosition.z);
            }
        }
    }

    private void StandUp()
    {
        if (IsCrouching)
        {
            IsCrouching = false;
            boxCollider.size = originalColliderSize;
            boxCollider.offset = originalColliderOffset;

            if (visualTransform != null)
            {
                visualTransform.DOComplete();
                visualTransform.localScale = originalVisualScale;
                visualTransform.localPosition = originalVisualPosition;
            }
        }
    }
}