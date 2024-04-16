using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vibration
{
    private const string HapticKey = "Haptics";

    public static bool IsHaptics
    {
        get => PlayerPrefs.GetInt(HapticKey, 0) == 1;
        set => PlayerPrefs.SetInt(HapticKey, value ? 1 : 0);
    }

    public static AndroidJavaClass unityPlayer;

    public static AndroidJavaObject currentActivity;

    public static AndroidJavaObject vibrator;

    static Vibration()
    {
#if !UNITY_EDITOR && UNITY_ANDROID
        unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", new object[1]
        {
            "vibrator"
        });
#endif
    }

    public static void Vibrate()
    {
        if (!IsHaptics)
            return;

        if (IsAndroidPlatform())
        {
            vibrator.Call("vibrate");
        }
        else
        {
            Handheld.Vibrate();
        }
    }

    public static void Vibrate(long milliseconds)
    {
        if (!IsHaptics)
            return;

        if (IsAndroidPlatform())
        {
            vibrator.Call("vibrate", milliseconds);
        }
    }

    public static void Vibrate(long[] pattern, int repeat)
    {
        if (!IsHaptics)
            return;

        if (IsAndroidPlatform())
        {
            vibrator.Call("vibrate", pattern, repeat);
        }
    }

    public static bool HasVibrator()
    {
        return IsAndroidPlatform();
    }

    public static void Cancel()
    {
        if (IsAndroidPlatform())
        {
            vibrator.Call("cancel");
        }
    }

    private static bool IsAndroidPlatform()
	{
#if UNITY_EDITOR
        return false;
#elif UNITY_ANDROID
        return true;
#endif
    }

}
