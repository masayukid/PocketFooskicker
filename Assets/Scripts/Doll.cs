using UnityEngine;

public class Doll : MonoBehaviour
{
    [SerializeField] private Renderer _headRenderer;
    [SerializeField] private Renderer _bodyRenderer;

    public void SetColor(Color color)
    {
        _headRenderer.material.color = color;
        _bodyRenderer.material.color = color;
    }
}
