using UnityEngine;

public class Doll : MonoBehaviour
{
    public IPlayerInfo OwnerInfo { get; private set; }

    [SerializeField] private Renderer _headRenderer;
    [SerializeField] private Renderer _bodyRenderer;

    private float _minZ;
    private float _maxZ;

    public void SetOwnerInfo(IPlayerInfo ownerInfo)
    {
        OwnerInfo = ownerInfo;

        _headRenderer.material.color = ownerInfo.Color;
        _bodyRenderer.material.color = ownerInfo.Color;
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
