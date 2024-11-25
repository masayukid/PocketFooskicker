using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "CPUConfig", menuName = "Game/CPUConfig")]
public class CPUConfig : ScriptableObject
{
    [Serializable]
    public class ModeSettings
    {
        [SerializeField] private CPUMode _mode;         // モード (例: Easy, Normal, Hard)
        [SerializeField] private float _moveSpeed;      // 横移動速度
        [SerializeField] private float _rotationSpeed;  // 回転速度

        public CPUMode Mode => _mode;
        public float MoveSpeed => _moveSpeed;
        public float RotationSpeed => _rotationSpeed;
    }

    [Header("CPU Modes Settings")]
    [SerializeField] private ModeSettings[] _modeSettings;

    public ModeSettings GetSettingsByMode(CPUMode mode)
    {
        return _modeSettings.Single(settings => settings.Mode == mode);
    }
}
