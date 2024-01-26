using System;
using CodeBase.Services.Providers.Player;
using UnityEngine;
using DG.Tweening;
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
        [SerializeField] private Vector3 _cameraPanOffset = new(0, 10f, 0);

        public event Action Moved;
        
        private Transform _target;
        private bool _isBlocked;
        private Transform _lastPosition;
        private PlayerProvider _playerProvider;
        private Vector3 _lastRotation;
        private Vector3 _lastOffset;

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

            transform.position = Vector3.Lerp(transform.position, _playerProvider.Player.transform.position + _offset,
                _speed * Time.deltaTime);
        }

        private void Awake()
        {
            _lastOffset = _offset;
        }

        public void SetLastOffset()
        {
            _offset = _lastOffset;
            // transform.DOMove(_playerProvider.Player.transform.position + _offset * _speed, 1f);
        }

        public void SetTargetOffset(Vector3 offset)
        {
            _offset += offset;
            // transform.DOMove(_playerProvider.Player.transform.position + _offset * _speed, 1f);
        }

        public void Block(bool isBlocked)
        {
            _isBlocked = isBlocked;
        }
        
        public void MoveTo(Transform target, float movementBackDelay, Action onComplete = null)
        {
            _target = target;
            Vector3 targetPosition = target.position - target.forward * _stopOffset;

            _lastRotation = transform.eulerAngles;
            transform.DODynamicLookAt(_target.position, _movementDuration);
            
            Moved?.Invoke();
            
            transform
                .DOMove(targetPosition + _cameraPanOffset, _movementDuration)
                .OnComplete(() => DOTween.Sequence()
                    .AppendInterval(movementBackDelay)
                    .OnComplete(() => MoveAndRotateBack(onComplete)));
        }

        public void MoveTo(Transform target, Action onComplete = null)
        {
            _target = target;
            Vector3 targetPosition = target.position - target.forward * _stopOffset;

            _lastRotation = transform.eulerAngles;
            Moved?.Invoke();
            transform.DODynamicLookAt(_target.position, _movementDuration);
            transform
                .DOMove(targetPosition + _cameraPanOffset, _movementDuration)
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