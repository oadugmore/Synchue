using UnityEngine;

public class MobileUtils
{
    private static AndroidJavaObject androidVibrator = null;
    private static AndroidJavaClass vibrationEffectClass = null;
    private static bool initialized = false;
    private static int androidApiVersion = 1;
    private static bool androidRequiresLegacyApi = false;
    private static bool hasVibrator = false;

    public static void InitializeVibrator()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            using (AndroidJavaClass androidVersionClass = new AndroidJavaClass("android.os.Build$VERSION"))
            {
                androidApiVersion = androidVersionClass.GetStatic<int>("SDK_INT");
                if (androidApiVersion < 26)
                {
                    androidRequiresLegacyApi = true;
                }
            }
            using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            using (AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                if (currentActivity != null)
                {
                    androidVibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
                    hasVibrator = androidVibrator != null && androidVibrator.Call<bool>("hasVibrator");
                    if (!androidRequiresLegacyApi)
                    {
                        vibrationEffectClass = new AndroidJavaClass("android.os.VibrationEffect");
                    }
                }
            }
        }
        initialized = true;
    }

    /// <summary>
    /// Vibrates the device, if vibration is supported. Vibration intensity is ignored on Android API < 26.
    /// </summary>
    /// <param name="duration">The duration in seconds.</param>
    /// <param name="intensity">The intensity as a percent.</param>
    public static void Vibrate(float duration, float intensity)
    {
        if (!hasVibrator) return;
        if (!initialized) InitializeVibrator();
        intensity = Mathf.Clamp01(intensity);
        if (Application.platform == RuntimePlatform.Android)
        {
            VibrateAndroid((long)(duration * 1000), (int)(intensity * 255));
        }
        else
        {
            Handheld.Vibrate();
        }
    }

    private static void VibrateAndroid(long milliseconds, int amplitude)
    {
        if (androidRequiresLegacyApi)
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
