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
    public int lives { get; private set; } = 3;

    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
            AudioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
            DontDestroyOnLoad(gameObject);
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
        NewGame();
        AudioManager.PlayMusic(AudioManager.backgroundGame);

    }

    private void NewGame()
    {
        lives = 3;

        LoadLevel(1,1);
    }

    private void LoadLevel(int world, int stage)
    {
        this.world = world;
        this.stage = stage;

        SceneManager.LoadScene($"{world}-{stage}");
    }

    public void NextLevel()
    {
        /* if world > 1 and many stages
        if(world==1 && stage == 10)
        {
            LoadLevel(world + 1, 1);
        }
        */
        LoadLevel(world, stage + 1);
    }

    public void ResetLevel(float delay)
    {
        //CancelInvoke(nameof(ResetLevel));
        Invoke(nameof(ResetLevel), delay);
    }

    public void ResetLevel()
    {
        lives--;
        if ( lives > 0)
        {
            LoadLevel(world, stage);
        }
        else
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        SceneManager.LoadScene("GameOver");
        //NewGame();
    }
}
