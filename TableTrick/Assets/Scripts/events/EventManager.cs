using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Manages connections between event listeners and event invokers
/// </summary>
public static class EventManager
{
	#region Fields

	static Dictionary<EventName, List<IntEventInvoker>> invokers = 
		new Dictionary<EventName, List<IntEventInvoker>>();
	static Dictionary<EventName, List<UnityAction<int>>> listeners =
		new Dictionary<EventName, List<UnityAction<int>>>();

    //support showing result page
    static List<TableTrickManager> showResultInvokers = new List<TableTrickManager>();
    static List<UnityAction<int, int, bool>> showResultListeners = new List<UnityAction<int, int, bool>>();

    //support change cup state event
    static List<TableTrickManager> changeCupStateInvokers = new List<TableTrickManager>();
    static List<UnityAction<CupState>> changeCupStateListeners = new List<UnityAction<CupState>>();

    #endregion

    #region Public methods

    /// <summary>
    /// Initializes the event manager
    /// </summary>
    public static void Initialize()
    {
		// create empty lists for all the dictionary entries
		foreach (EventName name in Enum.GetValues(typeof(EventName)))
        {
			if (!invokers.ContainsKey(name))
            {
				invokers.Add(name, new List<IntEventInvoker>());
				listeners.Add(name, new List<UnityAction<int>>());
			}
            else
            {
				invokers[name].Clear();
				listeners[name].Clear();
			}
		}

        //clear speedup effect invoker list 
        showResultInvokers.Clear();
    }
		
	/// <summary>
	/// Adds the given invoker for the given event name
	/// </summary>
	/// <param name="eventName">event name</param>
	/// <param name="invoker">invoker</param>
	public static void AddInvoker(EventName eventName, IntEventInvoker invoker)
    {
		// add listeners to new invoker and add new invoker to dictionary
		foreach (UnityAction<int> listener in listeners[eventName])
        {
			invoker.AddListener(eventName, listener);
		}
		invokers [eventName].Add(invoker);
	}

	/// <summary>
	/// Adds the given listener for the given event name
	/// </summary>
	/// <param name="eventName">event name</param>
	/// <param name="listener">listener</param>
	public static void AddListener(EventName eventName, UnityAction<int> listener)
    {
		// add as listener to all invokers and add new listener to dictionary
		foreach (IntEventInvoker invoker in invokers[eventName])
        {
			invoker.AddListener(eventName, listener);
		}
		listeners [eventName].Add(listener);
	}

	/// <summary>
	/// Removes the given invoker for the given event name
	/// </summary>
	/// <param name="eventName">event name</param>
	/// <param name="invoker">invoker</param>
	public static void RemoveInvoker(EventName eventName, IntEventInvoker invoker)
    {
		// remove invoker from dictionary
		invokers [eventName].Remove(invoker);
	}

    #endregion


    #region Show result event


    /// <summary>
    /// adds given script as invoker for  showing result event
    /// </summary>
    /// <param name="invoker">invoker</param>
    static public void AddShowingResultInvoker(TableTrickManager invoker)
    {
        showResultInvokers.Add(invoker);

        //add speed up listener
        foreach (UnityAction<int, int, bool> listener in showResultListeners)
        {
            invoker.AddShowResultListener(listener);
        }
    }

    /// <summary>
    /// Removes the given invoker for  showing result event
    /// </summary>
    /// <param name="invoker"></param>
    static public void RemoveShowingResultInvoker(TableTrickManager invoker)
    {
        showResultInvokers.Remove(invoker);
    }

    /// <summary>
    /// add given hander as showing result event listener for  showing result event
    /// </summary>
    /// <param name="handler"></param>
    static public void AddShowingResultListener(UnityAction<int, int, bool> handler)
    {
        showResultListeners.Add(handler);
        foreach (TableTrickManager gameManager in showResultInvokers)
        {
            gameManager.AddShowResultListener(handler);
        }
    }

    #endregion

    #region change the State of Cup  event


    /// <summary>
    /// adds given script as invoker for changing the State of Cup  event
    /// </summary>
    /// <param name="invoker">invoker</param>
    static public void AddChangingCupStateInvoker(TableTrickManager invoker)
    {
        changeCupStateInvokers.Add(invoker);

        //add speed up listener
        foreach (UnityAction<CupState> listener in changeCupStateListeners)
        {
            invoker.AddChangeCupStateListener(listener);
        }
    }

    /// <summary>
    /// Removes the given invoker for changing the State of Cup  event
    /// </summary>
    /// <param name="invoker"></param>
    static public void RemoveChangingCupStateInvoker(TableTrickManager invoker)
    {
        changeCupStateInvokers.Remove(invoker);
    }

    /// <summary>
    /// add given hander as a changing the State of Cup  event listener for changing the State of Cup  event
    /// </summary>
    /// <param name="handler"></param>
    static public void AddChangingCupStateListener(UnityAction<CupState> handler)
    {
        changeCupStateListeners.Add(handler);
        foreach (TableTrickManager invoker in changeCupStateInvokers)
        {
            invoker.AddChangeCupStateListener(handler);
        }
    }

    #endregion 
}
