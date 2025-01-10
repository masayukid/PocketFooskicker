using UnityEngine;

public class CPURodInputHandler : IRodInputHandler
{
    private const float REACTION_DISTANCE = 1.6f; // ボールに反応する最大距離
    private const float ROTATION_MULTIPLIER = 300f;

    private Ball _ball;
    private readonly RodController _rodController;
    private readonly Doll[] _dolls;
    private float _moveSpeed;
    private float _rotationSpeed;

    public CPURodInputHandler(RodController rodController)
    {
        _rodController = rodController;
        _dolls = rodController.GetDolls();
    }

    public void ApplyCPUSettings(CPUConfig.ModeSettings settings)
    {
        _moveSpeed = settings.MoveSpeed;
        _rotationSpeed = settings.RotationSpeed * ROTATION_MULTIPLIER;
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

        return CalculateMovementDelta(nearestDoll);
    }

    public float GetRotationDelta()
    {
        Doll nearestDoll = FindNearestDoll();

        if (!IsWithinReactionDistance(nearestDoll))
        {
            return 0;
        }

        return CalculateRotationDelta();
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

    private float CalculateMovementDelta(Doll nearestDoll)
    {
        float targetZ = _ball.GetPosition().z;
        float currentZ = _rodController.GetPositionZ();
        float desiredRodZ = currentZ + (targetZ - nearestDoll.GetPosition().z);

        return Mathf.MoveTowards(currentZ, desiredRodZ, _moveSpeed * Time.fixedDeltaTime) - currentZ;
    }
    
    private float CalculateRotationDelta()
    {
        return _rotationSpeed * Time.fixedDeltaTime;
    }
}
