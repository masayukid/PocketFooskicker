using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private Color _selfPlayerColor;
    [SerializeField] private Color _opponentPlayerColor;
    [SerializeField] private GameObject _selfPlayerSet;
    [SerializeField] private GameObject _opponentPlayerSet;
    [SerializeField] private ScoreBoard _selfScoreBoard;
    [SerializeField] private ScoreBoard _opponentScoreBoard;
    [SerializeField] private GameObject _controlAreas;
    [SerializeField] private PausePanel _pausePanel;

    [Header("CPU Settings")]
    [SerializeField] private CPUConfig _cpuConfig;

    public CPUMode CurrentCpuMode { get; private set; }
    public Player SelfPlayer { get; private set; }
    public Player OpponentPlayer { get; private set; }

    public Player GetPlayer(bool isSelf)
    {
        return isSelf ? SelfPlayer : OpponentPlayer;
    }

    public void SeizeRodControlAndReset()
    {
        SelfPlayer.SeizeRodControlAndReset();
        OpponentPlayer.SeizeRodControlAndReset();
    }

    public void ReturnRodControlAfterWhistle()
    {
        StartCoroutine(ReturnRodControlCoroutine());
    }

    private IEnumerator ReturnRodControlCoroutine()
    {
        yield return SoundManager.Instance.PlaySECoroutine("se_whistle");
        SelfPlayer.ReturnRodControl();
        OpponentPlayer.ReturnRodControl();
    }

    public void SetUpSelfPlayer(BallManager ballManager, bool gyroEnabled)
    {
        var selfRodControllers = _selfPlayerSet.GetComponentsInChildren<RodController>();
        IRodInputHandler[] inputHandlers;

        if (gyroEnabled)
        {
            Input.gyro.enabled = true;
            inputHandlers = selfRodControllers.Select(rod =>
            {
                var handler = new GyroRodInputHandler(rod);
                ballManager.OnSpawnBall += handler.UpdateBallReference;
                return handler;
            }).ToArray();

            _pausePanel.EnableGyroSettings((GyroRodInputHandler[])inputHandlers);
        }
        else
        {
            inputHandlers = _controlAreas.GetComponentsInChildren<IRodInputHandler>();
        }

        SetUpRodControllers(selfRodControllers, inputHandlers);

        SelfPlayer = new Player(
            true,
            _selfPlayerColor,
            selfRodControllers,
            _selfScoreBoard
        );
    }

    public void SetUpOpponentPlayer(BallManager ballManager, CPUMode cpuMode)
    {
        CurrentCpuMode = cpuMode;
        var settings = _cpuConfig.GetSettingsByMode(CurrentCpuMode);

        var opponentRodControllers = _opponentPlayerSet.GetComponentsInChildren<RodController>();
        var cpuInputHandlers = opponentRodControllers.Select(rod =>
        {
            var handler = new CPURodInputHandler(rod);
            ballManager.OnSpawnBall += handler.UpdateBallReference;
            return handler;
        }).ToArray();

        foreach (var cpuHandler in cpuInputHandlers)
        {
            cpuHandler.ApplyCPUSettings(settings);
        }

        SetUpRodControllers(opponentRodControllers, cpuInputHandlers);

        OpponentPlayer = new Player(
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
            throw new ArgumentException("RodControllerとInputHandlerの要素数が異なります。");
        }

        for (int i = 0; i < rodControllers.Length; i++)
        {
            rodControllers[i].RegisterHandler(inputHandlers[i]);
        }
    }
}