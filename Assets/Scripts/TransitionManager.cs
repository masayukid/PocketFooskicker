using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager Instance { get; private set; }

    private string _sceneName = null;
    private Dictionary<string, object> _transitionData = null;

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

    public T GetDataOrDefault<T>(string key, T defaultValue)
    {
        if (_transitionData != null && _transitionData.TryGetValue(key, out var value))
        {
            if (value is T typedValue)
            {
                return typedValue;
            }
        }

        return defaultValue;
    }

    public void TransitionTo(string sceneName, Dictionary<string, object> data = null)
    {
        if (_sceneName != null)
        {
            return;
        }

        _sceneName = sceneName;
        _transitionData = data;
        gameObject.SetActive(true);
    }

    public void LoadScene()
    {
        if (_sceneName == null)
        {
            return;
        }

        SceneManager.LoadScene(_sceneName);
    }

    public void OnFinish()
    {
        gameObject.SetActive(false);
        _sceneName = null;
        _transitionData = null;
    }
}
