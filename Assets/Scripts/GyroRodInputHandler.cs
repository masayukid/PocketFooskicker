using UnityEngine;

public class GyroRodInputHandler : IRodInputHandler
{
    private const float REACTION_DISTANCE = 1.6f; // ボールに反応する最大距離
    public float MovementSensitivity = 1.0f; 
    public float RotationSensitivity = 300f;
    private const float MIN_ACCELERATION_THRESH = 0.3f;

    private Ball _ball;
    private readonly Doll[] _dolls;

    public GyroRodInputHandler(Ball ball, RodController rodController)
    {
        _ball = ball;
        _dolls = rodController.GetDolls();
    }

    public void SetSensitivity(float movementSensitivity, float rotationSensitivity)
    {
        MovementSensitivity = movementSensitivity;
        RotationSensitivity = rotationSensitivity;
        Debug.Log($"[SetSensitivity] Movement sensitivity set to: {MovementSensitivity}");
        Debug.Log($"[SetSensitivity] Rotation sensitivity set to: {RotationSensitivity}");
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
        Vector3 tiltZ = Input.gyro.attitude.eulerAngles;

        if (tiltZ.x > 180)
        {
            tiltZ.x -= 360;
        }

        float normalized = Mathf.Clamp(tiltZ.x / 90f, -1f, 1f);
        float movement = -normalized * MovementSensitivity; 
        return movement;
    }

    private float GetAccelerationRotation()
    {
        Vector3 angularVelocity = Input.gyro.rotationRate;
        float rotationRate = angularVelocity.y;
        float rotationDelta = rotationRate * RotationSensitivity; 
        return rotationDelta * Time.fixedDeltaTime;
    }
}