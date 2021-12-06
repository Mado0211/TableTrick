using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Listens for the OnClick events for the Achievement menu button
/// </summary>
public class AchievementMenu : MonoBehaviour
{
    const int emblemNumber = 6;

    //support alter text
    AchievementEmblem[] emblemScript;


    //support condition
    int topScore;
    int topLevel;

    /// <summary>
    /// Use this for initialization
    /// </summary>
    void Start()
    {
        int id;
        AchievementEmblem script;

        //put script into emblemScript according to ID
        emblemScript = new AchievementEmblem[emblemNumber];
        GameObject[] achievementEmblem = GameObject.FindGameObjectsWithTag("Emblem");

        for (int i = 0; i < emblemNumber; i++)
        {
            script = achievementEmblem[i].GetComponent<AchievementEmblem>();
            id = script.Id;
            emblemScript[id] = script;
        }

        //reads record
        if (PlayerPrefs.HasKey(GameDataName.HighestScore.ToString()))
        {
            topScore = PlayerPrefs.GetInt(GameDataName.HighestScore.ToString());
            topLevel = PlayerPrefs.GetInt(GameDataName.HighestLevel.ToString());
        }
        else
        {
            topScore = 0;
            topLevel = 0;
        }
        
        
        // test
        //topScore = 5000;
        //topLevel = 5000;
        // test

        //initailize emblem text
        for (int i=0;i< emblemScript.Length; i++)
        {
            emblemScript[i].SetRewardText("?????");
        }

        //change the emblem text
        if (topScore != 0)
        {
            emblemScript[0].SetConditionText("Round Completed");
            emblemScript[0].SetRewardText("Just began");
        }
        else
        {
            emblemScript[0].SetConditionText("Round Completed");
        }

        //emblem 1
        if (topScore > 100)
        {
            emblemScript[1].SetConditionText("Scored over 100");
            emblemScript[1].SetRewardText("Is it easy?");
        }
        else
        {
            emblemScript[1].SetConditionText("Scored over 100");
        }

        //emblem 2
        if (topScore > 500)
        {
            emblemScript[2].SetConditionText("Scored over 500");
            emblemScript[2].SetRewardText("Great, show me more!");
        }
        else
        {
            emblemScript[2].SetConditionText("Scored over ?");
        }

        //emblem 3
        if (topLevel > 7)
        {
            emblemScript[3].SetConditionText("Level over 7");
            emblemScript[3].SetRewardText("You are just not lucky!");
        }
        else
        {
            emblemScript[3].SetConditionText("Level over 7");
        }

        //emblem 4
        if (topLevel > 15)
        {
            emblemScript[4].SetConditionText("Level over 15");
            emblemScript[4].SetRewardText("Your ability surpasses my imagination!");
        }
        else
        {
            emblemScript[4].SetConditionText("Level over ?");
        }

        emblemScript[emblemScript.Length - 1].SetConditionText("???");
    }

    /// <summary>
    /// Handles the on click event from the back button
    /// </summary>
    public void HandleBackButtonOnClickEvent()
    {
        AudioManager.PlaySE(SoundEffectClipName.ButtonClickSound);
        MenuManager.GotoMenu(MenuName.MainMenu);
    }
}
