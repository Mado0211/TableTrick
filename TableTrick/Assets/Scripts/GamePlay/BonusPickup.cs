using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Bonus Pickup
/// </summary>
public class BonusPickup : Pickup
{
    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();
        unityEvents.Add(EventName.BonusEvent, new BonusEvent());
        EventManager.AddInvoker(EventName.BonusEvent, this);
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
        //print("BonusPickup");
        unityEvents[EventName.BonusEvent].Invoke(0);
    }
}
