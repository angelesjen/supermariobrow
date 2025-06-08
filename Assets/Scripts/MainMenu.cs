using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    //[SerializeField] private Texture2D cursorTexture;
    //[SerializeField] private Vector2 hotSpot;

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
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMusic(AudioManager.Instance.backgroundMenu);
        }
        else
        {
            Debug.LogError("AudioManager instance not found!");
        }
        //Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.Auto);
    }
}