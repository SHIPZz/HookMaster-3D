using System;
using CodeBase.Services.Providers.Player;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using Zenject;

namespace CodeBase.Gameplay.Camera
{
    public class CameraFollower : MonoBehaviour
    {
        [SerializeField] private Vector3 _offset;
        [SerializeField] private Vector3 _targetRotation = new(63, 15.2f, 0);
        [SerializeField] private float _speed = 3f;
        [SerializeField] private float _movementDuration = 1.0f;
        [SerializeField] private float _stopOffset = 0.5f;

        public Vector3 CameraPanOffset => new Vector3(0, 10f, 0);

        private Transform _target;
        private bool _isBlocked;
        private Transform _lastPosition;
        private PlayerProvider _playerProvider;
        private Vector3 _lastRotation;

        [Inject]
        private void Construct(PlayerProvider playerProvider)
        {
            _playerProvider = playerProvider;
        }

        private void LateUpdate()
        {
            if (_isBlocked)
                return;

            if (_playerProvider.Player == null)
                return;

            transform.position = _playerProvider.Player.transform.position + _offset * _speed;
        }

        public void Block(bool isBlocked)
        {
            _isBlocked = isBlocked;
        }

        public void MoveTo(Transform target, Action onComplete = null)
        {
            _target = target;
            Vector3 targetPosition = target.position - target.forward * _stopOffset;

            _lastRotation = transform.eulerAngles;
            transform.DODynamicLookAt(_target.position, _movementDuration);
            transform
                .DOMove(targetPosition + CameraPanOffset, _movementDuration)
                .OnComplete(() => MoveAndRotateBack(onComplete));
        }

        private void MoveAndRotateBack(Action onComplete)
        {
            transform.DORotate(_lastRotation, _movementDuration);
            transform
                .DOMove(_lastPosition.position + _offset * _speed, _movementDuration)
                .OnComplete(() =>
                {
                    onComplete?.Invoke();
                    _isBlocked = false;
                });
        }

        public void SetTarget(Transform target)
        {
            _lastPosition = _target == null ? target : _target;

            _target = target;
        }
    }
}