using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject _pauseCanvas; 
    [SerializeField] private GameObject _pauseButton; 

    [Header("UI Groups")]
    [SerializeField] private GameObject _pauseMenuGroup; 
    [SerializeField] private GameObject _sensitivitySettingsGroup; 

    public void PauseGame()
    {
        _pauseCanvas.SetActive(true);
        _sensitivitySettingsGroup.SetActive(false);
        _pauseButton.SetActive(false);
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        _pauseCanvas.SetActive(false);
        _pauseButton.SetActive(true);
        Time.timeScale = 1;
    }

    public void ChangeControls()
    {
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1;
        TransitionManager.Instance.TransitionTo("Menu"); 
    }

    public void ShowSensitivitySettings()
    {
        _pauseMenuGroup.SetActive(false);
        _sensitivitySettingsGroup.SetActive(true);
    }

    public void ShowPauseMenu()
    {
        _pauseMenuGroup.SetActive(true);
        _sensitivitySettingsGroup.SetActive(false);
    }
}
