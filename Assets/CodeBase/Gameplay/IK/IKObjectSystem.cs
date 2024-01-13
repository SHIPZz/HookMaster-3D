using System;
using CodeBase.Gameplay.PlayerSystem;
using CodeBase.Services.TriggerObserve;
using UnityEngine;
using Zenject;

public class IKObjectSystem : MonoBehaviour
{
    [field: SerializeField] public Transform RightHandIK { get; private set; }
    [field: SerializeField] public Transform LeftHandIK { get; private set; }

    [SerializeField] private TriggerObserver _triggerObserver;
    [SerializeField] private Vector3 _targetPosition;
    [SerializeField] private Vector3 _targetRotation;

    public event Action PlayerTaken;

    private bool _isTaken;
    private PlayerIKService _playerIKService;

    [Inject]
    private void Construct(PlayerIKService playerIKService) => 
        _playerIKService = playerIKService;

    private void OnEnable()
    {
        _triggerObserver.CollisionEntered += OnPlayerEntered;
    }

    private void OnDisable()
    {
        _triggerObserver.CollisionEntered -= OnPlayerEntered;
        _playerIKService.ClearIKHandTargets();
    }

    private void OnPlayerEntered(Collision player)
    {
        if (_isTaken)
            return;
        
        transform.SetParent(player.transform,true);
        transform.localPosition = _targetPosition;
        transform.localEulerAngles = _targetRotation;
        _playerIKService.SetIKHandTargets(LeftHandIK, RightHandIK);
        
        _isTaken = true;
        PlayerTaken?.Invoke();
    }
}
