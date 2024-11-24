using UnityEngine;
using UnityEngine.Assertions;

public class OpponentPlayer : Player
{
    public override bool IsSelf => false;

    public OpponentPlayer(Color color, RodController[] rodControllers, ScorePanel scorePanel, IRodInputHandler[] inputHandlers): base(color, rodControllers, scorePanel)
    {
        Assert.IsTrue(rodControllers.Length == inputHandlers.Length);

        for (int i = 0; i < rodControllers.Length; i++)
        {
            rodControllers[i].RegisterHandler(inputHandlers[i]);
        }
    }
}
