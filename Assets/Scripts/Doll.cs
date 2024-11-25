using UnityEngine;

public class Doll : MonoBehaviour
{
    [SerializeField] private Renderer _headRenderer;
    [SerializeField] private Renderer _bodyRenderer;

    private float _minZ;
    private float _maxZ;

    public void SetColor(Color color)
    {
        _headRenderer.material.color = color;
        _bodyRenderer.material.color = color;
    }

    public void SetMoveRange(float minZ, float maxZ)
    {
        _minZ = minZ;
        _maxZ = maxZ;
    }

    public bool IsWithinMoveRange(float z)
    {
        return _minZ < z && z < _maxZ;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
}
