using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.EventSystems;
using DG.Tweening;

public class BridgeGameManager : MonoBehaviour
{
    public static BridgeGameManager Instance { get; private set; }

    public bool isGameActive = false;

    [Header("Platform Settings")]
    public float liftSpeed = 5f;
    public float dropSpeed = 4f;
    public float platformMinY = -3f;
    public float platformMaxY = 2f;

    [Header("Spawner Settings")]
    public GameObject[] antPrefabs;
    public float spawnInterval = 2f;

    [Header("Scoring Settings")]
    public int goodAntScore = 10;
    public int badAntDamage = 10;

    [Header("Player Health")]
    public int maxBaseHealth = 100;
    public int currentHealth;
    public int currentScore;

    [Header("UI References")]
    public GameObject mainMenuPanel;
    public Button startButton;
    public TextMeshProUGUI scoreText;
    public Image healthFill;
    public GameObject gameOverPanel;
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
        currentHealth = maxBaseHealth;
    }

    private void Start()
    {
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
        if (healthFill != null) healthFill.fillAmount = (float)currentHealth / maxBaseHealth;
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

    public void StartGame()
    {
        isGameActive = true;
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
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

        if (cameraShake != null)
        {
            cameraShake.Shake(damageShakeDuration, damageShakeAmount);
        }

        if (currentHealth <= 0)
        {
            TriggerGameOver();
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

        if (healthFill != null)
        {
            healthFill.DOKill();
            healthFill.DOFillAmount((float)currentHealth / maxBaseHealth, 0.3f).SetEase(Ease.OutCubic);
            healthFill.DOColor(Color.red, 0.1f).OnComplete(() => healthFill.DOColor(Color.white, 0.2f));
        }
    }

    public void TriggerGameOver()
    {
        isGameActive = false;
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            SetUIFocus(retryButton.gameObject);

            gameOverPanel.transform.localScale = Vector3.zero;
            gameOverPanel.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        }

        if (cameraShake != null)
        {
            cameraShake.Shake(gameOverShakeDuration, gameOverShakeAmount);
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