using UnityEngine;

public class OpponentPlayer : Player
{
    public override bool IsSelf => false;

    public OpponentPlayer(Color color, RodController[] rodControllers, ScorePanel scorePanel) : base(color, rodControllers, scorePanel)
    {
        
    }
}