using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TableTrickManager : IntEventInvoker
{
    #region Field

    int gameLevel = 1;
    int totalScore = 0;
    int oneGameScore = 0;

    //support bonus
    const int BonusFactor = 2;
    bool isBonusTime;

    int highestScore;
    int highestLevel;

    SettingQuestion settingQuestionScript;
    //support mutiple cup contain gem
    int rightAnsCount = 0;
    bool result;

    //support how many cups back position
    int backCount = 0;

    //support show result page event
    ShowResultEvent showResultEvent = new ShowResultEvent();
    
    Timer showChoiceDelayTimer;

    //changing the State of Cup  event
    ChangeCupStateEvent changeCupStateEvent = new ChangeCupStateEvent();

    #endregion

    // Use this for initialization
    void Start () {
        EventManager.AddListener(EventName.CheckAnswerEvent, CheckAnswerEvent);
        EventManager.AddListener(EventName.WaitAllBackEvent, CheckAllBack);
        EventManager.AddShowingResultInvoker(this);
        EventManager.AddChangingCupStateInvoker(this);

        //for bonus event
        EventManager.AddListener(EventName.BonusEvent, HandleBonusPickupEvent);

        showChoiceDelayTimer = gameObject.AddComponent<Timer>();
        showChoiceDelayTimer.AddTimerFinishedEventListener(ShowResultPageEvent);
        showChoiceDelayTimer.Duration = 2.0f;

        
        gameLevel = PlayerPrefs.GetInt(GameDataName.NowLevel.ToString(), 1);
        totalScore = PlayerPrefs.GetInt(GameDataName.NowScore.ToString(), 0);
        highestScore = PlayerPrefs.GetInt(GameDataName.HighestScore.ToString(), 0);
        highestLevel = PlayerPrefs.GetInt(GameDataName.HighestLevel.ToString(), 0);

        settingQuestionScript = gameObject.GetComponent<SettingQuestion>(); 
        
        if(DifficultyUtilities.MoveMode == Difficulty.Curve)
        {
            EdgeCollider2D[] edges = gameObject.GetComponents<EdgeCollider2D>();
            foreach(EdgeCollider2D edge in edges)
            {
                edge.enabled = false;
            }
        }
        else
        {
            EdgeCollider2D[] edges = gameObject.GetComponents<EdgeCollider2D>();
            foreach (EdgeCollider2D edge in edges)
            {
                edge.enabled = true;
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        
    }
 
    /// <summary>
    /// Show Result Page
    /// </summary>
    void ShowResultPageEvent()
    {
        changeCupStateEvent.Invoke(CupState.ShowResult);
        MenuManager.GotoMenu(MenuName.ResultMenu);
        showResultEvent.Invoke(gameLevel, totalScore, result);

        if (result)
        {
            gameLevel++;
            //save game data
            PlayerPrefs.SetInt(GameDataName.NowLevel.ToString(), gameLevel);
            PlayerPrefs.SetInt(GameDataName.NowScore.ToString(), totalScore);

            //updata highest level
            print("clear level: " + (gameLevel-1));
            if (highestLevel < gameLevel - 1)
            {
                highestLevel = gameLevel - 1;
                PlayerPrefs.SetInt(GameDataName.HighestLevel.ToString(), highestLevel);
                print("Update level");
            }

            //updata highest Score
            print("now score: " + totalScore);
            if (highestScore < totalScore)
            {
                highestScore = totalScore;
                PlayerPrefs.SetInt(GameDataName.HighestScore.ToString(), highestScore);
                print("Update score");
            }
        }
        else
        {
            gameLevel = 1;
            totalScore = 0;
        }

        oneGameScore = 0;
    }

    /// <summary>
    /// checking all the question be answered when open the cup
    /// </summary>
    void CheckAnswerEvent(int gemNumber)
    {
        int choosedGemNumber = gemNumber;

        if (choosedGemNumber < 0)
        {
            print("CheckAnswerEvent err");
            return;
        }

        //print("Number of gems is " + gemNumber);
        //bool result = false;
        
        if (choosedGemNumber >= 1)
        {//right answer
            AudioManager.PlaySE(SoundEffectClipName.RightSoundEffect);

            int answerNumber = settingQuestionScript.AnswerNumber;
            rightAnsCount++;

            if (rightAnsCount == answerNumber)
            {//all answer be chose

                rightAnsCount = 0;

                //Update score
                UpdateScore();

                //show Result page after delay for showing the right answer
                result = true;
                totalScore += oneGameScore;// add the score of this game to totoal score

                changeCupStateEvent.Invoke(CupState.ShowResultDelay);
                showChoiceDelayTimer.Run();
                

                //this game finished, end the bonus time
                isBonusTime = false;
               
            }
            else
            {
                //Update score
                UpdateScore();

                changeCupStateEvent.Invoke(CupState.Choose_Ans);
            }
        }
        else
        {//wrong answer
            AudioManager.PlaySE(SoundEffectClipName.WrongSoundEffect);

            rightAnsCount = 0;

            //there aren't any gems in the cup
            //showResultEvent.Invoke(gameLevel, score, false);
            result = false;
            showChoiceDelayTimer.Run();
            changeCupStateEvent.Invoke(CupState.ShowResultDelay);

            //gameLevel = 1;
            //score = 0;
            PlayerPrefs.SetInt(GameDataName.NowLevel.ToString(), 1);
            PlayerPrefs.SetInt(GameDataName.NowScore.ToString(), 0);
        }
    }

    /// <summary>
    /// a method for Update Score
    /// </summary>
    void UpdateScore()
    {
        if (isBonusTime)
        {
            oneGameScore += DifficultyUtilities.Score * BonusFactor;
        }
        else
        {
            oneGameScore += DifficultyUtilities.Score;
        }
    }

    /// <summary>
    /// check if all cups back to the position
    /// </summary>
    /// <param name="unuse">unuse</param>
    void CheckAllBack(int unuse)
    {
        backCount++;
        if (backCount == settingQuestionScript.CupNumber)
        {
            backCount = 0;
            changeCupStateEvent.Invoke(CupState.Choose_Ans);
        }
    }

    /// <summary>
    /// Handle Bonus Pickup Event
    /// </summary>
    /// <param name="unuse">unuse</param>
    void HandleBonusPickupEvent(int unuse)
    {
        isBonusTime = true;
        //print("pick the bonus");
    }

    #region Public Methods

    /// <summary>
    /// Adds a handler to a showing result event 
    /// </summary>
    /// <param name="handler">handler</param>
    public void AddShowResultListener(UnityAction<int, int, bool> handler)
    {
        if (showResultEvent != null)
        {
            showResultEvent.AddListener(handler);
        }
    }

    /// <summary>
    /// Adds a handler to changing cup state event
    /// </summary>
    /// <param name="handler">handler</param>
    public void AddChangeCupStateListener(UnityAction<CupState> handler)
    {
        if (changeCupStateEvent != null)
        {
            changeCupStateEvent.AddListener(handler);
        }
    }

    

    #endregion
}
