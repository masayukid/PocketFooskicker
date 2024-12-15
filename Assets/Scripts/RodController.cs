using System.Collections;
using UnityEngine;

public class RodController : MonoBehaviour
{
    private const float RESET_DURATION = 1.0f;
    private const float MOVE_RANGE = 1.35f; // 可動範囲
    private const float ROTATION_SPEED = 1000f;

    [SerializeField] private bool isPlayerControlled = false;
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

        if (isPlayerControlled)
        {
            Input.gyro.enabled = true; 
        }
    }

    void FixedUpdate()
    {
        if (_inputHandler == null || _rodRigidbody == null || !_isControllable)
        {
            return;
        }

        if (isPlayerControlled)
        {
            HandlePlayerMovement();
        }
        else
        {
            HandleMovement();
        }
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

    private void HandlePlayerMovement()
    {
        Quaternion gyroAttitude = Input.gyro.attitude;

        Vector3 acceleration = Input.acceleration;
        float horizontal = acceleration.x;
        Vector3 gyroEulerAngles = gyroAttitude.eulerAngles;
        float verticalPosition = Mathf.Clamp(ConvertGyroToRodPosition(gyroEulerAngles.x), -MOVE_RANGE, MOVE_RANGE);

        MovePositionZ_direct(verticalPosition);
        RotateRod(horizontal);
    }

    private float ConvertGyroToRodPosition(float gyroX)
    {
        if (gyroX > 180) gyroX -= 360;
        float normalized = Mathf.Clamp(gyroX / 90f, -1f, 1f);

        return normalized * MOVE_RANGE;
    }

    private void MovePositionZ_direct(float targetZ)
    {

        Vector3 newPosition = _rodTransform.position;
        newPosition.z = targetZ;

        Debug.Log($"Moving Rod to Z={newPosition.z}");

        _rodRigidbody.MovePosition(newPosition);
    }

    private void RotateRod(float horizontal)
    {
        if (Mathf.Abs(horizontal) < 0.3f) return; 

        Vector3 newRotation = _rodTransform.eulerAngles;
        float currentZ = newRotation.z;
        if (currentZ > 180) currentZ -= 360;

        currentZ -= horizontal * ROTATION_SPEED * Time.deltaTime;
        currentZ = Mathf.Clamp(currentZ, -180f, 180f);

        newRotation.z = currentZ;
        _rodRigidbody.MoveRotation(Quaternion.Euler(newRotation));

        Debug.Log($"Rotating Rod: Z={currentZ} with horizontal={horizontal}");
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
