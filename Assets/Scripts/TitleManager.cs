using UnityEngine;

public class TitleManager : MonoBehaviour
{
    [SerializeField] private Animation _titleAnim;

    void Start()
    {
        SoundManager.Instance.PlayBGM("bgm_title");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SoundManager.Instance.PlaySE("se_click");
            
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
