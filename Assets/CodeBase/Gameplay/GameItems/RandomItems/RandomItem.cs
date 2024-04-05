using System;
using CodeBase.Animations;
using CodeBase.Services.TriggerObserve;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CodeBase.Gameplay.GameItems.RandomItems
{
    public class RandomItem : GameItemAbstract
    {
        [SerializeField] private AppearanceEffect _appearanceEffect;
        [SerializeField] private TriggerObserver _triggerObserver;
        [field: SerializeField] public Vector3 SpawnOffset { get; private set; }
        
        private bool _playerExited = true;
        
        public event Action PlayerApproached;
        public event Action PlayerExited;

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

        private void OnPlayerExited(Collider player)
        {
            _playerExited = true;
            PlayerExited?.Invoke();
        }

        private async void OnPlayerEntered(Collider player)
        {
            if(!_playerExited)
                return;

            _playerExited = false;
            var playerRigidBody = player.GetComponent<Rigidbody>();

            await UniTask.WaitUntil(() => playerRigidBody.velocity.sqrMagnitude > 0.1f);
            
            PlayerApproached?.Invoke();
        }
    }
}