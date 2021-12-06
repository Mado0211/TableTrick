using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Speed Down Pickup
/// </summary>
public class SpeedDownPickup : Pickup
{
    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();
        unityEvents.Add(EventName.SpeedDownEvent, new SpeedDownEvent());
        EventManager.AddInvoker(EventName.SpeedDownEvent, this);

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Destroys the pick up when click
    /// </summary>
    override protected void OnMouseDown()
    {
        base.OnMouseDown();
        unityEvents[EventName.SpeedDownEvent].Invoke(0);
        //print("SpeedDownPickup");
    }
}
