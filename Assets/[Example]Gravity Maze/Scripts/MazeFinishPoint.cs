using UnityEngine;

public class MazeFinishPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ant"))
        {
            if (MazeGameManager.Instance != null)
            {
                MazeGameManager.Instance.TriggerWin();
            }
        }
    }
}