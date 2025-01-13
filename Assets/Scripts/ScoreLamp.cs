using UnityEngine;

public class ScoreLamp : MonoBehaviour
{
    [SerializeField] private Light _light;

    private Animation _animation;
    private Renderer _renderer;
    private Color _color;
    private Color _defaultColor;
    private bool _isTurnedOn = false;

    void Awake()
    {
        _animation = GetComponent<Animation>();
        _renderer = GetComponent<Renderer>();
        _defaultColor = _renderer.material.color;
    }

    public void SetColor(Color color)
    {
        _light.color = color;
        _color = color;
        _color.a = _defaultColor.a;
    }

    public void TurnOn()
    {
        if (_isTurnedOn)
        {
            return;
        }

        _isTurnedOn = true;
        _renderer.material.color = _color;
        _animation.Play();
    }

    public void TurnOff()
    {
        _light.enabled = false;
        _renderer.material.color = _defaultColor;
        _isTurnedOn = false;
    }
}
