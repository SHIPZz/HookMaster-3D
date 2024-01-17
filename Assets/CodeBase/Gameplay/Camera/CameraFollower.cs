using UnityEngine;
using DG.Tweening;

namespace CodeBase.Gameplay.Camera
{
    public class CameraFollower : MonoBehaviour
    {
        [SerializeField] private Vector3 _offset;
        [SerializeField] private Vector3 _targetRotation = new(63, 15.2f, 0);
        [SerializeField] private float _speed = 3f;
        [SerializeField] private float _movementDuration = 1.0f;

        public Vector3 CameraPanOffset => new Vector3(0, 10f, 0);

        private Transform _target;
        private bool _isBlocked;
        private Transform _lastPosition;

        private void LateUpdate()
        {
            if (_isBlocked)
                return;

            if (_target == null)
                return;

            transform.position = _target.position + _offset * _speed;
        }

        public void Block(bool isBlocked)
        {
            _isBlocked = isBlocked;
        }

        public void MoveTo(Transform target)
        {
            transform
                .DOMove(target.position + CameraPanOffset, _movementDuration)
                .OnComplete(() =>
                {
                    transform.DOMove(_lastPosition.position + _offset * _speed, _movementDuration).OnComplete(() => _isBlocked = false);
                    transform.DORotate(_targetRotation, _movementDuration);
                });

            transform.DORotateQuaternion(Quaternion.LookRotation(target.up * -1), 0.1f);
        }

        public void SetTarget(Transform target)
        {
            _lastPosition = _target == null ? target : _target;
            
            _target = target;
        }
    }
}