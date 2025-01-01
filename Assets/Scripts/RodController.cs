using System.Collections;
using UnityEngine;

public class RodController : MonoBehaviour
{
    private const float RESET_DURATION = 1.0f;
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
    private bool _isControllable;

    void Awake()
    {
        InitializeComponents();
        InitializeDollMoveRanges();
    }

    void FixedUpdate()
    {
        if (_inputHandler == null || _rodRigidbody == null || !_isControllable)
        {
            return;
        }

        HandleMovement();
    }

    public void RegisterHandler(IRodInputHandler inputHandler)
    {
        _inputHandler = inputHandler;
    }

    public void SetSensitivity(float movementSensitivity, float rotationSensitivity)
    {
        if (_inputHandler is GyroRodInputHandler gyroHandler)
        {
            gyroHandler.SetSensitivity(movementSensitivity, rotationSensitivity);
            Debug.Log($"RodController: 感度を更新しました - 移動感度 = {movementSensitivity}, 回転感度 = {rotationSensitivity}");
        }
    }

    public void SetColor(Color color)
    {
        foreach (var doll in _dolls)
        {
            doll.SetColor(color);
        }
    }

    public void ResetPositionAndRotation()
    {
        StartCoroutine(ResetPositionAndRotationSmoothly());
    }

    public void SetIsControllable(bool isControllable)
    {
        _isControllable = isControllable;

        if (isControllable)
        {
            _inputHandler.GetMovementDelta();
            _inputHandler.GetRotationDelta();
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

    private IEnumerator ResetPositionAndRotationSmoothly()
    {
        Vector3 startPosition = _rodTransform.position;
        Quaternion startRotation = _rodTransform.rotation;
        Vector3 eulerRotation = startRotation.eulerAngles;

        Vector3 targetPosition = new Vector3(startPosition.x, startPosition.y, 0);
        Quaternion targetRotation = Quaternion.Euler(eulerRotation.x, eulerRotation.y, 0);

        float timer = 0;

        while (timer < RESET_DURATION)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / RESET_DURATION);
            float tCubic = Mathf.Pow(t - 1, 3) + 1;     // 三次関数で滑らかに補完
            _rodTransform.position = Vector3.Lerp(startPosition, targetPosition, tCubic);
            _rodTransform.rotation = Quaternion.Lerp(startRotation, targetRotation, tCubic);

            yield return null;
        }

        _rodTransform.position = targetPosition;
        _rodTransform.rotation = targetRotation;
    }
}
