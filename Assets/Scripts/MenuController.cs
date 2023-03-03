using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MenuController: MonoBehaviour
{
    public Button gameButton;
    public Button mainMenuButton;
    public Button quitButton;

    GameStateManager gameStateManager;

    public void Start()
    {
        gameStateManager = FindObjectOfType<GameStateManager>();
        gameButton.onClick.AddListener(() => gameStateManager.StartGame());
        mainMenuButton.onClick.AddListener(() => gameStateManager.MainMenu());
        quitButton.onClick.AddListener(() => gameStateManager.QuitGame());
    }
}
