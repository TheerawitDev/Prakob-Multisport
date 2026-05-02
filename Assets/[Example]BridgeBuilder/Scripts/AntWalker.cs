using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class AntWalker : MonoBehaviour
{
    public float walkSpeed = 2f;

    private Rigidbody2D rb;
    private SpineAnimationController animController;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animController = GetComponentInChildren<SpineAnimationController>();
    }

    private void Start()
    {
        if (animController != null)
        {
            animController.PlayWalk();
        }
    }

    private void Update()
    {
        if (BridgeGameManager.Instance != null && !BridgeGameManager.Instance.isGameActive)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            return;
        }

        rb.linearVelocity = new Vector2(walkSpeed, rb.linearVelocity.y);
    }
}