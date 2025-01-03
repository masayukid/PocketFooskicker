using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    public event Action<Ball> OnSpawnBall;

    private const float BALL_RESPAWN_SPEED = 0.05f;     // ボールを再生成する下限速度
    private const float BALL_RESPAWN_TIMEOUT = 3.0f;    // ボールが下限速度を何秒間下回ったら再生成するか

    [SerializeField] private Color _selfPlayerColor;
    [SerializeField] private Color _opponentPlayerColor;
    [SerializeField] private GameObject _selfPlayerSet;
    [SerializeField] private GameObject _opponentPlayerSet;
    [SerializeField] private ScoreBoard _selfScoreBoard;
    [SerializeField] private ScoreBoard _opponentScoreBoard;
    [SerializeField] private GameObject _controlAreas;
    [SerializeField] private Goal _selfGoal;
    [SerializeField] private Goal _opponentGoal;
    [SerializeField] private GoalPanel _goalPanel;
    [SerializeField] private GameObject _ballPrefab;
    [SerializeField] private Vector2 _ballInitialOffset;
    [Header("CPU Settings")]
    [SerializeField] private CPUConfig _cpuConfig;
    [SerializeField] private CPUMode _defaultCPUMode;

    private CPUMode _currentCpuMode;
    private Player _selfPlayer;
    private Player _opponentPlayer;
    private Ball _currentBall;
    private float _respawnTimer;
    private bool _isSelfTurn;
    private bool _isKickedOff;

    void Start()
    {
        Initialize();
        SpawnBall();
    }

    void Update()
    {
        HandleBallRespawn();
    }

    private void Initialize()
    {
        SetupPlayers();
        SubscribeGoalEvents();
        ResetGameState();
    }

    private void SetupPlayers()
    {
        // プレイヤー設定
        var selfRodControllers = _selfPlayerSet.GetComponentsInChildren<RodController>();
        IRodInputHandler[] inputHandlers;
        
        if (SystemInfo.supportsGyroscope)
        {
            // デバイスがジャイロに対応している場合
            Input.gyro.enabled = true;
            inputHandlers = selfRodControllers.Select(rod =>
            {
                var handler = new GyroRodInputHandler(_currentBall, rod);
                OnSpawnBall += handler.UpdateBallReference;
                return handler;
            }).ToArray();
        }
        else
        {
            inputHandlers = _controlAreas.GetComponentsInChildren<IRodInputHandler>();
        }

        SetUpRodControllers(selfRodControllers, inputHandlers);

        // CPU設定
        _currentCpuMode = TransitionManager.Instance.GetDataOrDefault("CPUMode", _defaultCPUMode);
        var settings = _cpuConfig.GetSettingsByMode(_currentCpuMode);

        var opponentRodControllers = _opponentPlayerSet.GetComponentsInChildren<RodController>();
        var cpuInputHandlers = opponentRodControllers.Select(rod =>
        {
            var handler = new CPURodInputHandler(_currentBall, rod);
            OnSpawnBall += handler.UpdateBallReference;
            return handler;
        }).ToArray();

        foreach (var cpuHandler in cpuInputHandlers)
        {
            cpuHandler.ApplyCPUSettings(settings);
        }

        SetUpRodControllers(opponentRodControllers, cpuInputHandlers);

        _selfPlayer = new Player(
            true,
            _selfPlayerColor,
            selfRodControllers,
            _selfScoreBoard
        );

        _opponentPlayer = new Player(
            false,
            _opponentPlayerColor,
            opponentRodControllers,
            _opponentScoreBoard
        );
    }

    private void SetUpRodControllers(RodController[] rodControllers, IRodInputHandler[] inputHandlers)
    {
        if (rodControllers.Length != inputHandlers.Length)
        {
            throw new Exception("RodControllerとInputHandlerの要素数が異なります。");
        }

        for (int i = 0; i < rodControllers.Length; i++)
        {
            rodControllers[i].RegisterHandler(inputHandlers[i]);
        }
    }

    private void SubscribeGoalEvents()
    {
        _selfGoal.OnGoal += OnGoal;
        _opponentGoal.OnGoal += OnGoal;
    }

    private void ResetGameState()
    {
        _currentBall = null;
        ResetRespawnTimer();
        _isSelfTurn = true;
        _isKickedOff = false;
    }

    private void SpawnBall()
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

        _selfPlayer.ReturnRodControl();
        _opponentPlayer.ReturnRodControl();
    }

    private void HandleBallRespawn()
    {
        if (_currentBall == null || !_isKickedOff)
        {
            return;
        }

        if (_currentBall.GetCurrentSpeed() > BALL_RESPAWN_SPEED)
        {
            ResetRespawnTimer();
            return;
        }

        _respawnTimer += Time.deltaTime;

        if (_respawnTimer > BALL_RESPAWN_TIMEOUT)
        {
            SpawnBall();
        }
    }

    private void OnGoal(Goal goal)
    {
        _currentBall.Inactivate();

        Player goalPlayer = goal.IsSelf ? _opponentPlayer : _selfPlayer;
        goalPlayer.AddScore();
        _isSelfTurn = !goalPlayer.IsSelf;

        _selfPlayer.SeizeRodControlAndReset();
        _opponentPlayer.SeizeRodControlAndReset();

        if (goalPlayer.IsWinner())
        {
            _goalPanel.Open(goalPlayer.Color, () => EndGame(goalPlayer.IsSelf));
        }
        else
        {
            _goalPanel.Open(goalPlayer.Color, SpawnBall);
        }
    }

    private void OnTouchBall()
    {
        if (!_isKickedOff)
        {
            _isKickedOff = true;
        }

        ResetRespawnTimer();
    }

    private void EndGame(bool isSelf)
    {
        var resultData = new Dictionary<string, object>
        {
            { "PlayerScore", _selfPlayer.Score.Value },
            { "OpponentScore", _opponentPlayer.Score.Value },
            { "IsSelfWinner", isSelf },
            { "CPUMode", _currentCpuMode }
        };
        TransitionManager.Instance.TransitionTo("Result", resultData);
    }

    private void ResetRespawnTimer()
    {
        _respawnTimer = 0;
    }
}
