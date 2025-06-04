using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    AudioManager AudioManager;
    [SerializeField] private Texture2D cursorTexture;
    [SerializeField] private Vector2 hotSpot;

    private void Awake()
    {
        AudioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    public void PlayGame()
    {
        //StartCoroutine(LoadSceneWithDelay());
        SceneManager.LoadSceneAsync("1-1");
    }

    /*private IEnumerator LoadSceneWithDelay()
    {
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadSceneAsync("1-1");
    }*/

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Start()
    {
        AudioManager.PlayMusic(AudioManager.backgroundMenu);
        //Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.Auto);
    }
}