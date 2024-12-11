using System;
using UnityEngine;
using UnityEngine.UI;

public class GoalPanel : MonoBehaviour
{
    [SerializeField] private Image _textImage;
    public event Action OnClose;
    private Animation _animation;

    void Awake()
    {
        _animation = GetComponent<Animation>();
    }

    public void Open(Color color)
    {
        if (gameObject.activeSelf)
        {
            throw new Exception("GoalPanelは既に開いています。");
        }

        _textImage.color = color;
        gameObject.SetActive(true);
        _animation.Play();
    }

    public void Close()
    {
        OnClose?.Invoke();
        gameObject.SetActive(false);
    }
}
