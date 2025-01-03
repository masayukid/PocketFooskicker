using UnityEngine;

public class TitleManager : MonoBehaviour
{
    [SerializeField] private Animation _titleAnim;
    [SerializeField] private AudioSource _bgmAudioSource;
    [SerializeField] private AudioSource _seTapAudioSource;

    private AudioSource _audioSource;

    void Start()
    {
        _bgmAudioSource.Play();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _seTapAudioSource.Play();

            if (_titleAnim.isPlaying)
            {
                // アニメーション再生中ならスキップ
                AnimationState state = _titleAnim[_titleAnim.clip.name];
                state.normalizedTime = 1;
                _titleAnim.Play();
            }
            else
            {
                TransitionManager.Instance.TransitionTo("Menu");
            }
        }
    }
}
