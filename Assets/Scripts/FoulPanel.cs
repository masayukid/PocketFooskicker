using System;
using UnityEngine;

public class FoulPanel : MonoBehaviour
{
    private Action _onClose;

    public void Open(Action onClose)
    {
        if (gameObject.activeSelf)
        {
            throw new Exception("FoulPanelは既に開いています。");
        }

        gameObject.SetActive(true);
        _onClose = onClose;
    }

    public void Close()
    {
        _onClose?.Invoke();
        _onClose = null;
        gameObject.SetActive(false);
    }
}
