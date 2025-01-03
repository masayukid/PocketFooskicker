using UnityEngine;

public class TitleManager : MonoBehaviour
{
    [SerializeField] private Animation _titleAnim;
    [SerializeField] private AudioSource _seClick;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _seClick.Play();

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
