using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private List<AudioClip> _bgmClips;
    [SerializeField] private float _bgmVolume = 0.5f;

    [SerializeField] private List<AudioClip> _seClips;
    [SerializeField] private float _seVolume = 1.0f;

    private AudioSource _bgmSource;
    private AudioSource _seSource;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        _bgmSource = gameObject.AddComponent<AudioSource>();
        _bgmSource.loop = true;
        _bgmSource.volume = _bgmVolume;

        _seSource = gameObject.AddComponent<AudioSource>();
        _seSource.loop = false;
    }

    // BGM再生メソッド
    public void PlayBGM(string clipName)
    {
        AudioClip clip = _bgmClips.Find(se => se.name == clipName);

        if (_bgmSource.clip == clip)
        {
            return;
        }

        if (clip != null)
        {
            _bgmSource.clip = clip;
            _bgmSource.Play();
        }
        else
        {
            Debug.LogWarning($"BGM '{clipName}' が見つかりません");
        }
    }

    // SE再生メソッド
    public void PlaySE(string clipName)
    {
        StartCoroutine(PlaySECoroutine(clipName));
    }

    // SE再生コルーチン
    public IEnumerator PlaySECoroutine(string clipName)
    {
        AudioClip clip = _seClips.Find(se => se.name == clipName);

        if (clip != null)
        {
            _seSource.PlayOneShot(clip, _seVolume);
            yield return new WaitForSeconds(clip.length);
        }
        else
        {
            Debug.LogWarning($"SE '{clipName}' が見つかりません");
        }
    }
}
