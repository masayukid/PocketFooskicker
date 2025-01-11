using System;
using UnityEngine;

public class BallManager : MonoBehaviour
{
    public event Action<Ball> OnSpawnBall;

    [SerializeField] private GameObject _ballPrefab;
    [SerializeField] private Vector2 _ballInitialOffset;

    private const float BALL_RESPAWN_SPEED = 0.05f;     // ボールを再生成する下限速度
    private const float BALL_RESPAWN_TIMEOUT = 3.0f;    // ボールが下限速度を何秒間下回ったら再生成するか
    private const float VIBRATION_IMPULSE_THRESH = 10.0f;   // バイブレーションを起こす衝撃の閾値

    private Ball _currentBall = null;
    private float _respawnTimer = 0;
    private bool _isSelfTurn = true;
    private bool _isKickedOff = false;

    public void SpawnBall()
    {
        if (_currentBall != null)
        {
            Destroy(_currentBall.gameObject);
        }

        float offsetX = _isSelfTurn ? _ballInitialOffset.x : -_ballInitialOffset.x;
        var ballPosition = new Vector3(offsetX, _ballInitialOffset.y, 0);
        GameObject ballObject = Instantiate(_ballPrefab, ballPosition, Quaternion.identity);

        _currentBall = ballObject.GetComponent<Ball>();
        _currentBall.OnTouch += OnTouchBall;
        _isKickedOff = false;

        OnSpawnBall?.Invoke(_currentBall);
    }

    public void InactivateCurrentBall()
    {
        if (_currentBall == null)
        {
            throw new InvalidOperationException("CurrentBallがnullのため、非表示にできません。");
        }

        _currentBall.Inactivate();
    }

    public void SetTurnPlayer(bool isSelf)
    {
        _isSelfTurn = isSelf;
    }

    public bool IsBallRespawnRequired()
    {
        if (_currentBall == null || _currentBall.IsInactive || !_isKickedOff)
        {
            return false;
        }

        if (_currentBall.GetCurrentSpeed() > BALL_RESPAWN_SPEED)
        {
            ResetRespawnTimer();
            return false;
        }

        _respawnTimer += Time.deltaTime;
        return _respawnTimer > BALL_RESPAWN_TIMEOUT;
    }

    private void OnTouchBall(Collision collision)
    {
        _isKickedOff = true;

        if (collision.gameObject.CompareTag("Rod"))
        {
            SoundManager.Instance.PlaySE("se_kick_ball");

            if (ShouldTriggerVibration(collision))
            {
                VibrationManager.ShortVibration();
            }
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            SoundManager.Instance.PlaySE("se_collision");
        }

        ResetRespawnTimer();
    }

    private bool ShouldTriggerVibration(Collision collision)
    {
        if (collision.impulse.magnitude < VIBRATION_IMPULSE_THRESH)
        {
            return false;
        }

        var rodController = collision.gameObject.GetComponentInParent<RodController>();
        return rodController.OwnerInfo.IsSelf;
    }

    private void ResetRespawnTimer()
    {
        _respawnTimer = 0;
    }
}