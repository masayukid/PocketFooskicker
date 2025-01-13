using System;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public event Action<Collision> OnTouch;
    public bool IsInactive => !gameObject.activeSelf;

    private Rigidbody _rigidbody;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("Field"))
        {
            OnTouch?.Invoke(other);
        }
    }

    public void Inactivate()
    {
        gameObject.SetActive(false);
    }

    public float GetCurrentSpeed()
    {
        return _rigidbody.velocity.magnitude;
    }

    public Vector3 GetPosition()
    {
        return _rigidbody.position;
    }
}
