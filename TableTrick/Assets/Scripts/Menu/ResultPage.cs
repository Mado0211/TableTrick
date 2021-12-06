using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// shows result when this question has been answered
/// </summary>
public class ResultPage : IntEventInvoker {

    [SerializeField]
    Image resultImage;
    Sprite RightSprite;
    Sprite WrongSprite;

    [SerializeField]
    Text levelText;

    [SerializeField]
    Text scoreText;

    [SerializeField]
    Text buttonNameText;

    [SerializeField]
    Image SaveQuitButtonImage;

    //support level system
    int previousLevel;

    //save result
    bool result;

    

    // Use this for initialization
    void Start()
    {
        //Load image
        RightSprite = Resources.Load<Sprite>("circle");
        WrongSprite = Resources.Load<Sprite>("cross");

        //stop time
        //Time.timeScale = 0;
        EventManager.AddShowingResultListener(ChangeResultText);
        //gameObject.SetActive(false);
        GetComponent<Canvas>().enabled = false;

        unityEvents.Add(EventName.GiveNewQuestionEvent, new GiveNewQuestionEvent() );
        EventManager.AddInvoker(EventName.GiveNewQuestionEvent, this);

    }

    /*// Update is called once per frame
	void Update () {	
	}*/

    /// <summary>
    /// Change the result after finishing a question
    /// </summary>
    /// <param name="level">level</param>
    /// <param name="score">score</param>
    void ChangeResultText(int level, int score, bool result)
    {
        this.result = result;

        string levelPrefix;
        string scorePrefix;

        //save for next quetion
        previousLevel = level;

        //change result page
        if (result)
        {
            resultImage.sprite = RightSprite;
            buttonNameText.text = "Next";
            levelPrefix = "Level ";
            scorePrefix = "Score: ";
            levelText.text = levelPrefix + level.ToString()+" Clear";
            SaveQuitButtonImage.enabled = true;
            
            AudioManager.PlaySE(SoundEffectClipName.CompletedSoundEffect);
        }
        else
        {
            resultImage.sprite = WrongSprite;
            buttonNameText.text = "Main Menu";
            levelPrefix = "Top Level: ";
            scorePrefix = "Final Score: ";
            levelText.text = levelPrefix + (level-1).ToString();
            SaveQuitButtonImage.enabled = false;

            AudioManager.PlaySE(SoundEffectClipName.FailTheGame);
        }
        //levelText.text = levelPrefix + level.ToString();
        scoreText.text = scorePrefix + score.ToString();

        //print("result: " + result + "\n L = " + level + ", S=" + score);
    }

    #region Public Methods
    /*/// <summary>
    /// handle the on click event from the Resume button
    /// </summary>
    public void HandleResumeButtonOnClickEvent()
    {
        //AudioManager.Play(AudioClipName.ButtonClick);

        //restart time
        //Time.timeScale = 1;
        //Destroy(gameObject);

        GetComponent<Canvas>().enabled = false;
        //Time.timeScale = 1;
    }*/

    /// <summary>
    /// handle the on click event from the Quit button
    /// </summary>
    public void HandleNextStepButtonOnClickEvent()
    {
        //restart time
        //Time.timeScale = 1;
        //Destroy(gameObject);
        GetComponent<Canvas>().enabled = false;
        //Time.timeScale = 1;

        AudioManager.PlaySE(SoundEffectClipName.ButtonClickSound);

        //print("Next Step Button Clicked");
        if (result)
        {
            // complete this game 
            unityEvents[EventName.GiveNewQuestionEvent].Invoke(previousLevel + 1);
        }
        else
        {
            //unityEvents[EventName.GiveNewQuestionEvent].Invoke(1);
            //fail this game
            MenuManager.GotoMenu(MenuName.MainMenu);
        }
    }

    
    /// <summary>
    /// Handles the on click event from the Save and Quit button
    /// </summary>
    public void HandleSaveQuitButtonOnClickEvent()
    {
        AudioManager.PlaySE(SoundEffectClipName.ButtonClickSound);

        // unpause game, destroy menu, and go to main menu
        //Destroy(gameObject);

        // save level and score

        MenuManager.GotoMenu(MenuName.MainMenu);
    }

    #endregion
}
