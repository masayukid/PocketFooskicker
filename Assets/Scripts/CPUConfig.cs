using UnityEngine;

[CreateAssetMenu(fileName = "CPUConfig", menuName = "Game/CPUConfig")]
public class CPUConfig : ScriptableObject
{
    [System.Serializable]
    public class ModeSettings
    {
        public string modeName;     // モード名 (例: Easy, Normal, Hard)
        public float moveSpeed;     // 横移動速度
        public float rotationSpeed; // 回転速度
    }

    [Header("CPU Modes Settings")]
    public ModeSettings[] modeSettings;

    public ModeSettings GetSettingsByName(string modeName)
    {
        foreach (var settings in modeSettings)
        {
            if (settings.modeName == modeName)
                return settings;
        }
        
        return modeSettings.Length > 0 ? modeSettings[0] : null;
    }
}
