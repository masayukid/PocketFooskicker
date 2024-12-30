using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI resultMessage;
    [SerializeField] private GameObject victoryParticlePrefab;
    [SerializeField] private Canvas canvas;

    void Start()
    {
        int playerScore = TransitionManager.Instance.GetDataOrDefault("PlayerScore", 0);
        int opponentScore = TransitionManager.Instance.GetDataOrDefault("OpponentScore", 0);
        bool isSelfWinner = TransitionManager.Instance.GetDataOrDefault("IsSelfWinner", true);
        scoreText.text = $"{playerScore} - {opponentScore}";

        if (isSelfWinner)
        {
            resultMessage.text = "You Win !";
            SpawnVictoryParticles();
        }
        else
        {
            resultMessage.text = "You Lose...";
            resultMessage.color = Color.red;
        }
    }

    private void SpawnVictoryParticles()
    {
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        float canvasWidth = canvasRect.rect.width;
        float canvasHeight = canvasRect.rect.height;
        Vector2 leftPosition = new Vector2(-canvasWidth / 2, canvasHeight / 8);
        Vector2 rightPosition = new Vector2(canvasWidth / 8, canvasHeight / 8);
        
        InstantiateParticle(leftPosition, Quaternion.Euler(0, 90, 0));
        InstantiateParticle(rightPosition, Quaternion.Euler(0, -90, 0));
    }

    private void InstantiateParticle(Vector2 anchoredPosition, Quaternion rotation)
    {
        GameObject particleEffect = Instantiate(victoryParticlePrefab, canvas.transform);

        RectTransform rectTransform = particleEffect.GetComponent<RectTransform>();

        if (rectTransform != null)
        {
            rectTransform.anchoredPosition = anchoredPosition;
            rectTransform.localRotation = rotation;
            rectTransform.localScale = Vector3.one;
        }

        ParticleSystem ps = particleEffect.GetComponent<ParticleSystem>();

        if (ps != null)
        {
            ps.Play();
        }

        Destroy(particleEffect, 5.0f);
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
