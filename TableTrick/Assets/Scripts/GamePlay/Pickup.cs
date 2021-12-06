using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// a pick up for few effect
/// </summary>
public class Pickup : IntEventInvoker
{
    Timer deathTimer;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        deathTimer = gameObject.AddComponent<Timer>();
        deathTimer.Duration = DifficultyUtilities.PickupLifeTime;
        deathTimer.AddTimerFinishedEventListener(HandleDeathTimerFinishedEvent);
        deathTimer.Run();

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Handle Death Timer Finished Event
    /// </summary>
    void HandleDeathTimerFinishedEvent()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// Destroys the pick up when click
    /// </summary>
    virtual protected void OnMouseDown()
    {
        AudioManager.PlaySE(SoundEffectClipName.PickupSoundEffect);
        Destroy(gameObject);
    }
}
