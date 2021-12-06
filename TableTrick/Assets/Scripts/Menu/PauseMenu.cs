using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Listens for the onClick events for the Pause page menu buttons
/// </summary>
public class PauseMenu : MonoBehaviour
{
    /// <summary>
    /// Use this for initialization
    /// </summary>
    void Start()
    {
        // pause the game when added to the scene
        //Time.timeScale = 0;
    }

    /*
    /// <summary>
    /// Handles the on click event from the Resume button
    /// </summary>
    public void HandleResumeButtonOnClickEvent()
    {
        AudioManager.PlaySE(SoundEffectClipName.ButtonClickSound);

        // unpause game and destroy menu
        //Time.timeScale = 1;
        Destroy(gameObject);
    }*/

    /// <summary>
    /// Handles the on click event from the Save and Quit button
    /// </summary>
    public void HandleSaveQuitButtonOnClickEvent()
    {
        AudioManager.PlaySE(SoundEffectClipName.ButtonClickSound);

        // unpause game, destroy menu, and go to main menu
        //Time.timeScale = 1;
        Destroy(gameObject);
        
        // save level and score

        MenuManager.GotoMenu(MenuName.MainMenu);
    }
}
