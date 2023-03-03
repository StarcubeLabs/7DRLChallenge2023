using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    ServicesManager servicesManager;
    // Start is called before the first frame update
    void Start()
    {
        servicesManager = FindObjectOfType<ServicesManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    public void MainMenu()
    {
        servicesManager.ResetServices();
        SceneManager.LoadScene("StartMenu", LoadSceneMode.Single);
    }

    public void GameOver()
    {
        servicesManager.ResetServices();
        SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
    }

    public void WinGame()
    {
        servicesManager.ResetServices();
        SceneManager.LoadScene("WinScreen", LoadSceneMode.Single);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
