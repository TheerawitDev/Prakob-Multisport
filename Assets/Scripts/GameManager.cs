using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int currentScore;

    public UnityEvent<int> OnScoreChanged;
    public UnityEvent OnGameOver;
    public UnityEvent OnLevelComplete;

    public bool IsGameActive { get; private set; } = true;

    [Header("Quit Setup")]
    public KeyCode keyboardQuitKey = KeyCode.Escape;
    public KeyCode quitButton1 = KeyCode.JoystickButton4;
    public KeyCode quitButton2 = KeyCode.JoystickButton5;
    public KeyCode quitButton3 = KeyCode.JoystickButton6;
    public KeyCode quitButton4 = KeyCode.JoystickButton7;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (IsGameActive)
        {
            if (Input.GetKeyDown(keyboardQuitKey) ||
                Input.GetKeyDown(quitButton1) ||
                Input.GetKeyDown(quitButton2) ||
                Input.GetKeyDown(quitButton3) ||
                Input.GetKeyDown(quitButton4))
            {
                GameOver();
            }
        }
    }

    public void AddScore(int amount)
    {
        if (!IsGameActive) return;

        currentScore += amount;
        OnScoreChanged?.Invoke(currentScore);
    }

    public void GameOver()
    {
        if (!IsGameActive) return;

        IsGameActive = false;
        OnGameOver?.Invoke();
    }

    public void LevelComplete()
    {
        if (!IsGameActive) return;

        IsGameActive = false;
        OnLevelComplete?.Invoke();
    }
}