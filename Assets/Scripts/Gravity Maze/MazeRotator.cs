using UnityEngine;

public class MazeRotator : MonoBehaviour
{
    public float rotationSpeed = 45f;

    private void Update()
    {
        if (GameInputManager.Instance == null) return;

        if (GameInputManager.Instance.IsActionPressed(InputActionType.MoveLeft))
        {
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        }
        else if (GameInputManager.Instance.IsActionPressed(InputActionType.MoveRight))
        {
            transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime);
        }
    }
}