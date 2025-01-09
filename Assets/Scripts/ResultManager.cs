using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _difficultyText;
    [SerializeField] private TextMeshProUGUI _resultMessage;
    [SerializeField] private Image _difficultyImage;
    [SerializeField] private Sprite _hardSprite;
    [SerializeField] private Sprite _normalSprite;
    [SerializeField] private Sprite _easySprite;
    [SerializeField] private GameObject _victoryParticlePrefab;
    [SerializeField] private Canvas _canvas;

    private const float CRACKER_DELAY_SECONDS = 1.5f;
    private CPUMode _currentCpuMode;

    void Start()
    {
        TransitionData transitionData = TransitionManager.Instance.GetTransitionData();
        int playerScore = transitionData.GetValueOrDefault("PlayerScore", 0);
        int opponentScore = transitionData.GetValueOrDefault("OpponentScore", 0);
        bool isSelfWinner = transitionData.GetValueOrDefault("IsSelfWinner", true);
        _currentCpuMode = transitionData.GetValueOrDefault("CPUMode", CPUMode.Normal);
        string difficulty = _currentCpuMode.ToString();
        _scoreText.text = $"{playerScore} - {opponentScore}";
        _difficultyText.text = $"{difficulty}";
        SetDifficultyImage(_currentCpuMode);

        if (isSelfWinner)
        {
            _resultMessage.text = "You Win !";
            SoundManager.Instance.StopBGM();
            StartCoroutine(PlayVictorySequence());
        }
        else
        {
            _resultMessage.text = "You Lose...";
            _resultMessage.color = Color.red;
            SoundManager.Instance.PlayBGM("bgm_result");
        }
    }

    private void SetDifficultyImage(CPUMode mode)
    {
        switch (mode)
        {
            case CPUMode.Hard:
                _difficultyImage.sprite = _hardSprite;
                break;
            case CPUMode.Normal:
                _difficultyImage.sprite = _normalSprite;
                break;
            case CPUMode.Easy:
                _difficultyImage.sprite = _easySprite;
                break;
            default:
                break;
        }
    }

    private IEnumerator PlayVictorySequence()
    {
        SoundManager.Instance.PlaySE("se_victory");
        yield return new WaitForSeconds(CRACKER_DELAY_SECONDS);
        yield return SpawnVictoryParticles();
        SoundManager.Instance.PlayBGM("bgm_result");
    }

    private IEnumerator SpawnVictoryParticles()
    {
        RectTransform canvasRect = _canvas.GetComponent<RectTransform>();
        float canvasWidth = canvasRect.rect.width;
        float canvasHeight = canvasRect.rect.height;
        Vector2 leftPosition = new Vector2(-canvasWidth / 2, canvasHeight / 8);
        Vector2 rightPosition = new Vector2(canvasWidth / 2, canvasHeight / 8);
        
        InstantiateParticle(leftPosition, Quaternion.Euler(0, 90, 0));
        InstantiateParticle(rightPosition, Quaternion.Euler(0, -90, 0));
        return SoundManager.Instance.PlaySECoroutine("se_cracker");
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

    // BackToMenuButtonから呼ばれるメソッド
    public void OnClickBackToMenu()
    {
        SoundManager.Instance.PlaySE("se_click");
        TransitionManager.Instance.TransitionTo(SceneName.Menu);
    }

    // RetryButtonから呼ばれるメソッド
    public void OnClickRetry()
    {
        SoundManager.Instance.PlaySE("se_click");

        var data = new TransitionData(
            ("CPUMode", _currentCpuMode)
        );
        TransitionManager.Instance.TransitionTo(SceneName.Main, data);
    }
}
