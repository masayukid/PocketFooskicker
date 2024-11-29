using UnityEngine;
using UnityEngine.Assertions;

public class OpponentPlayer : Player
{
    public override bool IsSelf => false;
    
    public OpponentPlayer(Color color, RodController[] rodControllers, ScoreBoard scoreBoard, IRodInputHandler[] inputHandlers) : base(color, rodControllers, scoreBoard)
    {
        Assert.IsTrue(rodControllers.Length == inputHandlers.Length);

        for (int i = 0; i < rodControllers.Length; i++)
        {
            rodControllers[i].RegisterHandler(inputHandlers[i]);
        }
    }
}
