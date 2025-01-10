using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager Instance { get; private set; }

    private SceneName _sceneName = null;
    private TransitionData _transitionData = null;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        gameObject.SetActive(false);
    }

    public TransitionData GetTransitionData()
    {
        if (_transitionData != null)
        {
            return _transitionData;
        }

        Debug.LogWarning("TransitionDataがnullです。新しいインスタンスを返します。");
        return new TransitionData();
    }

    public void TransitionTo(SceneName sceneName, TransitionData transitionData = null)
    {
        if (_sceneName != null)
        {
            Debug.LogWarning($"現在シーン遷移中です（遷移元: {_sceneName}, 遷移先: {sceneName}）。TransitionToを呼び出すことはできません。");
            return;
        }

        _sceneName = sceneName;
        _transitionData = transitionData;
        gameObject.SetActive(true);
    }

    // AnimationEventで呼ばれるメソッド
    public void LoadScene()
    {
        if (_sceneName == null)
        {
            return;
        }

        SceneManager.LoadScene(_sceneName.ToString());
    }

    // AnimationEventで呼ばれるメソッド
    public void OnFinishTransition()
    {
        gameObject.SetActive(false);
        _sceneName = null;
        _transitionData = null;
    }
}
