using System;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public void OnSelect(string difficulty)
    {
        if (Enum.TryParse(difficulty, out CPUMode cpuMode))
        {
            var data = new Dictionary<string, object>
            {
                { "CPUMode", cpuMode }
            };

            TransitionManager.Instance.TransitionTo("Main", data);
        }
        else
        {
            throw new Exception($"文字列 {difficulty} をCPUModeに変換できません。");
        }
    }
}
