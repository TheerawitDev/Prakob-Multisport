using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlatformLifter : MonoBehaviour
{
    public InputActionType liftAction;

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
        if (BridgeGameManager.Instance == null || !BridgeGameManager.Instance.isGameActive) return;

        Vector3 currentLocalPos = transform.localPosition;
        bool isPressing = GameInputManager.Instance.IsActionPressed(liftAction);

        float liftSpd = BridgeGameManager.Instance.liftSpeed;
        float dropSpd = BridgeGameManager.Instance.dropSpeed;
        float minY = BridgeGameManager.Instance.platformMinY;
        float maxY = BridgeGameManager.Instance.platformMaxY;

        if (isPressing)
        {
            currentLocalPos.y += liftSpd * Time.fixedDeltaTime;
        }
        else
        {
            currentLocalPos.y -= dropSpd * Time.fixedDeltaTime;
        }

        currentLocalPos.y = Mathf.Clamp(currentLocalPos.y, minY, maxY);

        Vector2 targetGlobalPos = transform.parent != null
            ? transform.parent.TransformPoint(currentLocalPos)
            : currentLocalPos;

        rb.MovePosition(targetGlobalPos);
    }
}