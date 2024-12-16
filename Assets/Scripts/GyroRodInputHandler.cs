using UnityEngine;

public class GyroRodInputHandler : IRodInputHandler
{
    private const float REACTION_DISTANCE = 1.6f; // ボールに反応する最大距離
    private const float ROTATION_MULTIPLIER = 300f;
    private const float GYRO_SENSITIVITY = 1.0f; // ジャイロ感度
    private const float ACCELERATION_SENSITIVITY = 1000f; // 加速度センサ感度
    private const float MOVE_RANGE = 1.0f;

    private Ball _ball;
    private readonly RodController _rodController;
    private readonly Doll[] _dolls;
    private float _moveSpeed;
    private float _rotationSpeed;

    public GyroRodInputHandler(Ball ball, RodController rodController)
    {
        _ball = ball;
        _rodController = rodController;
        _dolls = rodController.GetDolls();
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
        if (nearestDoll != null)
        {
            Debug.Log($"Nearest Doll: {nearestDoll.name}");
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
        // ジャイロセンサーの傾きを取得
        Vector3 tiltZ = Input.gyro.attitude.eulerAngles; 
        if (tiltZ.x > 180) tiltZ.x -= 360;
        float normalized = Mathf.Clamp(tiltZ.x / 90f, -1f, 1f);
        Debug.Log($"normalized: {normalized}");

        float movement = - normalized * GYRO_SENSITIVITY; 
        return movement;
    }

    private float GetAccelerationRotation()
    {
        Vector3 acceleration = Input.acceleration;
        float horizontal = acceleration.x;
        if (Mathf.Abs(horizontal) < 0.3f)
            return 0f;
        float rotationDelta = -acceleration.x * ACCELERATION_SENSITIVITY; 
        return rotationDelta * Time.fixedDeltaTime; 
    }
}