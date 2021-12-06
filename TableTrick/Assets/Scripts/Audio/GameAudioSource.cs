using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// an audio source for the entire game
/// </summary>
public class GameAudioSource : MonoBehaviour
{
    /// <summary>
    /// Awake is called before Start
    /// </summary>
    private void Awake()
    {
        //make sure we only have one of this GameObejct for BGM in the game
        if( !AudioManager.InitializedBGM)
        {
            // initialize audio manager for BGM and persist audio source across scenes
            AudioSource BGMSource = gameObject.AddComponent<AudioSource>();
            BGMSource.loop = true;
            AudioManager.InitializeBGM(BGMSource);
            AudioManager.PlayBGM(BGMClipName.DungeonsDragons_NatureElemental);

            DontDestroyOnLoad(BGMSource);
        }
        else
        {
            // duplicate game object, so destroy
            Destroy(gameObject);
        }

        //make sure we only have one of this GameObejct for SE in the game
        if (!AudioManager.InitializedSE)
        {
            // initialize audio manager for SE and persist audio source across scenes
            AudioSource soundEffectSource = gameObject.AddComponent<AudioSource>();
            AudioManager.InitializeSE(soundEffectSource);

            DontDestroyOnLoad(soundEffectSource);
        }
        else
        {
            // duplicate game object, so destroy
            Destroy(gameObject);
        }
    }
}
