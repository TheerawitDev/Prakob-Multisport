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

    private void Awake()
    {
        Instance = this;
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