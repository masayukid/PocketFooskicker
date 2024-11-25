using UnityEngine;

public class RodController : MonoBehaviour
{
    private const float MOVE_RANGE = 1.35f; // 可動範囲

    [SerializeField] private Transform _rodTransform;
    [SerializeField] private GameObject _dollsObject;

    private Rigidbody _rodRigidbody;
    private Doll[] _dolls;
    private IRodInputHandler _inputHandler;

    public void SetColor(Color color)
    {
        foreach (var doll in _dolls)
        {
            doll.SetColor(color);
        }
    }

    public void RegisterHandler(IRodInputHandler inputHandler)
    {
        _inputHandler = inputHandler;
    }

    public Doll[] GetDolls()
    {
        return _dolls;
    }

    void Awake()
    {
        _rodRigidbody = _rodTransform.GetComponent<Rigidbody>();
        _dolls = _dollsObject.GetComponentsInChildren<Doll>();
    }

    void FixedUpdate()
    {
        if (_inputHandler == null || _rodRigidbody == null)
        {
            return;
        }

        MovePosition(_rodTransform.position.z + _inputHandler.GetMovementDelta());
        MoveRotation(_rodTransform.eulerAngles.z + _inputHandler.GetRotationDelta());
    }

    private void MovePosition(float z)
    {
        var newPosition = _rodTransform.position;
        newPosition.z = Mathf.Clamp(z, -MOVE_RANGE, MOVE_RANGE);
        _rodRigidbody.MovePosition(newPosition);
    }

    private void MoveRotation(float z)
    {
        var newRotation = _rodTransform.eulerAngles;
        newRotation.z = z;
        _rodRigidbody.MoveRotation(Quaternion.Euler(newRotation));
    }
}
