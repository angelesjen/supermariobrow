using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    AudioManager AudioManager;
    public static GameManager Instance { get; private set; }
    public int world { get; private set; } = 1;
    public int stage { get; private set; } = 1;

    private int _lives = 3;
    public int lives
    {
        get => _lives;
        private set
        {
            _lives = value;
            OnLivesChanged?.Invoke(_lives);
        }
    }

    private bool gameStarted = false;

    public event System.Action<int> OnLivesChanged;

    private void Awake()
    {
        // Set execution order to -200 in Project Settings
        if (Instance != null && Instance != this)
        {
            DestroyImmediate(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeAudioManager();
    }

    private void InitializeAudioManager()
    {
        AudioManager = FindObjectOfType<AudioManager>();
        if (AudioManager == null)
        {
            GameObject audioObj = GameObject.FindGameObjectWithTag("Audio");
            if (audioObj != null)
            {
                AudioManager = audioObj.GetComponent<AudioManager>();
            }
            else
            {
                audioObj = new GameObject("AudioManager");
                audioObj.tag = "Audio";
                AudioManager = audioObj.AddComponent<AudioManager>();
                DontDestroyOnLoad(audioObj);
            }
        }
    }

    private void OnDestroy()
    {
        if(Instance == this)
        {
            Instance = null;
        }
    }

    private void Start()
    {
        if (!gameStarted)
        {
            NewGame();
            gameStarted = true;
        }

    }

    private void NewGame()
    {
        lives = 3;

        LoadLevel(1,1);

        AudioManager.PlayMusic(AudioManager.backgroundGame);

    }

    private void LoadLevel(int world, int stage)
    {
        this.world = world;
        this.stage = stage;

        SceneManager.LoadScene($"{world}-{stage}");
    }

    public void ResetLevel(float delay)
    {
        lives--;
        if (lives > 0)
        {
            StartCoroutine(ResetLevelWithTimer());
        }
        else
        {
            GameOver();
        }
    }

    public void ResetLevel()
    {
        ResetLevel(0f);
    }

    private void GameOver()
    {
        SceneManager.LoadScene("GameOver");
        Timer.Instance?.FullResetTimer();
        //NewGame();
    }

    public void HandleTimeExpired()
    {
        Debug.Log("Time expired!");
        GameOver();
    }

    private IEnumerator ResetLevelWithTimer()
    {
        Timer.Instance?.PauseTimer();

        yield return new WaitForSeconds(1f);

        LoadLevel(world, stage);
        AudioManager.RestartMusic();

        Timer.Instance?.ResumeTimer();
    }

}
