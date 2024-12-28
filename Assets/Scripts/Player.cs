using UnityEngine;

public class Player : IPlayerInfo
{
    public bool IsSelf { get; }
    public Color Color { get; }
    private readonly RodController[] _rodControllers;
    private readonly ScoreBoard _scoreBoard;
    private Score _score;

    public Player(bool isSelf, Color color, RodController[] rodControllers, ScoreBoard scoreBoard)
    {
        IsSelf = isSelf;
        Color = color;
        _rodControllers = rodControllers;
        _score = Score.Zero();
        
        _scoreBoard = scoreBoard;
        _scoreBoard.SetColor(color);
        _scoreBoard.DisplayScore(_score);

        foreach (var rodController in rodControllers)
        {
            rodController.SetOwnerInfo(this);
        }
    }

    public void AddScore()
    {
        _score = _score.Incremented();
        _scoreBoard.DisplayScore(_score);
    }

    public void SeizeRodControlAndReset()
    {
        foreach (var rodController in _rodControllers)
        {
            rodController.SetIsControllable(false);
            rodController.ResetPositionAndRotation();
        }
    }

    public void ReturnRodControl()
    {
        foreach (var rodController in _rodControllers)
        {
            rodController.SetIsControllable(true);
        }
    }
}
