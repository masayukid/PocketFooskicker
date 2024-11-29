using UnityEngine;

public abstract class Player
{
    public abstract bool IsSelf { get; }
    private readonly ScoreBoard _scoreBoard;
    private Score _score;

    public Player(Color color, RodController[] rodControllers, ScoreBoard scoreBoard)
    {
        _score = Score.Zero();
        
        _scoreBoard = scoreBoard;
        _scoreBoard.SetColor(color);
        _scoreBoard.DisplayScore(_score);
        
        foreach (var rodController in rodControllers)
        {
            rodController.SetColor(color);
        }
    }

    public void AddScore()
    {
        _score = _score.Incremented();
        _scoreBoard.DisplayScore(_score);
    }
}
