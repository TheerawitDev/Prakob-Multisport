using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.EventSystems;
using DG.Tweening;

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

    [Header("Camera Shake Settings")]
    public CameraShake cameraShake;
    public float damageShakeDuration = 0.2f;
    public float damageShakeAmount = 0.1f;
    public float gameOverShakeDuration = 0.5f;
    public float gameOverShakeAmount = 0.3f;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (antRigidbody != null) antRigidbody.simulated = false;

        if (gameOverPanel != null) gameOverPanel.SetActive(false);

        if (startButton != null)
        {
            startButton.onClick.AddListener(StartGame);
            SetupButtonAnimation(startButton);
        }
        if (retryButton != null)
        {
            retryButton.onClick.AddListener(RestartGame);
            SetupButtonAnimation(retryButton);
        }
        if (quitButton != null)
        {
            quitButton.onClick.AddListener(QuitGame);
            SetupButtonAnimation(quitButton);
        }

        if (cameraShake == null)
        {
            cameraShake = Camera.main.GetComponent<CameraShake>();
        }

        if (mainMenuPanel != null)
        {
            mainMenuPanel.SetActive(true);
            SetUIFocus(startButton.gameObject);

            mainMenuPanel.transform.localScale = Vector3.zero;
            mainMenuPanel.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        }

        if (scoreText != null) scoreText.text = "Score: " + currentScore;
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

        if (cameraShake != null)
        {
            cameraShake.Shake(damageShakeDuration, damageShakeAmount);
        }
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

        if (cameraShake != null)
        {
            cameraShake.Shake(gameOverShakeDuration, gameOverShakeAmount);
        }
    }

    private void ShowGameOverPanel()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            SetUIFocus(retryButton.gameObject);

            gameOverPanel.transform.localScale = Vector3.zero;
            gameOverPanel.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        }
    }

    private void SetUIFocus(GameObject firstSelected)
    {
        if (EventSystem.current != null && firstSelected != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(firstSelected);
        }
    }

    private void UpdateUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + currentScore;
            scoreText.transform.DOKill(true);
            scoreText.transform.DOPunchScale(Vector3.one * 0.2f, 0.2f, 5, 1);
        }
    }

    private void SetupButtonAnimation(Button btn)
    {
        if (btn == null) return;

        EventTrigger trigger = btn.gameObject.GetComponent<EventTrigger>();
        if (trigger == null) trigger = btn.gameObject.AddComponent<EventTrigger>();

        Vector3 originalScale = btn.transform.localScale;

        EventTrigger.Entry enterEntry = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
        enterEntry.callback.AddListener((data) => { btn.transform.DOScale(originalScale * 1.1f, 0.2f).SetEase(Ease.OutBack); });
        trigger.triggers.Add(enterEntry);

        EventTrigger.Entry exitEntry = new EventTrigger.Entry { eventID = EventTriggerType.PointerExit };
        exitEntry.callback.AddListener((data) => { btn.transform.DOScale(originalScale, 0.2f).SetEase(Ease.OutQuad); });
        trigger.triggers.Add(exitEntry);

        EventTrigger.Entry downEntry = new EventTrigger.Entry { eventID = EventTriggerType.PointerDown };
        downEntry.callback.AddListener((data) => { btn.transform.DOScale(originalScale * 0.9f, 0.1f).SetEase(Ease.OutQuad); });
        trigger.triggers.Add(downEntry);

        EventTrigger.Entry upEntry = new EventTrigger.Entry { eventID = EventTriggerType.PointerUp };
        upEntry.callback.AddListener((data) => { btn.transform.DOScale(originalScale * 1.1f, 0.2f).SetEase(Ease.OutBack); });
        trigger.triggers.Add(upEntry);
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