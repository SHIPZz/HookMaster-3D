using System;
using System.Threading.Tasks;
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

        private Tween _tween;

        private void OnEnable() =>
            _triggerObserver.CollisionEntered += OnPlayerEntered;

        private void OnDisable() =>
            _triggerObserver.CollisionEntered -= OnPlayerEntered;

        private async void OnPlayerEntered(Collision player)
        {
            var playerRigidBody = player.gameObject.GetComponent<Rigidbody>();
            Vector3 targetPosition = player.transform.position + _playerOffset;

            var dot = Vector3.Dot(transform.forward, player.transform.forward);

            if (Math.Abs(dot - -1) < 0.1)
            {
                await PushPlayerAwayToOpen(playerRigidBody, targetPosition);
            }

            _tween?.Kill(true);
            _tween = transform.DOLocalRotate(_openRotation, _openDuration);

            await UniTask.Delay(TimeSpan.FromSeconds(_closeDelay));

            _tween?.Kill(true);
            _tween = transform.DOLocalRotate(_closeRotation, _closeDuration);
        }

        private async UniTask PushPlayerAwayToOpen(Rigidbody playerRigidBody, Vector3 targetPosition)
        {
            while (Vector3.Distance(playerRigidBody.position, targetPosition) > 0.5f)
            {
                playerRigidBody.position =
                    Vector3.Lerp(playerRigidBody.position, targetPosition, _speed * Time.fixedDeltaTime);
                await UniTask.WaitForFixedUpdate();
            }
        }
    }
}