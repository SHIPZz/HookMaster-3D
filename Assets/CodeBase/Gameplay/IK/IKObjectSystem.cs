using System;
using CodeBase.Services.TriggerObserve;
using UnityEngine;

public class IKObjectSystem : MonoBehaviour
{
    [field: SerializeField] public Transform RightHandIK { get; private set; }
    [field: SerializeField] public Transform LeftHandIK { get; private set; }

    [SerializeField] private TriggerObserver _triggerObserver;
    [SerializeField] private Vector3 _targetPosition;

    public event Action PlayerTaken;

    private bool _isTaken;
    
    private void OnEnable()
    {
        _triggerObserver.CollisionEntered += OnPlayerEntered;
    }

    private void OnDisable()
    {
        _triggerObserver.CollisionEntered -= OnPlayerEntered;
    }

    private void OnPlayerEntered(Collision player)
    {
        if (_isTaken)
            return;
        
        transform.SetParent(player.transform,true);
        transform.localPosition = _targetPosition;
        _isTaken = true;
        PlayerTaken?.Invoke();
    }
}
