using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.EventSystems;
using DG.Tweening;

public class FlightGameManager : MonoBehaviour
{
    public static FlightGameManager Instance { get; private set; }

    public bool isGameActive = false;

    [Header("Player Settings")]
    public int maxHealth = 3;
    public float playerMoveSpeed = 8f;
    public float playerMinY = -4.5f;
    public float playerMaxY = 4.5f;

    [Header("Environment Settings")]
    public float scrollSpeed = 5f;
    public float destroyX = -10f;

    [Header("Spawner Settings")]
    public GameObject[] spawnPrefabs;
    public float spawnInterval = 2f;
    public float spawnMinY = -4f;
    public float spawnMaxY = 4f;

    [Header("Interactable Settings")]
    public int goodItemScore = 10;
    public int badItemDamage = 1;

    [Header("Score Over Time")]
    public float scoreTickInterval = 1f;
    public int scorePerTick = 1;
    private float scoreTimer;

    [Header("Live Data")]
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
        currentHealth = maxHealth;
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

        UpdateUI();
    }

    private void Update()
    {
        if (isGameActive)
        {
            if (GameInputManager.Instance != null && GameInputManager.Instance.IsActionDown(InputActionType.QuitGame))
            {
                TriggerGameOver();
            }

            scoreTimer += Time.deltaTime;
            if (scoreTimer >= scoreTickInterval)
            {
                scoreTimer = 0f;
                AddScore(scorePerTick);
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
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
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
            healthFill.DOFillAmount((float)currentHealth / maxHealth, 0.3f).SetEase(Ease.OutCubic);
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