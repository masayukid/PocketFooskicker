using System;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField] private bool _isSelf;
    [SerializeField] private GameObject _goalParticle;
    [SerializeField] private AudioSource _seGoal;

    public bool IsSelf => _isSelf;
    public event Action<Goal> OnGoal;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            _goalParticle.SetActive(true);
            _seGoal.Play();
            OnGoal?.Invoke(this);
        }
    }
}