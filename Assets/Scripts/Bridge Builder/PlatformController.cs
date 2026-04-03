using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlatformLifter : MonoBehaviour
{
    public InputActionType liftAction;

    public float liftSpeed = 5f;
    public float dropSpeed = 4f;
    public float defaultLocalY = -3f;
    public float maxLocalY = 2f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.useFullKinematicContacts = true;
    }

    private void FixedUpdate()
    {
        if (GameInputManager.Instance == null) return;

        Vector3 currentLocalPos = transform.localPosition;
        bool isPressing = GameInputManager.Instance.IsActionPressed(liftAction);

        if (isPressing)
        {
            currentLocalPos.y += liftSpeed * Time.fixedDeltaTime;
        }
        else
        {
            currentLocalPos.y -= dropSpeed * Time.fixedDeltaTime;
        }

        currentLocalPos.y = Mathf.Clamp(currentLocalPos.y, defaultLocalY, maxLocalY);

        Vector2 targetGlobalPos = transform.parent != null
            ? transform.parent.TransformPoint(currentLocalPos)
            : currentLocalPos;

        rb.MovePosition(targetGlobalPos);
    }
}