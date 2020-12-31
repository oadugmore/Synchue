using UnityEngine;

public class SFX
{
    public static int deathFileID;

    const string DEATH_FILE_NAME = "Slash Down.wav";
    static bool firstTimeInitializationComplete = false;

    public static void Initialize()
    {
        if (firstTimeInitializationComplete) return;
        if (Application.platform == RuntimePlatform.Android)
        {
            LoadNativeAudio();
            Application.focusChanged += AndroidApplicationFocusChanged;
        }
        firstTimeInitializationComplete = true;
    }

    static void LoadNativeAudio()
    {
        AndroidNativeAudio.makePool();
        deathFileID = AndroidNativeAudio.load(DEATH_FILE_NAME);
    }

    static void UnloadNativeAudio()
    {
        AndroidNativeAudio.unload(deathFileID);
        AndroidNativeAudio.releasePool();
    }

    static void AndroidApplicationFocusChanged(bool focused)
    {
        if (focused)
        {
            LoadNativeAudio();
            Debug.Log("Loaded native audio");
        }
        else
        {
            UnloadNativeAudio();
            Debug.Log("Unloaded native audio");
        }
    }

    /// <summary>
    /// Plays the audio file specified by <paramref name="nativeFileID"/> if on Android, otherwise plays the AudioSource.
    /// Call <see cref="Initialize"/> first to avoid delays.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="nativeFileID"></param>
    public static void Play(AudioSource source, int nativeFileID)
    {
        if (!firstTimeInitializationComplete)
        {
            Debug.LogWarning("Tried playing a sound effect before initializing.");
            Initialize();
        }
        if (Application.platform == RuntimePlatform.Android)
        {
            var streamId = AndroidNativeAudio.play(nativeFileID);
            Debug.Assert(streamId != -1);
        }
        else
        {
            source.Play();
        }
    }
}