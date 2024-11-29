using UnityEngine;

public class ScoreLamp : MonoBehaviour
{
    [SerializeField] private Light _light;

    private Renderer _renderer;
    private Color _color;
    private Color _defaultColor;

    void Awake()
    {
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
        _light.enabled = true;
        _renderer.material.color = _color;
    }

    public void TurnOff()
    {
        _light.enabled = false;
        _renderer.material.color = _defaultColor;
    }
}
