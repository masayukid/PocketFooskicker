using UnityEngine;

public class GyroRodInputHandler : IRodInputHandler
{
    private const float REACTION_DISTANCE = 1.6f; // ボールに反応する最大距離
    private float movementSensitivity = 1.0f; // 移動感度
    private float rotationSensitivity = 1.0f; // 回転感度
    private const float MIN_ACCELERATION_THRESH = 0.3f;

    private Ball _ball;
    private readonly Doll[] _dolls;

    public GyroRodInputHandler(Ball ball, RodController rodController)
    {
        _ball = ball;
        _dolls = rodController.GetDolls();
    }

    public void SetSensitivity(float movement, float rotation)
    {
        movementSensitivity = movement;
        rotationSensitivity = rotation;
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
        // ジャイロセンサーの傾きを取得
        Vector3 tiltZ = Input.gyro.attitude.eulerAngles;

        if (tiltZ.x > 180)
        {
            tiltZ.x -= 360;
        }

        float normalized = Mathf.Clamp(tiltZ.x / 90f, -1f, 1f);
        float movement = -normalized * movementSensitivity; 
        return movement;
    }

    private float GetAccelerationRotation()
    {
        // ジャイロセンサーの角速度（回転速度）を取得
        Vector3 angularVelocity = Input.gyro.rotationRate;

        // 水平方向の回転速度（ジャイロのY軸）を抽出
        float rotationRate = angularVelocity.y;

        // 回転速度を基に回転量を計算
        float rotationDelta = rotationRate * rotationSensitivity; // 感度を利用して調整

        // フレームレートに依存しないスムーズな回転を実現
        return rotationDelta * Time.fixedDeltaTime;
    }
}