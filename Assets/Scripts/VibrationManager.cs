using UnityEngine;

public static class VibrationManager
{
    public static void ShortVibration()
    {
        if (SystemInfo.supportsVibration)
        {
            Vibrate(3);
        }
    }

    // Android設定
#if UNITY_ANDROID
    public static AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
    public static AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
    public static AndroidJavaObject vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
#endif

    private static void Vibrate(long milliseconds)
    {
#if UNITY_ANDROID
            vibrator.Call("vibrate", milliseconds);
#endif
        if (milliseconds >= 1000)
        {
            Handheld.Vibrate();
        }
    }
}