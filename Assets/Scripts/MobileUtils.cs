using UnityEngine;
using UnityCoreHaptics;

public class MobileUtils
{
    private static AndroidJavaObject androidVibrator = null;
    private static AndroidJavaClass vibrationEffectClass = null;
    private static bool vibratorInitialized = false;
    private static bool androidRequiresLegacyVibratorApi = false;
    private static bool iPhoneSupportsCoreHaptics = false;
    private static bool hasVibrator = false;

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
    /// Vibrates the device, if vibration is supported. Vibration intensity is ignored on Android API < 26
    /// and on iOS devices without Core Haptics.
    /// </summary>
    /// <param name="duration">The duration in seconds. Setting to 0 results in a transient haptic on devices with Core Haptics.</param>
    /// <param name="intensity">The intensity as a percent.</param>
    /// <param name="sharpness">The sharpness as a percent. Only affects devices with Core Haptics.</param>
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
            if (duration > 0)
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
