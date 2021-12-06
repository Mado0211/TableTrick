using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Provides difficulty-specific utilities
/// </summary>
public static class DifficultyUtilities
{
    #region Fields

    static Difficulty difficulty;

    #endregion

    #region Properties

    /// <summary>
    /// gets the current game Difficulty
    /// </summary>
    public static Difficulty MoveMode
    {
        get
        {
            return difficulty;
        }
    }


    /// <summary>
    /// the duration of cup moving
    /// </summary>
    public static float MovingDuration
    {
        get
        {
            switch (difficulty)
            {
                case Difficulty.Normal:
                    return 3.0f;
                case Difficulty.Curve:
                    return 5.0f;
                default:
                    Debug.Log("MovingDuration seting default");
                    return 3.0f;
            }
        }
    }

    /// <summary>
    /// Pickup Life Time
    /// </summary>
    public static float PickupLifeTime
    {
        get
        {
            switch (difficulty)
            {
                case Difficulty.Normal:
                    return 1.5f;
                case Difficulty.Curve:
                    return 1.8f;
                default:
                    Debug.Log("MovingDuration seting default");
                    return 3.0f;
            }
        }
    }

    #region Pickup Probability

    /// <summary>
    /// a probability of bonus pickup appearing in the scene
    /// </summary>
    public static int BonusProbability
    {
        get
        {
            switch (difficulty)
            {
                case Difficulty.Normal:
                    return 25;
                case Difficulty.Curve:
                    return 15;
                default:
                    Debug.Log("MovingDuration seting default");
                    return 33;
            }
        }
    }

    /// <summary>
    /// a probability of Speed Down pickup appearing in the scene
    /// </summary>
    public static int SpeedDownProbability
    {
        get
        {
            switch (difficulty)
            {
                case Difficulty.Normal:
                    return 25;
                case Difficulty.Curve:
                    return 15;
                default:
                    Debug.Log("MovingDuration seting default");
                    return 33;
            }
        }
    }

    /// <summary>
    /// a probability of distraction pickup appearing in the scene
    /// </summary>
    public static int DistractionProbability
    {
        get
        {
            switch (difficulty)
            {
                case Difficulty.Normal:
                    return 50;
                case Difficulty.Curve:
                    return 70;
                default:
                    Debug.Log("MovingDuration seting default");
                    return 33;
            }
        }
    }
    #endregion

    /// <summary>
    ///  gets the score of the game
    /// </summary>
    public static int Score
    {
        get
        {
            switch (difficulty)
            {
                case Difficulty.Normal:
                    return 10;
                case Difficulty.Curve:
                    return 30;
                default:
                    Debug.Log("Score seting default");
                    return 10;
            }
        }
    }

    /// <summary>
    /// gets the minimun speed of moving
    /// </summary>
    public static float MinSpeed
    {
        get
        {
            switch (difficulty)
            {
                case Difficulty.Normal:
                    return 5.0f;
                case Difficulty.Curve:
                    return 7.5f;
                default:
                    Debug.Log("Score seting default");
                    return 10;
            }
        }
    }

    /// <summary>
    /// gets the maxmum speed of moving
    /// </summary>
    public static float MaxSpeed
    {
        get
        {
            switch (difficulty)
            {
                case Difficulty.Normal:
                    return 8.0f;
                case Difficulty.Curve:
                    return 11.0f;
                default:
                    Debug.Log("Score seting default");
                    return 10;
            }
        }
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Initialize the difficulty utilities
    /// </summary>
    public static void Initialize()
    {
        EventManager.AddListener(EventName.GameStartedEvent, HandleGameStartedEvent);
    }

    #endregion

    #region Private Methods

    /// <summary>
	/// Sets the difficulty and starts the game
	/// </summary>
	/// <param name="intDifficulty">int value for difficulty</param>
    static void HandleGameStartedEvent(int intDifficulty)
    {
        difficulty = (Difficulty)intDifficulty;
        SceneManager.LoadScene("GamePlay");
    }

    #endregion
}
