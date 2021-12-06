using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// menu manager
/// </summary>
public static class MenuManager
{
    public static void GotoMenu(MenuName name)
    {
        switch (name)
        {
            case MenuName.ResultMenu:
                //Object.Instantiate(Resources.Load("ScoreLevelPage"));
                GameObject resultPage = GameObject.FindWithTag("ScoreLevelPage");
                resultPage.GetComponent<Canvas>().enabled = true;
                //Time.timeScale = 0;

                break;
            case MenuName.DifficultyMenu:
                //go to Difficulty Menu scene
                SceneManager.LoadScene("DifficultyMenu");
                //Debug.Log("DifficultyMenu");

                break;
            case MenuName.HelpMenu:
                //see help menu
                //Object.Instantiate(Resources.Load("HelpPage"));
                SceneManager.LoadScene("HelpMenu");
                //Debug.Log("HelpMenu");

                break;
            case MenuName.HightScore:

                //Debug.Log("HightScore");
                // deactivate MainMenuCanvas and instantiate prefab
                GameObject mainMenuCanvas = GameObject.Find("MainMenuCanvas");
                if (mainMenuCanvas != null)
                {
                    mainMenuCanvas.SetActive(false);
                }
                Object.Instantiate(Resources.Load("HighScoreMenu"));

                break;

            case MenuName.MainMenu:
                //go to main menu scene
                SceneManager.LoadScene("MainMenu");
                //Debug.Log("MainMenu");

                break;
            case MenuName.AchievementMenu:
                //go to Achievement scene
                SceneManager.LoadScene("AchievementMenu");

                break;
            case MenuName.GamePlay:
                //go to play game scene
                //SceneManager.LoadScene("GamePlay");

                break;
            
            /*case MenuName.PauseMenu:
                //pause game when playing
                Object.Instantiate(Resources.Load("PausePage"));
                

                break;*/
            
            case MenuName.GameOverMenu:
                //show game over message and socre
                //Object.Instantiate(Resources.Load("GameOverPage"));

                break;
            case MenuName.Quit:
                //quit the game
                Debug.Log("Quit");
                Application.Quit();

                break;
        }
    }
}