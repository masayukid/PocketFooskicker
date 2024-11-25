using UnityEngine;

public class RodController : MonoBehaviour
{
    private const float MOVE_RANGE = 1.35f; // 可動範囲

    [SerializeField] private Transform _rodTransform;
    [SerializeField] private GameObject _dollsObject;

    private Rigidbody _rodRigidbody;
    private Doll[] _dolls;
    private IRodInputHandler _inputHandler;

    void Awake()
    {
        InitializeComponents();
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

    public float GetRotationZ()
    {
        return _rodTransform.eulerAngles.z;
    }

    private void InitializeComponents()
    {
        _rodRigidbody = _rodTransform.GetComponent<Rigidbody>();
        _dolls = _dollsObject.GetComponentsInChildren<Doll>();
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
