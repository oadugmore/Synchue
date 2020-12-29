using UnityEngine;
using UnityCoreHaptics;

public class MobileUtils
{
    public const float MIN_CONTINUOUS_CORE_HAPTICS_DURATION = 0.1f;

    private static AndroidJavaObject androidVibrator = null;
    private static AndroidJavaClass vibrationEffectClass = null;
    private static bool vibratorInitialized = false;
    private static bool androidRequiresLegacyVibratorApi = false;
    private static bool iPhoneSupportsCoreHaptics = false;
    private static bool hasVibrator = false;

    /// <summary>
    /// Determines the Android API version of the device.
    /// </summary>
    /// <returns>The current API version if running on Android, or -1 otherwise.</returns>
    public static int GetAndroidApiVersion()
    {
        var version = -1;
        if (Application.platform == RuntimePlatform.Android)
        {
            using (AndroidJavaClass androidVersionClass = new AndroidJavaClass("android.os.Build$VERSION"))
            {
                version = androidVersionClass.GetStatic<int>("SDK_INT");
            }
        }
        return version;
    }

    /// <summary>
    /// Initializes the device vibrator. Call this before attempting to call <see cref="Vibrate"/> to avoid delays.
    /// </summary>
    public static void InitializeVibrator()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (GetAndroidApiVersion() < 26)
            {
                androidRequiresLegacyVibratorApi = true;
            }

            using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            using (AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                if (currentActivity != null)
                {
                    androidVibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
                    hasVibrator = androidVibrator != null && androidVibrator.Call<bool>("hasVibrator");
                    if (!androidRequiresLegacyVibratorApi)
                    {
                        vibrationEffectClass = new AndroidJavaClass("android.os.VibrationEffect");
                    }
                }
            }
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            // Leave as true in case a future iPad supports vibration,
            // for now the Core Haptics check is sufficient.
            // "Not checking" appears to be the iConvention anyway
            hasVibrator = true;

            if (UnityCoreHapticsProxy.SupportsCoreHaptics())
            {
                iPhoneSupportsCoreHaptics = true;
                UnityCoreHapticsProxy.OnEngineCreated += () => { Debug.Log("Created engine"); };
                UnityCoreHapticsProxy.OnEngineError += () => { Debug.LogError("UnityCoreHaptics Engine had an error."); };
                UnityCoreHapticsProxy.CreateEngine();
            }
        }
        vibratorInitialized = true;
    }

    /// <summary>
    /// Vibrates the device, if vibration is supported. Intensity is ignored on Android API before 26,
    /// and all parameters are ignored on iOS devices without Core Haptics. Sharpness only affects devices with Core Haptics.
    /// </summary>
    /// <param name="duration">The duration in seconds. Values less than <see cref="MIN_CONTINUOUS_CORE_HAPTICS_DURATION"/>
    /// result in a transient haptic on devices with Core Haptics.</param>
    /// <param name="intensity">The intensity as a percent.</param>
    /// <param name="sharpness">The sharpness as a percent. </param>
    public static void Vibrate(float duration, float intensity, float sharpness)
    {
        if (!hasVibrator) return;
        if (!vibratorInitialized)
        {
            Debug.LogWarning("Vibrate was called without first initializing the vibrator.");
            InitializeVibrator();
        }
        intensity = Mathf.Clamp01(intensity);
        if (Application.platform == RuntimePlatform.Android)
        {
            VibrateAndroid((long)(duration * 1000), (int)(intensity * 255));
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            VibrateiPhone(duration, intensity, sharpness);
        }
        else
        {
            Handheld.Vibrate();
        }
    }

    private static void VibrateiPhone(float duration, float intensity, float sharpness)
    {
        if (iPhoneSupportsCoreHaptics)
        {
            if (duration > MIN_CONTINUOUS_CORE_HAPTICS_DURATION)
            {
                UnityCoreHapticsProxy.PlayContinuousHaptics(intensity, sharpness, duration);
            }
            else
            {
                UnityCoreHapticsProxy.PlayTransientHaptics(intensity, sharpness);
            }
        }
        else
        {
            Handheld.Vibrate();
        }
    }

    private static void VibrateAndroid(long milliseconds, int amplitude)
    {
        if (androidRequiresLegacyVibratorApi)
        {
            androidVibrator.Call("vibrate", milliseconds);
        }
        else
        {
            using (AndroidJavaObject vibrationEffect = vibrationEffectClass.CallStatic<AndroidJavaObject>("createOneShot", milliseconds, amplitude))
            {
                androidVibrator.Call("vibrate", vibrationEffect);
            }
        }
    }
}
