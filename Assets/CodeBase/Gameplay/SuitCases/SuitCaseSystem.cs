using System;
using System.Threading;
using CodeBase.Animations;
using CodeBase.Gameplay.GameItems;
using CodeBase.Services.TriggerObserve;
using CodeBase.Services.Window;
using CodeBase.UI.SuitCase;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.SuitCases
{
    public class SuitCaseSystem : GameItemAbstract
    {
        [SerializeField] private AppearanceEffect _appearanceEffect;
        [SerializeField] private TriggerObserver _triggerObserver;
        [SerializeField] private float _openWindowDelay = 2f; // Change the delay to 2 seconds

        private WindowService _windowService;
        private CancellationTokenSource _cancellationTokenSource;

        [Inject]
        private void Construct(WindowService windowService)
        {
            _windowService = windowService;
        }

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
        }

        private async void OnPlayerEntered(Collider obj)
        {
            _cancellationTokenSource = new CancellationTokenSource();

            try
            {
                await UniTask.Delay(TimeSpan.FromSeconds(_openWindowDelay), cancellationToken: _cancellationTokenSource.Token);
            }
            catch (OperationCanceledException)
            {
                return;
            }

            _windowService.Open<SuitCaseWindow>();
        }
    }
}
