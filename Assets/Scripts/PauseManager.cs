using UnityEngine;
using UnityEngine.SceneManagement; // シーン遷移用

public class PauseManager : MonoBehaviour
{
    public GameObject pauseCanvas; // PauseCanvasをアタッチ
    public GameObject pauseButton; // PauseButtonをアタッチ

    [Header("UI Groups")]
    [SerializeField] private GameObject pauseMenuGroup;  // PauseMenuGroup (ボタンたち)
    [SerializeField] private GameObject sensitivitySettingsGroup; // SensitivitySettingsGroup (感度設定)


    // 一時停止
    public void PauseGame()
    {
        pauseCanvas.SetActive(true);
        sensitivitySettingsGroup.SetActive(false);
        pauseButton.SetActive(false);
        Time.timeScale = 0;
    }

    // 再開
    public void ResumeGame()
    {
        pauseCanvas.SetActive(false);
        pauseButton.SetActive(true);
        Time.timeScale = 1;
    }

    // 操作変更
    public void ChangeControls()
    {
        Debug.Log("操作方法を変更しました！");
    }

    // メニュー画面に戻る
    public void GoToMainMenu()
    {
        Time.timeScale = 1; // 時間を再開
        SceneManager.LoadScene("Menu"); // メニュー画面のシーン名を指定
    }

    public void ShowSensitivitySettings()
    {
        // 感度設定を表示し、その他を非表示にする
        pauseMenuGroup.SetActive(false);
        sensitivitySettingsGroup.SetActive(true);
    }

    public void ShowPauseMenu()
    {
        // 感度設定を非表示にし、PauseMenuを表示する
        pauseMenuGroup.SetActive(true);
        sensitivitySettingsGroup.SetActive(false);
    }
}
