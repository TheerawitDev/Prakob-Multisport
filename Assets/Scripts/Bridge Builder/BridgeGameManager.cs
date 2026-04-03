using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class BridgeGameManager : MonoBehaviour
{
    public static BridgeGameManager Instance { get; private set; }

    public int maxBaseHealth = 100;
    public int currentHealth;
    public int currentScore;
    public bool isGameActive = true;

    [Header("UI References")]
    public TextMeshProUGUI scoreText;
    public Image healthFill;
    public GameObject gameOverPanel;
    public Button retryButton;
    public Button quitButton;

    private void Awake()
    {
        Instance = this;
        currentHealth = maxBaseHealth;
    }

    private void Start()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(false);

        if (retryButton != null) retryButton.onClick.AddListener(RestartGame);
        if (quitButton != null) quitButton.onClick.AddListener(QuitGame);

        UpdateUI();
    }

    private void Update()
    {
        if (isGameActive && GameInputManager.Instance != null)
        {
            if (GameInputManager.Instance.IsActionDown(InputActionType.QuitGame))
            {
                TriggerGameOver();
            }
        }
    }

    public void AddScore(int amount)
    {
        if (!isGameActive) return;
        currentScore += amount;
        UpdateUI();
    }

    public void TakeDamage(int amount)
    {
        if (!isGameActive) return;
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxBaseHealth);
        UpdateUI();

        if (currentHealth <= 0)
        {
            TriggerGameOver();
        }
    }

    private void UpdateUI()
    {
        if (scoreText != null) scoreText.text = "Score: " + currentScore;
        if (healthFill != null) healthFill.fillAmount = (float)currentHealth / maxBaseHealth;
    }

    public void TriggerGameOver()
    {
        isGameActive = false;
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void QuitGame()
    {
        Application.Quit();
    }
}