using UnityEngine;

public class GyroRodInputHandler : IRodInputHandler
{
    private const float REACTION_DISTANCE = 1.6f; // ボールに反応する最大距離
    private float _movementSensitivity = 1.0f; 
    private float _rotationSensitivity = 300f;

    private Ball _ball;
    private readonly Doll[] _dolls;

    public GyroRodInputHandler(Ball ball, RodController rodController)
    {
        _ball = ball;
        _dolls = rodController.GetDolls();
    }

    public void SetMovementSensitivity(float movementSensitivity)
    {
        _movementSensitivity = movementSensitivity;
    }
    
    public void SetRotationSensitivity(float rotationSensitivity)
    {
        _rotationSensitivity = rotationSensitivity;
    }

    public void UpdateBallReference(Ball newBall)
    {
        _ball = newBall;
    }

    public float GetMovementDelta()
    {
        Doll nearestDoll = FindNearestDoll();

        if (!IsWithinReactionDistance(nearestDoll))
        {
            return 0;
        }

        return GetGyroMovement();
    }

    public float GetRotationDelta()
    {
        Doll nearestDoll = FindNearestDoll();

        if (!IsWithinReactionDistance(nearestDoll))
        {
            return 0;
        }

        return GetAccelerationRotation();
    }

    private Doll FindNearestDoll()
    {
        Doll nearestDoll = null;
        float minDistance = float.MaxValue;

        foreach (var doll in _dolls)
        {
            float ballZ = _ball.GetPosition().z;

            if (!doll.IsWithinMoveRange(ballZ))
            {
                continue;
            }

            float distance = Vector3.Distance(doll.GetPosition(), _ball.GetPosition());

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

        float xDistance = Mathf.Abs(nearestDoll.GetPosition().x - _ball.GetPosition().x);
        return xDistance <= REACTION_DISTANCE;
    }

    private float GetGyroMovement()
    {
        Quaternion deviceRotation = Input.gyro.attitude;
        Vector3 gravity = deviceRotation * Vector3.down;
        float tilt = gravity.z;
        float normalizedTilt = Mathf.Clamp(tilt, -1f, 1f);
        float movement = normalizedTilt * _movementSensitivity;
        return movement;
    }

    private float GetAccelerationRotation()
    {
        Vector3 angularVelocity = Input.gyro.rotationRate;
        float rotationRate = angularVelocity.y;
        float rotationDelta = rotationRate * _rotationSensitivity; 
        return rotationDelta * Time.fixedDeltaTime;
    }
}