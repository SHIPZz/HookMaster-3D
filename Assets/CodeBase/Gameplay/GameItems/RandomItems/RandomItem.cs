using System;
using System.Threading;
using CodeBase.Animations;
using CodeBase.Services.TriggerObserve;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CodeBase.Gameplay.GameItems
{
    public class RandomItem : GameItemAbstract
    {
        [SerializeField] private AppearanceEffect _appearanceEffect;
        [SerializeField] private TriggerObserver _triggerObserver;
        [field: SerializeField] public Vector3 SpawnOffset { get; private set; }

        private CancellationTokenSource _cancellationTokenSource;

        public event Action PlayerApproached;

        private void OnEnable()
        {
            _appearanceEffect.PlayAppearEffect();
            _appearanceEffect.PlayLoopEffects();
            _triggerObserver.TriggerEntered += OnPlayerEntered;
            _triggerObserver.TriggerExited += OnPlayerExited;
        }

        private void OnDisable()
        {
            _triggerObserver.TriggerEntered -= OnPlayerEntered;
            _triggerObserver.TriggerExited -= OnPlayerExited;
        }

        private void OnPlayerExited(Collider obj)
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
        }

        private async void OnPlayerEntered(Collider obj)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            var playerRigidBody = obj.GetComponent<Rigidbody>();

            try
            {
                while (playerRigidBody.velocity.sqrMagnitude > 0.1f)
                {
                    await UniTask.Yield(_cancellationTokenSource.Token);
                }
            }
            catch (OperationCanceledException)
            {
                return;
            }
            
            PlayerApproached?.Invoke();
        }
    }
}