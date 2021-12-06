using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Listens for the OnClick events for the main menu buttons
/// </summary>
public class MainMenu : MonoBehaviour
{
    /// <summary>
    /// handle the on click event from the Play button
    /// </summary>
    public void HandlePlayButtonOnClickEvent()
    {
        AudioManager.PlaySE(SoundEffectClipName.ButtonClickSound);
        MenuManager.GotoMenu(MenuName.DifficultyMenu);
    }

    /// <summary>
    /// handle the on click event from the Help button
    /// </summary>
    public void HandleHelpButtonOnClickEvent()
    {
        AudioManager.PlaySE(SoundEffectClipName.ButtonClickSound);
        MenuManager.GotoMenu(MenuName.HelpMenu);
    }

    /// <summary>
    /// handle the on click event from the Quit button
    /// </summary>
    public void HandleQuitButtonOnClickEvent()
    {
        AudioManager.PlaySE(SoundEffectClipName.ButtonClickSound);
        MenuManager.GotoMenu(MenuName.Quit);
    }

    /// <summary>
    /// handle the on click event from the Hight Score button
    /// </summary>
    public void HandleHightScoreButtonOnClickEvent()
    {
        AudioManager.PlaySE(SoundEffectClipName.ButtonClickSound);
        MenuManager.GotoMenu(MenuName.HightScore);
    }

    /// <summary>
    /// handle the on click event from the Achievement button
    /// </summary>
    public void HandleAchievementButtonOnClickEvent()
    {
        AudioManager.PlaySE(SoundEffectClipName.ButtonClickSound);
        MenuManager.GotoMenu(MenuName.AchievementMenu);
    }
}
