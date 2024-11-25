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

    public (float minZ, float maxZ) GetMoveRange()
    {
        return (_minZ, _maxZ);
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
}
