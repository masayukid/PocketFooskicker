using UnityEngine;

public class RodController : MonoBehaviour
{
    private const float MOVE_RANGE = 1.35f; // 可動範囲

    [SerializeField] private Transform _topBushing;
    [SerializeField] private Transform _bottomBushing;
    [SerializeField] private Transform _rodTransform;
    [SerializeField] private GameObject _dollsObject;

    private Rigidbody _rodRigidbody;
    private Doll[] _dolls;
    private IRodInputHandler _inputHandler;
    private float _topBushingDistance;    // 上部のDollとBushingの距離
    private float _bottomBushingDistance; // 下部のDollとBushingの距離

    void Awake()
    {
        InitializeComponents();
        InitializeDollMoveRanges();
    }

    void FixedUpdate()
    {
        if (_inputHandler == null || _rodRigidbody == null)
        {
            return;
        }

        HandleMovement();
    }

    public void RegisterHandler(IRodInputHandler inputHandler)
    {
        _inputHandler = inputHandler;
    }

    public void SetColor(Color color)
    {
        foreach (var doll in _dolls)
        {
            doll.SetColor(color);
        }
    }

    public Doll[] GetDolls()
    {
        return _dolls;
    }

    public float GetPositionZ()
    {
        return _rodTransform.position.z;
    }

    private void InitializeComponents()
    {
        _rodRigidbody = _rodTransform.GetComponent<Rigidbody>();
        _dolls = _dollsObject.GetComponentsInChildren<Doll>();
    }

    private void CalculateBushingDistances()
    {
        if (_dolls.Length == 0)
        {
            return;
        }

        float topDollZ = _dolls[0].GetPosition().z;
        float bottomDollZ = _dolls[_dolls.Length - 1].GetPosition().z;

        _topBushingDistance = Mathf.Abs(_topBushing.position.z - topDollZ);
        _bottomBushingDistance = Mathf.Abs(_bottomBushing.position.z - bottomDollZ);
    }

    private void InitializeDollMoveRanges()
    {
        CalculateBushingDistances();

        foreach (var doll in _dolls)
        {
            float initialZ = doll.GetPosition().z;
            float minZ = initialZ - _bottomBushingDistance;
            float maxZ = initialZ + _topBushingDistance;
            doll.SetMoveRange(minZ, maxZ);
        }
    }

    private void HandleMovement()
    {
        MovePositionZ(_rodTransform.position.z + _inputHandler.GetMovementDelta());
        MoveRotationZ(_rodTransform.eulerAngles.z + _inputHandler.GetRotationDelta());
    }

    private void MovePositionZ(float z)
    {
        var newPosition = _rodTransform.position;
        newPosition.z = Mathf.Clamp(z, -MOVE_RANGE, MOVE_RANGE);
        _rodRigidbody.MovePosition(newPosition);
    }

    private void MoveRotationZ(float z)
    {
        var newRotation = _rodTransform.eulerAngles;
        newRotation.z = z;
        _rodRigidbody.MoveRotation(Quaternion.Euler(newRotation));
    }
}
