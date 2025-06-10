using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{

    //[SerializeField] private Texture2D cursorTexture;
    //[SerializeField] private Vector2 hotSpot;

    public void Restart()
    {
        //StartCoroutine(LoadSceneWithDelay());
        GameManager.Instance.resetLevelAfterOver();
        //SceneManager.LoadSceneAsync("1-1");
    }

    /*private IEnumerator LoadSceneWithDelay()
    {
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadSceneAsync("1-1");
    }*/

    public void BackToMain()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void Start()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMusic(AudioManager.Instance.backgroundOver);
        }
        else
        {
            Debug.LogError("AudioManager instance not found!");
        }
        //Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.Auto);
    }
}