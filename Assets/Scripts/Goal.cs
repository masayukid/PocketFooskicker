using System;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField] private bool _isSelf;

    public bool IsSelf => _isSelf;
    public event Action<Goal> OnGoal;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            OnGoal?.Invoke(this);
        }
    }
}