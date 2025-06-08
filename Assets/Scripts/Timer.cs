using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public TMP_Text timerText;
    public float initialTime = 180f;
    private float timeRemaining;
    private bool isTimerRunning = true;
    public static Timer Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            timeRemaining = initialTime;
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        timerText = GameObject.Find("TimerText")?.GetComponent<TMP_Text>();
        UpdateTimerDisplay();
    }

    void Update()
    {
        if (!isTimerRunning || timerText == null) return;

        timeRemaining -= Time.deltaTime;
        timeRemaining = Mathf.Max(timeRemaining, 0f);

        if (timeRemaining <= 0f)
        {
            TimeExpired();
        }

        UpdateTimerDisplay();
    }

    public void UpdateTimerDisplay()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(timeRemaining / 60f);
            int seconds = Mathf.FloorToInt(timeRemaining % 60f);
            timerText.text = string.Format("{0:0}:{01:00}", minutes, seconds);
        }
    }

    public void FullResetTimer()
    {
        timeRemaining = initialTime;
        isTimerRunning = true;
        UpdateTimerDisplay();
    }

    public void PauseTimer()
    {
        isTimerRunning = false;
    }

    public void ResumeTimer()
    {
        isTimerRunning = true;
    }

    private void TimeExpired()
    {
        isTimerRunning = false;
        GameManager.Instance?.HandleTimeExpired();
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}