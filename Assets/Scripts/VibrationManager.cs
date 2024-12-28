using UnityEngine;

public static class VibrationManager
{
    public static void ShortVibration()
    {
        if (SystemInfo.supportsVibration)
        {
            PlaySystemSound(1519);
            Vibrate(3);
        }
    }

    // iOS設定
#if UNITY_IOS && !UNITY_EDITOR
    [DllImport ("__Internal")]
    private static extern void _playSystemSound(int n);
#endif

    private static void PlaySystemSound(int n)
    {
#if UNITY_IOS && !UNITY_EDITOR
            _playSystemSound(n);
#endif
    }

    // Android設定
#if UNITY_ANDROID && !UNITY_EDITOR
    public static AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
    public static AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
    public static AndroidJavaObject vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
#endif

    private static void Vibrate(long milliseconds)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
            vibrator.Call("vibrate", milliseconds);
#endif
        if (milliseconds >= 1000)
        {
            Handheld.Vibrate();
        }
    }
}