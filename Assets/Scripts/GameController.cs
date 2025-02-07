﻿using UnityEngine;

[RequireComponent(typeof(BallManager))]
[RequireComponent(typeof(PlayerManager))]
public class GameController : MonoBehaviour
{
    [SerializeField] private Goal _selfGoal;
    [SerializeField] private Goal _opponentGoal;
    [SerializeField] private GoalPanel _goalPanel;
    [SerializeField] private FoulPanel _foulPanel;

    private BallManager _ballManager;
    private PlayerManager _playerManager;

    void Awake()
    {
        _ballManager = GetComponent<BallManager>();
        _playerManager = GetComponent<PlayerManager>();
    }

    void Start()
    {
        SoundManager.Instance.PlayBGM("bgm_main");
        Initialize();
        _ballManager.SpawnBall();
    }

    void Update()
    {
        _ballManager.ClampBallPosition();
        
        if (_ballManager.IsBallRespawnRequired())
        {
            _ballManager.InactivateCurrentBall();
            _playerManager.SeizeRodControlAndReset();
            _foulPanel.Open(_ballManager.SpawnBall);
        }
    }

    private void Initialize()
    {
        _ballManager.OnSpawnBall += (_) => _playerManager.ReturnRodControlAfterWhistle();

        bool gyroEnabled = SystemInfo.supportsGyroscope;
        _playerManager.SetUpSelfPlayer(_ballManager, gyroEnabled);
        
        TransitionData transitionData = TransitionManager.Instance.GetTransitionData();
        CPUMode _currentCpuMode = transitionData.GetValueOrDefault("CPUMode",  CPUMode.Normal);
        _playerManager.SetUpOpponentPlayer(_ballManager, _currentCpuMode);

        SubscribeGoalEvents();
    }

    private void SubscribeGoalEvents()
    {
        _selfGoal.OnGoal += HandleGoalEvent;
        _opponentGoal.OnGoal += HandleGoalEvent;
    }

    private void HandleGoalEvent(Goal goal)
    {
        VibrationManager.LongVibration();
        
        _ballManager.InactivateCurrentBall();
        _ballManager.SetTurnPlayer(goal.IsSelf);
        _playerManager.SeizeRodControlAndReset();

        Player goalPlayer = _playerManager.GetPlayer(!goal.IsSelf);
        goalPlayer.AddScore();

        if (goalPlayer.IsWinner())
        {
            _goalPanel.Open(goalPlayer.Color, () => EndGame(goalPlayer.IsSelf));
        }
        else
        {
            _goalPanel.Open(goalPlayer.Color, _ballManager.SpawnBall);
        }
    }

    private void EndGame(bool isSelf)
    {
        var resultData = new TransitionData(
            ("PlayerScore", _playerManager.SelfPlayer.Score.Value),
            ("OpponentScore", _playerManager.OpponentPlayer.Score.Value),
            ("IsSelfWinner", isSelf),
            ("CPUMode", _playerManager.CurrentCpuMode)
        );
        TransitionManager.Instance.TransitionTo(SceneName.Result, resultData);
    }
}
