using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// the audio manager
/// </summary>
public static class AudioManager 
{
    #region Fields

    static bool initializedSE = false;
    static bool initializedBGM = false;
    static AudioSource audioSource;
    static AudioSource BGMSource;

    static Dictionary<SoundEffectClipName, AudioClip> soundEffectClip = new Dictionary<SoundEffectClipName, AudioClip>();
    static Dictionary<BGMClipName, AudioClip> backgroundMusicClip = new Dictionary<BGMClipName, AudioClip>();
    #endregion

    #region Properties

    /// <summary>
    /// Gets whether or not the SE audio manager has been initialized
    /// </summary>
    public static bool InitializedSE
    {
        get { return initializedSE; }
    }

    /// <summary>
    /// Gets whether or not the BGM audio manager has been initialized
    /// </summary>
    public static bool InitializedBGM
    {
        get { return initializedBGM; }
    }
    #endregion

    #region Public Methodes

    /// <summary>
    /// initializes the Sound Effect audio manager
    /// </summary>
    /// <param name="source">Audio Source</param>
    public static void InitializeSE(AudioSource source)
    {
        initializedSE = true;
        audioSource = source;

        //load sound effect clip
        foreach (SoundEffectClipName clipName in Enum.GetValues(typeof(SoundEffectClipName)))
        {
            soundEffectClip.Add(clipName,
                Resources.Load<AudioClip>(clipName.ToString()));
        }
    }

    /// <summary>
    /// plays the sound effect with the given name
    /// </summary>
    /// <param name="clipName">name of the sound effect </param>
    public static void PlaySE(SoundEffectClipName clipName)
    {
        audioSource.PlayOneShot(soundEffectClip[clipName]);
    }

    /// <summary>
    /// initializes the BGM audio manager
    /// </summary>
    /// <param name="source">BGM Audio Source</param>
    public static void InitializeBGM(AudioSource source)
    {
        initializedBGM = true;
        BGMSource = source;

        // load background music clip 
        foreach(BGMClipName musicName in Enum.GetValues(typeof(BGMClipName)))
        {
            backgroundMusicClip.Add(musicName,
                Resources.Load<AudioClip>(musicName.ToString()));
        }
    }

    /// <summary>
    /// play the background music with the given name
    /// </summary>
    /// <param name="clipName">the name of BGM</param>
    public static void PlayBGM(BGMClipName clipName)
    {
        BGMSource.clip = backgroundMusicClip[clipName];
        BGMSource.Play();
    }
    #endregion
}
