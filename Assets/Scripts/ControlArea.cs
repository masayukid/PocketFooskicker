using UnityEngine;
using UnityEngine.EventSystems;

public class ControlArea : MonoBehaviour, IRodInputHandler, IDragHandler
{
    private const float ROT_SPEED = 1.0f;       // 回転速度
    private const float MOVE_SPEED = 0.10f;     // 移動速度

    private float _rotationBuffer;
    private float _movementBuffer;

    void Awake()
    {
        _rotationBuffer = 0;
        _movementBuffer = 0;
    }

    public float GetRotationDelta()
    {
        float delta = _rotationBuffer;
        _rotationBuffer = 0;
        return delta * ROT_SPEED;
    }

    public float GetMovementDelta()
    {
        float delta = _movementBuffer;
        _movementBuffer = 0;
        return delta * MOVE_SPEED;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rotationBuffer += eventData.delta.x;
        _movementBuffer += eventData.delta.y;
    }
}
