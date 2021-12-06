using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Distraction Pickup
/// </summary>
public class DistractionPickup : Pickup
{
    /// <summary>
    /// Destroys the pick up when click
    /// </summary>
    override protected void OnMouseDown()
    {
        base.OnMouseDown();
        //print("Nothing happened");
    }
}
