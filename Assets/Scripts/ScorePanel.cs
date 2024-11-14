using TMPro;
using UnityEngine;

public class ScorePanel : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;

    public void DisplayScore(Score score)
    {
        _scoreText.text = score.ToString();
    }
}
