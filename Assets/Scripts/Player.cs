using UnityEngine;

public abstract class Player
{
    public abstract bool IsSelf { get; }
    private readonly ScorePanel _scorePanel;
    private Score _score;

    public Player(Color color, RodController[] rodControllers, ScorePanel scorePanel)
    {
        _scorePanel = scorePanel;
        _score = Score.Zero();
        _scorePanel.DisplayScore(_score);
        
        foreach (var rodController in rodControllers)
        {
            rodController.SetColor(color);
        }
    }

    public void AddScore()
    {
        _score = _score.Incremented();
        _scorePanel.DisplayScore(_score);
    }
}
