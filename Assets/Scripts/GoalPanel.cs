using System;
using UnityEngine;
using UnityEngine.UI;

public class GoalPanel : MonoBehaviour
{
    [SerializeField] private Image _textImage;
    private Action _onClose;
    private Animation _animation;

    void Awake()
    {
        _animation = GetComponent<Animation>();
    }

    public void Open(Color color, Action onClose)
    {
        if (gameObject.activeSelf)
        {
            throw new Exception("GoalPanelは既に開いています。");
        }

        _textImage.color = color;
        gameObject.SetActive(true);
        _animation.Play();
        _onClose = onClose;
    }

    public void Close()
    {
        _onClose?.Invoke();
        _onClose = null;
        gameObject.SetActive(false);
    }
}
