using System;
using UnityEngine;

public class ScoreBoard : MonoBehaviour
{
    private ScoreLamp[] _lamps;

    void Awake()
    {
        _lamps = GetComponentsInChildren<ScoreLamp>();
    }

    public void SetColor(Color color)
    {
        foreach (var lamp in _lamps)
        {
            lamp.SetColor(color);
        }
    }

    public void DisplayScore(Score score)
    {
        if (score.Value > _lamps.Length)
        {
            throw new ArgumentException($"ScoreLampの数より大きなScoreが渡されました: {score.Value}");
        }
        
        for (int i = 0; i < _lamps.Length; i++)
        {
            if (i < score.Value)
            {
                _lamps[i].TurnOn();
            }
            else
            {
                _lamps[i].TurnOff();
            }
        }
    }
}
