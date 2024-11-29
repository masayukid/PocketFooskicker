using UnityEngine;

public class OpponentPlayer : Player
{
    public override bool IsSelf => false;

    public OpponentPlayer(Color color, RodController[] rodControllers, ScoreBoard scoreBoard) : base(color, rodControllers, scoreBoard)
    {
        
    }
}