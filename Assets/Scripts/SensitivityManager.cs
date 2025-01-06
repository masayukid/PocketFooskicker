using UnityEngine;
using UnityEngine.UI;

public class SensitivityManager : MonoBehaviour
{
    [SerializeField] private Slider _movementSlider;
    [SerializeField] private Slider _rotationSlider;
    [SerializeField] private GameObject _sensitivityButton;
    private GyroRodInputHandler _gyroInputHandler;

    public void SetGyroInputHandler(GyroRodInputHandler gyroInputHandler)
    {
        _gyroInputHandler = gyroInputHandler;

        if (_gyroInputHandler == null)
        {
            _sensitivityButton.SetActive(false);
        }
    }

    public void OnMovementSliderChanged()
    {
        if (_gyroInputHandler != null)
        {
            _gyroInputHandler.SetSensitivity(_movementSlider.value, _gyroInputHandler.RotationSensitivity);
        }
    }

    public void OnRotationSliderChanged()
    {
        if (_gyroInputHandler != null)
        {
            _gyroInputHandler.SetSensitivity(_gyroInputHandler.MovementSensitivity, _rotationSlider.value);
        }
    }
}