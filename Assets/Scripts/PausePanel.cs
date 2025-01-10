using UnityEngine;
using UnityEngine.UI;

public class PausePanel : MonoBehaviour
{
    [SerializeField] private GameObject _pauseButton;
    [SerializeField] private GameObject _settingsButton;
    [SerializeField] private Slider _movementSensitivitySlider;
    [SerializeField] private Slider _rotationSensitivitySlider;

    [Header("UI Groups")]
    [SerializeField] private GameObject _pauseMenuGroup; 
    [SerializeField] private GameObject _sensitivitySettingsGroup; 

    public void Open()
    {
        SoundManager.Instance.PlaySE("se_pause_open");
        gameObject.SetActive(true);
        _pauseButton.SetActive(false);
        Time.timeScale = 0;
    }

    public void Close()
    {
        SoundManager.Instance.PlaySE("se_pause_close");
        gameObject.SetActive(false);
        _pauseButton.SetActive(true);
        Time.timeScale = 1;
    }

    public void EnableGyroSettings(GyroRodInputHandler[] gyroInputHandlers)
    {
        _settingsButton.SetActive(true);

        _movementSensitivitySlider.onValueChanged.AddListener((value) =>
        {
            foreach (var handler in gyroInputHandlers)
            {
                handler.SetMovementSensitivity(value);
            }
        });

        _rotationSensitivitySlider.onValueChanged.AddListener((value) =>
        {
            foreach (var handler in gyroInputHandlers)
            {
                handler.SetRotationSensitivity(value);
            }
        });
    }

    public void OnClickSettings()
    {
        SoundManager.Instance.PlaySE("se_click");
        _pauseMenuGroup.SetActive(false);
        _sensitivitySettingsGroup.SetActive(true);
    }

    public void OnClickConfirm()
    {
        SoundManager.Instance.PlaySE("se_click");
        _pauseMenuGroup.SetActive(true);
        _sensitivitySettingsGroup.SetActive(false);
    }

    public void OnClickExit()
    {
        SoundManager.Instance.PlaySE("se_click");
        Time.timeScale = 1;
        TransitionManager.Instance.TransitionTo("Menu");
    }
}
