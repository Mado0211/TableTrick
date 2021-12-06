using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Retrieves and displays high score and listens for
/// the OnClick events for the high score menu button
/// </summary>
public class HighScoreMenu : MonoBehaviour
{
    [SerializeField]
    Text scoreMessage;

    [SerializeField]
    Text levelMessage;

    /// <summary>
    /// Use this for initialization
    /// </summary>
    void Start()
    {
        // pause the game when added to the scene
        Time.timeScale = 0;

        // retrieve and display high score
        if (PlayerPrefs.HasKey(GameDataName.HighestScore.ToString()))
        {
            levelMessage.text = "Top Level: " + PlayerPrefs.GetInt(GameDataName.HighestLevel.ToString());
            scoreMessage.text = "High Score: " + PlayerPrefs.GetInt(GameDataName.HighestScore.ToString());
        }
        else
        {
            scoreMessage.text = "No games played yet";
            levelMessage.text = "";
        }
    }

    /// <summary>
    /// Handles the on click event from the quit button
    /// </summary>
    public void HandleQuitButtonOnClickEvent()
    {
        AudioManager.PlaySE(SoundEffectClipName.ButtonClickSound);

        // unpause game and go to main menu
        Time.timeScale = 1;
        MenuManager.GotoMenu(MenuName.MainMenu);
    }
}
