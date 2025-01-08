using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class SensitivitySliderController : MonoBehaviour
{
    [SerializeField] private Text _currentValueText; // ハンドルの上の現在値テキスト

    private Slider _sensitivitySlider;

    private void Awake()
    {
        _sensitivitySlider = GetComponent<Slider>();
    }

    private void Start()
    {
        // 初期値をテキストに反映
        UpdateCurrentValue(_sensitivitySlider.value);

        // スライダーの値が変更されたときに呼び出すイベントを登録
        _sensitivitySlider.onValueChanged.AddListener(UpdateCurrentValue);
    }

    private void UpdateCurrentValue(float value)
    {
        // 現在値を更新
        _currentValueText.text = value.ToString("0.0");

        // ハンドル位置に合わせてテキスト位置を更新
        var handlePosition = _sensitivitySlider.handleRect.transform.position;
        _currentValueText.transform.position = new Vector3(handlePosition.x, handlePosition.y + 30, handlePosition.z);
    }

    private void OnDestroy()
    {
        // イベントを解除
        _sensitivitySlider.onValueChanged.RemoveListener(UpdateCurrentValue);
    }
}