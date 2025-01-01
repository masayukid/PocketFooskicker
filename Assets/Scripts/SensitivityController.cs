using UnityEngine;
using UnityEngine.UI;

public class SensitivityController : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Slider movementSlider;
    [SerializeField] private Slider rotationSlider;

    [Header("Sensitivity Values")]
    public float movementSensitivity = 1.0f;
    public float rotationSensitivity = 300f;

    private void Start()
    {
        // 初期値をUIに反映
        movementSlider.value = movementSensitivity;
        rotationSlider.value = rotationSensitivity;

        // UIイベントリスナーを登録
        movementSlider.onValueChanged.AddListener(OnMovementSliderChanged);
        rotationSlider.onValueChanged.AddListener(OnRotationSliderChanged);
    }

    private void OnDestroy()
    {
        // イベントリスナーを解除
        movementSlider.onValueChanged.RemoveListener(OnMovementSliderChanged);
        rotationSlider.onValueChanged.RemoveListener(OnRotationSliderChanged);
    }

    // スライダー操作時に感度を更新
    private void OnMovementSliderChanged(float value)
    {
        movementSensitivity = value;
        UpdateRodSensitivity();
    }

    private void OnRotationSliderChanged(float value)
    {
        rotationSensitivity = value;
        UpdateRodSensitivity();
    }

    // 感度をロッドに反映
    private void UpdateRodSensitivity()
    {
        foreach (var rod in FindObjectsOfType<RodController>())
        {
            rod.SetSensitivity(movementSensitivity, rotationSensitivity);
        }
    }
}