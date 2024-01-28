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
        private PlayerProvider _playerProvider;
        private Vector3 _lastRotation;
        private Vector3 _lastOffset;
        private Vector3 _lastPos;
        private Vector3 _currentVelocity;

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

            transform.position = Vector3.Slerp(transform.position, _playerProvider.Player.transform.position + _offset,
                _speed * Time.deltaTime);
        }

        private void Awake()
        {
            _lastOffset = _offset;
        }

        public void SetLastOffset()
        {
            _offset = _lastOffset;
        }

        public void SetTargetOffset(Vector3 offset)
        {
            _offset += offset;
        }

        public void Block(bool isBlocked)
        {
            _isBlocked = isBlocked;
        }
        
        public void MoveTo(Transform target, float movementBackDelay, Action onComplete = null)
        {
            _target = target;
            _isBlocked = true;
            Vector3 targetPosition = target.position - target.forward * _stopOffset;
            _lastPos = transform.position;

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
            _isBlocked = true;
            _target = target;
            _lastPos = transform.position;
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
                .DOMove(_lastPos, _movementDuration)
                .OnComplete(() =>
                {
                    onComplete?.Invoke();
                    _isBlocked = false;
                });
        }

        public void SetTarget(Transform target)
        {
            _target = target;
        }
    }
}