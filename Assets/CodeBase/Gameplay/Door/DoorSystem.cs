using System;
using CodeBase.Services.GOPush;
using CodeBase.Services.TriggerObserve;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.Door
{
    public class DoorSystem : MonoBehaviour
    {
        [SerializeField] private TriggerObserver _triggerObserver;
        [SerializeField] private float _openDuration = 0.5f;
        [SerializeField] private float _closeDuration = 0.5f;
        [SerializeField] private Vector3 _openRotation = new Vector3(0, 159.6f, 0);
        [SerializeField] private Vector3 _closeRotation;
        [SerializeField] private Vector3 _playerOffset = new Vector3(0,0, 1.5f);
        [SerializeField] private float _speed = 5;
        [SerializeField] private float _closeDelay = 2f;
        [SerializeField] private float _openDistance = 0.2f;
        [SerializeField] private float _targetBlockOpenDot = -0.75f;
        [SerializeField] private Transform _targetTransform;
        [SerializeField] private SoundPlayerSystem _soundPlayerSystem;

        private bool _isMoving;
        private GameObjectPushService _gameObjectPushService;

        [Inject]
        private void Construct(GameObjectPushService gameObjectPushService)
        {
            _gameObjectPushService = gameObjectPushService;
        }
        
        private void OnEnable() =>
            _triggerObserver.CollisionEntered += OnPlayerEntered;

        private void OnDisable() =>
            _triggerObserver.CollisionEntered -= OnPlayerEntered;

        private async void OnPlayerEntered(Collision player)
        {
            if(transform.localEulerAngles != _closeRotation)
                return;
            
            if (_isMoving)
                return;

            var playerRigidBody = player.gameObject.GetComponent<Rigidbody>();
            Vector3 targetPosition = player.transform.position + _playerOffset;

            var dot = Vector3.Dot(_targetTransform.forward, player.transform.forward);

            if(dot < 0 && dot >= _targetBlockOpenDot)
                return;

            if (dot <= 0)
            {
                await _gameObjectPushService.PushRigidBodyAwayAsync(playerRigidBody, targetPosition, _openDistance, _speed);
            }

            _isMoving = true;
            _soundPlayerSystem.PlayActiveSound();
            transform.DOLocalRotate(_openRotation, _openDuration).OnComplete(() => _isMoving = false);

            await UniTask.Delay(TimeSpan.FromSeconds(_closeDelay));

            _isMoving = true;
            transform.DOLocalRotate(_closeRotation, _closeDuration).OnComplete(() =>
            {
                _soundPlayerSystem.PlayInactiveClip();
                _isMoving = false;
            });
        }
    }
}