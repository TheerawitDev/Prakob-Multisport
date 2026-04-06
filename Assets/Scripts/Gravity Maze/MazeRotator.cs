using UnityEngine;

public class MazeRotator : MonoBehaviour
{
    public float rotationSpeed = 45f;
    private Vector3 calculatedCenter;

    private void Start()
    {
        CalculateMazeCenter();
    }

    private void CalculateMazeCenter()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();

        if (renderers.Length == 0)
        {
            calculatedCenter = transform.position;
            return;
        }

        Bounds bounds = renderers[0].bounds;
        for (int i = 1; i < renderers.Length; i++)
        {
            bounds.Encapsulate(renderers[i].bounds);
        }

        calculatedCenter = bounds.center;
    }

    private void Update()
    {
        if (GameInputManager.Instance == null) return;

        // บรรทัดนี้สำคัญมาก! ถ้าเกมยังไม่ Active (อยู่หน้า Main Menu หรือ Game Over) คำสั่ง Rotate จะไม่ทำงาน
        if (MazeGameManager.Instance != null && !MazeGameManager.Instance.isGameActive) return;

        if (GameInputManager.Instance.IsActionPressed(InputActionType.MoveLeft))
        {
            transform.RotateAround(calculatedCenter, Vector3.forward, rotationSpeed * Time.deltaTime);
        }
        else if (GameInputManager.Instance.IsActionPressed(InputActionType.MoveRight))
        {
            transform.RotateAround(calculatedCenter, Vector3.forward, -rotationSpeed * Time.deltaTime);
        }
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(calculatedCenter, 0.5f);
        }
    }
}