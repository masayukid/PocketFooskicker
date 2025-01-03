using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _resultMessage;
    [SerializeField] private GameObject _victoryParticlePrefab;
    [SerializeField] private Canvas _canvas;

    void Start()
    {
        int playerScore = TransitionManager.Instance.GetDataOrDefault("PlayerScore", 0);
        int opponentScore = TransitionManager.Instance.GetDataOrDefault("OpponentScore", 0);
        bool isSelfWinner = TransitionManager.Instance.GetDataOrDefault("IsSelfWinner", true);
        _scoreText.text = $"{playerScore} - {opponentScore}";

        if (isSelfWinner)
        {
            _resultMessage.text = "You Win !";
            SpawnVictoryParticles();
        }
        else
        {
            _resultMessage.text = "You Lose...";
            _resultMessage.color = Color.red;
        }
    }

    private void SpawnVictoryParticles()
    {
        RectTransform canvasRect = _canvas.GetComponent<RectTransform>();
        float canvasWidth = canvasRect.rect.width;
        float canvasHeight = canvasRect.rect.height;
        Vector2 leftPosition = new Vector2(-canvasWidth / 2, canvasHeight / 8);
        Vector2 rightPosition = new Vector2(canvasWidth / 2, canvasHeight / 8);
        
        InstantiateParticle(leftPosition, Quaternion.Euler(0, 90, 0));
        InstantiateParticle(rightPosition, Quaternion.Euler(0, -90, 0));
    }

    private void InstantiateParticle(Vector2 anchoredPosition, Quaternion rotation)
    {
        GameObject particleEffect = Instantiate(_victoryParticlePrefab, _canvas.transform);

        RectTransform rectTransform = particleEffect.GetComponent<RectTransform>();

        if (rectTransform != null)
        {
            rectTransform.anchoredPosition = anchoredPosition;
            rectTransform.localRotation = rotation;
            rectTransform.localScale = Vector3.one;
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
