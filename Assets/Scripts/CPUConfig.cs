using UnityEngine;

[CreateAssetMenu(fileName = "CPUConfig", menuName = "Game/CPUConfig")]
public class CPUConfig : ScriptableObject
{
    [System.Serializable]
    public class ModeSettings
    {
        [SerializeField] private CPUMode _mode;     // モード名 (例: Easy, Normal, Hard)
        [SerializeField] private float _moveSpeed;     // 横移動速度
        [SerializeField] private float _rotationSpeed; // 回転速度

        public CPUMode Mode => _mode;
        public float MoveSpeed => _moveSpeed;
        public float RotationSpeed => _rotationSpeed;
    }

    [Header("CPU Modes Settings")]
    [SerializeField] private ModeSettings[] _modeSettings;

    public ModeSettings GetSettingsByName(CPUMode mode)
    {
        foreach (var settings in _modeSettings)
        {
            if (settings.Mode == mode)
                return settings;
        }
        
        return _modeSettings.Length > 0 ? _modeSettings[0] : null;
    }
}
