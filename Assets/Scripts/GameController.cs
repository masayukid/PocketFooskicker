using System;
using UnityEngine;
using System.Linq;

public class GameController : MonoBehaviour
{
    public event Action<Ball> OnSpawnBall;
    private const float BALL_RESPAWN_SPEED = 0.05f;     // ボールを再生成する下限速度
    private const float BALL_RESPAWN_TIMEOUT = 3.0f;    // ボールが下限速度を何秒間下回ったら再生成するか

    [SerializeField] private Color _selfPlayerColor;
    [SerializeField] private Color _opponentPlayerColor;
    [SerializeField] private GameObject _selfPlayerSet;
    [SerializeField] private GameObject _opponentPlayerSet;
    [SerializeField] private ScorePanel _selfScorePanel;
    [SerializeField] private ScorePanel _opponentScorePanel;
    [SerializeField] private GameObject _controlAreas;
    [SerializeField] private Goal _selfGoal;
    [SerializeField] private Goal _opponentGoal;
    [SerializeField] private GameObject _ballPrefab;
    [SerializeField] private Vector2 _ballInitialOffset;
    [Header("CPU Settings")]
    [SerializeField] private CPUConfig _cpuConfig;
    [SerializeField] private string _cpuModeName;

    private SelfPlayer _selfPlayer;
    private OpponentPlayer _opponentPlayer;
    private Ball _currentBall;
    private float _respawnTimer;
    private bool _isSelfTurn;
    private bool _isKickedOff;

    void Awake()
    {
        Initialize();
    }

    void Start()
    {
        SpawnBall(_isSelfTurn);
    }

    void Update()
    {
        if (_currentBall == null || !_isKickedOff)
        {
            return;
        }

        if (_currentBall.GetCurrentSpeed() > BALL_RESPAWN_SPEED)
        {
            ResetRespawnTimer();
            return;
        }

        _respawnTimer += Time.deltaTime;

        if (_respawnTimer > BALL_RESPAWN_TIMEOUT)
        {
            SpawnBall(_isSelfTurn);
        }
    }

    private void Initialize()
    {
        SetupPlayers();
        _selfGoal.OnGoal += OnGoal;
        _opponentGoal.OnGoal += OnGoal;

        _currentBall = null;
        ResetRespawnTimer();
        _isSelfTurn = true;
        _isKickedOff = false;
    }

    private void SetupPlayers()
    {
        var selfRodControllers = _selfPlayerSet.GetComponentsInChildren<RodController>();
        var inputHandlers = _controlAreas.GetComponentsInChildren<IRodInputHandler>();

        var opponentRodControllers = _opponentPlayerSet.GetComponentsInChildren<RodController>();
        var ballTransform = _ballPrefab.transform;
        if (!System.Enum.TryParse(_cpuModeName, out CPUMode cpuMode))
        {
            return;
        }
        var settings = _cpuConfig.GetSettingsByName(cpuMode);
        var cpuInputHandlers = opponentRodControllers.Select(rod =>
        {
            var handler = new CPURodInputHandler(_currentBall, rod);
            OnSpawnBall += handler.UpdateBallTransform;
            return handler as IRodInputHandler;
        }).ToArray();

        foreach (var handler in cpuInputHandlers)
        {
            if (handler is CPURodInputHandler cpuHandler)
            {
                cpuHandler.ApplyCPUSettings(settings);
            }
        }

        _selfPlayer = new SelfPlayer(
            _selfPlayerColor,
            selfRodControllers,
            _selfScorePanel,
            inputHandlers
        );

        _opponentPlayer = new OpponentPlayer(
            _opponentPlayerColor,
            opponentRodControllers,
            _opponentScorePanel,
            cpuInputHandlers
        );
    }

    private void OnGoal(Goal goal)
    {
        if (goal.IsSelf)
        {
            _opponentPlayer.AddScore();
        }
        else
        {
            _selfPlayer.AddScore();
        }

        _isSelfTurn = goal.IsSelf;
        SpawnBall(_isSelfTurn);
    }

    private void SpawnBall(bool isSelf)
    {
        if (_currentBall != null)
        {
            Destroy(_currentBall.gameObject);
        }

        GameObject ballObject = Instantiate(_ballPrefab);
        float offsetX = isSelf ? _ballInitialOffset.x : -_ballInitialOffset.x;
        ballObject.transform.position = new Vector3(offsetX, _ballInitialOffset.y, 0);

        _currentBall = ballObject.GetComponent<Ball>();
        _currentBall.OnTouch += OnTouchBall;
        _isKickedOff = false;
        OnSpawnBall?.Invoke(_currentBall);
    }

    private void OnTouchBall()
    {
        if (!_isKickedOff)
        {
            _isKickedOff = true;
        }

        ResetRespawnTimer();
    }

    private void ResetRespawnTimer()
    {
        _respawnTimer = 0;
    }
}
