using System;
using CodeBase.Services.TriggerObserve;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace CodeBase.Gameplay.Door
{
    public class DoorSystem : MonoBehaviour
    {
        [SerializeField] private TriggerObserver _triggerObserver;
        [SerializeField] private float _openDuration;
        [SerializeField] private float _closeDuration;
        [SerializeField] private Vector3 _openRotation;
        [SerializeField] private Vector3 _closeRotation;
        [SerializeField] private Vector3 _playerOffset;
        [SerializeField] private float _speed = 5;
        [SerializeField] private float _closeDelay = 2f;
        [SerializeField] private float _openDistance = 0.2f;
        [SerializeField] private float _targetBlockOpenDot = -0.75f;
        [SerializeField] private Transform _targetTransform;
        [SerializeField] private AudioSource _openSound;
        [SerializeField] private AudioSource _closeSound;

        private bool _isMoving;

        private void OnEnable() =>
            _triggerObserver.CollisionEntered += OnPlayerEntered;

        private void OnDisable() =>
            _triggerObserver.CollisionEntered -= OnPlayerEntered;

        private async void OnPlayerEntered(Collision player)
        {
            if (_isMoving)
                return;

            var playerRigidBody = player.gameObject.GetComponent<Rigidbody>();
            Vector3 targetPosition = player.transform.position + _playerOffset;

            var dot = Vector3.Dot(_targetTransform.forward, player.transform.forward);

            if(dot < 0 && dot >= _targetBlockOpenDot)
                return;

            if (dot <= 0)
            {
                await PushPlayerAwayToOpen(playerRigidBody, targetPosition);
            }

            _isMoving = true;
            _openSound.Play();
            transform.DOLocalRotate(_openRotation, _openDuration).OnComplete(() => _isMoving = false);

            await UniTask.Delay(TimeSpan.FromSeconds(_closeDelay));

            _isMoving = true;
            transform.DOLocalRotate(_closeRotation, _closeDuration).OnComplete(() =>
            {
                _closeSound.Play();
                _isMoving = false;
            });
        }

        private async UniTask PushPlayerAwayToOpen(Rigidbody playerRigidBody, Vector3 targetPosition)
        {
            while (Vector3.Distance(playerRigidBody.position, targetPosition) > _openDistance)
            {
                playerRigidBody.position =
                    Vector3.Lerp(playerRigidBody.position, targetPosition, _speed * Time.fixedDeltaTime);
                await UniTask.WaitForFixedUpdate();
            }
        }
    }
}