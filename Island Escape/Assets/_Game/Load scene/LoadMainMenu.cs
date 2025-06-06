using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMainMenu : MonoBehaviour
{
    private string _mainMenuSceneName = "Main menu";
    private void Start()
    {
        SceneManager.LoadScene(_mainMenuSceneName);
    }
}
