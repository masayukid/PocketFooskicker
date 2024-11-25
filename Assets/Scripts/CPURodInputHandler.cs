using UnityEngine;

public class CPURodInputHandler : IRodInputHandler
{
    private Ball _ball;
    private readonly RodController _rodController;
    private readonly Doll[] _dolls;

    private float _moveSpeed;
    private float _rotationSpeed;
    private const float REACTION_DISTANCE = 1.3f; // ボールに反応する最大距離
    private const float ROTATION_MULTIPLIER = 300f;

    public CPURodInputHandler(Ball ball, RodController rodController)
    {
        _ball = ball;
        _rodController = rodController;
        _dolls = rodController.GetDolls();
    }

    public void ApplyCPUSettings(CPUConfig.ModeSettings settings)
    {
        _moveSpeed = settings.MoveSpeed;
        _rotationSpeed = settings.RotationSpeed * ROTATION_MULTIPLIER;
    }

    public void UpdateBallTransform(Ball newBall)
    {
        _ball = newBall;
    }

    public float GetMovementDelta()
    {
        Doll nearestDoll = FindNearestDoll();
        if (!IsWithinReactionDistance(nearestDoll)) return 0;

        return CalculateMovementDelta(nearestDoll);
    }

    public float GetRotationDelta()
    {
        Doll nearestDoll = FindNearestDoll();
        if (!IsWithinReactionDistance(nearestDoll)) return 0;

        return CalculateRotationDelta(nearestDoll);
    }

    private Doll FindNearestDoll()
    {
        Doll nearestDoll = null;
        float minDistance = float.MaxValue;

        foreach (var doll in _dolls)
        {
            float distance = Vector3.Distance(doll.transform.position, _ball.GetPosition());
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
        if (nearestDoll == null) return false;

        float xDistance = Mathf.Abs(nearestDoll.transform.position.x - _ball.GetPosition().x);
        return xDistance <= REACTION_DISTANCE;
    }

    private float CalculateMovementDelta(Doll nearestDoll)
    {
        float targetZ = _ball.GetPosition().z;
        float currentZ = _rodController.transform.position.z;
        float desiredRodZ = currentZ + (targetZ - nearestDoll.transform.position.z);

        return Mathf.MoveTowards(currentZ, desiredRodZ, _moveSpeed * Time.fixedDeltaTime) - currentZ;
    }
    
    private float CalculateRotationDelta(Doll nearestDoll)
    {
        Vector3 direction = _ball.GetPosition() - nearestDoll.transform.position;
        float targetAngle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
        float currentAngle = _rodController.transform.eulerAngles.z;

        return Mathf.Sign(Mathf.DeltaAngle(currentAngle, targetAngle)) * _rotationSpeed * Time.fixedDeltaTime;
    }
}
