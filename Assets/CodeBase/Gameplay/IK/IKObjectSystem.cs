using System;
using CodeBase.Gameplay.PlayerSystem;
using CodeBase.Services.TriggerObserve;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.IK
{
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
        private void Construct(PlayerIKService playerIKService)
        {
            _playerIKService = playerIKService;
        }

        private void OnEnable()
        {
            _triggerObserver.CollisionEntered += OnPlayerEntered;
            _triggerObserver.TriggerEntered += OnPlayerEnteredTrigger;
        }

        private void OnDisable()
        {
            _triggerObserver.CollisionEntered -= OnPlayerEntered;
            _triggerObserver.TriggerEntered -= OnPlayerEnteredTrigger;
            _playerIKService.ClearIKHandTargets().Forget();
        }

        private void OnPlayerEnteredTrigger(Collider player)
        {
            if (_isTaken || _playerIKService.HasItemInHands)
                return;

            Activate(player.transform);
        }

        protected virtual void OnPlayerEntered(Collision player)
        {
            if (_isTaken || _playerIKService.HasItemInHands)
                return;

            Activate(player.transform);
        }

        private void Activate(Transform player)
        {
            transform.SetParent(player.transform, true);
            transform.localPosition = _targetPosition;
            transform.localEulerAngles = _targetRotation;
            _playerIKService.SetIKHandTargets(LeftHandIK, RightHandIK);

            _isTaken = true;
            PlayerTaken?.Invoke();
        }
    }
}