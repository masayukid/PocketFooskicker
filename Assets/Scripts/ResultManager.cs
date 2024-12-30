using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI resultMessage;

    void Start()
    {
        int playerScore = TransitionManager.Instance.GetDataOrDefault("PlayerScore", 0);
        int opponentScore = TransitionManager.Instance.GetDataOrDefault("OpponentScore", 0);
        bool isSelfWinner = TransitionManager.Instance.GetDataOrDefault("IsSelfWinner", true);
        scoreText.text = $"{playerScore} - {opponentScore}";

        if (isSelfWinner)
        {
            resultMessage.text = "You Win !";
        }
        else
        {
            resultMessage.text = "You Lose...";
            resultMessage.color = Color.red;
        }
    }

    public void OnSelect(string sceneName)
    {
        if (sceneName == "Menu" || sceneName == "Main")
        {
            TransitionManager.Instance.TransitionTo(sceneName);
        }
        else
        {
            throw new Exception($"文字列 {sceneName} は不正です。");
        }
    }
}
