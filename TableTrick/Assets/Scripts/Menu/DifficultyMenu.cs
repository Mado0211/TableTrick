using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Listens for the OnClick events for the difficulty menu buttons
/// </summary>
public class DifficultyMenu : IntEventInvoker
{
    /// <summary>
    /// Use this for initialization
    /// </summary>
    void Start()
    {
        // add event component and add invoker to event manager
        unityEvents.Add(EventName.GameStartedEvent, new GameStartedEvent());
        EventManager.AddInvoker(EventName.GameStartedEvent, this);
    }

    /// <summary>
    /// Handles the on click event from the easy button
    /// </summary>
    public void HandleEasyButtonOnClickEvent()
    {
        AudioManager.PlaySE(SoundEffectClipName.ButtonClickSound);
        unityEvents[EventName.GameStartedEvent].Invoke((int)Difficulty.Normal);
        //Debug.Log("EasyButton");
    }


    /// <summary>
    /// Handles the on click event from the hard button
    /// </summary>
    public void HandleHardButtonOnClickEvent()
    {
        AudioManager.PlaySE(SoundEffectClipName.ButtonClickSound);
        unityEvents[EventName.GameStartedEvent].Invoke((int)Difficulty.Curve);
        //Debug.Log("HardButton");
    }

}
