using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinGame : MonoBehaviour
{

    public void Home()
    {
        //StartCoroutine(LoadSceneWithDelay());
        SceneManager.LoadSceneAsync("MainMenu");
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
            AudioManager.Instance.PlayMusic(AudioManager.Instance.ending);
        }
        else
        {
            Debug.LogError("AudioManager instance not found!");
        }
    }
}