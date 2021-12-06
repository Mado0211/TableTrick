using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Initializes the game  when load game scene
/// </summary>
public class GameInitializer : MonoBehaviour
{
    /// <summary>
    /// Awake is called before Start
    /// </summary>
	void Awake()
    {
        // initialize screen utils
        ScreenUtils.Initialize();

        // initialize configuration utils
        //ConfigurationUtils.Initialize();

        // initialize effect utils
        //EffectUtils.Initialize();

        // initializes event manager
        EventManager.Initialize();

        // initializes Difficulty Utilities
        DifficultyUtilities.Initialize();

        //PlayerPrefs.DeleteAll();
        DLGCurve.Initailize();
    }
}
