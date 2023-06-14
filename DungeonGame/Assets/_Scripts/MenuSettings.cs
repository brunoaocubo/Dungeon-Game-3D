using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSettings : MonoBehaviour
{
    private void Start()
    {
        PlayerPrefs.SetInt("indexSelectedCharacter", 0);
    }

    public void StartGame() 
    {
        int value = 0;

        if(PlayerPrefs.GetInt("indexSelectedCharacter") != value) 
        {
            SceneManager.LoadSceneAsync(2, LoadSceneMode.Single);
        }
        else 
        {
            SceneManager.LoadScene(1, LoadSceneMode.Single);
        }
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
