using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.EventSystems; // เพิ่มบรรทัดนี้เพื่อใช้งานระบบโฟกัส UI

public class MazeGameManager : MonoBehaviour
{
    public static MazeGameManager Instance { get; private set; }

    public bool isGameActive = false;
    public int currentScore;

    [Header("Gameplay References")]
    public Rigidbody2D antRigidbody;

    [Header("UI References")]
    public GameObject mainMenuPanel;
    public Button startButton;
    public TextMeshProUGUI scoreText;
    public GameObject gameOverPanel;
    public TextMeshProUGUI statusText;
    public Button retryButton;
    public Button quitButton;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (antRigidbody != null) antRigidbody.simulated = false;

        if (gameOverPanel != null) gameOverPanel.SetActive(false);

        if (startButton != null) startButton.onClick.AddListener(StartGame);
        if (retryButton != null) retryButton.onClick.AddListener(RestartGame);
        if (quitButton != null) quitButton.onClick.AddListener(QuitGame);

        // เปิดหน้า Main Menu และบังคับจอยให้ไปโฟกัสที่ปุ่ม Start
        if (mainMenuPanel != null)
        {
            mainMenuPanel.SetActive(true);
            SetUIFocus(startButton.gameObject);
        }

        UpdateUI();
    }

    public void StartGame()
    {
        isGameActive = true;

        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (antRigidbody != null) antRigidbody.simulated = true;
    }

    public void AddScore(int amount)
    {
        if (!isGameActive) return;
        currentScore += amount;
        UpdateUI();
    }

    public void DeductScore(int amount)
    {
        if (!isGameActive) return;
        currentScore -= amount;
        if (currentScore < 0) currentScore = 0;
        UpdateUI();
    }

    public void TriggerWin()
    {
        if (!isGameActive) return;
        isGameActive = false;
        if (statusText != null) statusText.text = "YOU WIN!";
        ShowGameOverPanel();
    }

    public void TriggerGameOver()
    {
        if (!isGameActive) return;
        isGameActive = false;
        if (statusText != null) statusText.text = "GAME OVER";
        ShowGameOverPanel();
    }

    private void ShowGameOverPanel()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            // บังคับจอยให้ไปโฟกัสที่ปุ่ม Retry ทันทีที่ตาย
            SetUIFocus(retryButton.gameObject);
        }
    }

    // ฟังก์ชันสำหรับบังคับเคอร์เซอร์จอย
    private void SetUIFocus(GameObject firstSelected)
    {
        if (EventSystem.current != null && firstSelected != null)
        {
            EventSystem.current.SetSelectedGameObject(null); // เคลียร์ของเก่าก่อน
            EventSystem.current.SetSelectedGameObject(firstSelected); // โฟกัสปุ่มใหม่
        }
    }

    private void UpdateUI()
    {
        if (scoreText != null) scoreText.text = "Score: " + currentScore;
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