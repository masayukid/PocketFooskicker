using UnityEngine;

public class CPURodInputHandler : IRodInputHandler
{
    private Transform _ballTransform;
    private Transform _rodTransform;
    private Doll[] _dolls;

    private float _moveSpeed;
    private float _rotationSpeed;
    private const float REACTION_DISTANCE = 1.5f; // ボールに反応する最大距離
    private const float ROTATION_MULTIPLIER = 300f;

    public CPURodInputHandler(Transform ballTransform, Transform rodTransform, Doll[] dolls, float moveSpeed, float rotationSpeed)
    {
        _ballTransform = ballTransform;
        _rodTransform = rodTransform;
        _dolls = dolls;
        _moveSpeed = moveSpeed;
        _rotationSpeed = rotationSpeed * ROTATION_MULTIPLIER;
    }

    public float GetMovementDelta()
    {
        Doll nearestDoll = FindNearestDoll();
        if (!IsWithinReactionDistance(nearestDoll))
        {
            return 0;
        }

        return CalculateMovementDelta(nearestDoll);
    }

    public float GetRotationDelta()
    {
        Doll nearestDoll = FindNearestDoll();
        if (!IsWithinReactionDistance(nearestDoll))
        {
            return 0;
        }

        return CalculateRotationDelta(nearestDoll);
    }

    private Doll FindNearestDoll()
    {
        Doll nearestDoll = null;
        float minDistance = float.MaxValue;

        foreach (var doll in _dolls)
        {
            float distance = Vector3.Distance(doll.transform.position, _ballTransform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestDoll = doll;
            }
        }

        return nearestDoll;
    }

    private bool IsWithinReactionDistance(Doll nearestDoll)
    {
        if (nearestDoll == null)
        {
            return false;
        }

        float xDistance = Mathf.Abs(nearestDoll.transform.position.x - _ballTransform.position.x);
        return xDistance <= REACTION_DISTANCE;
    }

    private float CalculateMovementDelta(Doll nearestDoll)
    {
        float targetZ = _ballTransform.position.z;
        float currentZ = _rodTransform.position.z;
        float desiredRodZ = currentZ + (targetZ - nearestDoll.transform.position.z);

        return Mathf.MoveTowards(currentZ, desiredRodZ, _moveSpeed * Time.fixedDeltaTime) - currentZ;
    }

    private float CalculateRotationDelta(Doll nearestDoll)
    {
        Vector3 direction = _ballTransform.position - nearestDoll.transform.position;
        float targetAngle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
        float currentAngle = _rodTransform.eulerAngles.z;

        return Mathf.Sign(Mathf.DeltaAngle(currentAngle, targetAngle)) * _rotationSpeed * Time.fixedDeltaTime;
    }

    public void UpdateBallTransform(Transform newBallTransform)
    {
        _ballTransform = newBallTransform;
    }
}
