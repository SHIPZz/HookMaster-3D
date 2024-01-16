using System;
using System.Collections.Generic;
using CodeBase.Animations;
using CodeBase.Gameplay.GameItems;
using CodeBase.Services.TriggerObserve;
using CodeBase.Services.Window;
using CodeBase.UI.SuitCase;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.SuitCases
{
    public class SuitCaseSystem : GameItemAbstract
    {
        [SerializeField] private AppearanceEffect _appearanceEffect;
        [SerializeField] private TriggerObserver _triggerObserver;
        private WindowService _windowService;

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
        }

        private void OnDisable()
        {
            _triggerObserver.TriggerEntered -= OnPlayerEntered;
        }

        private void OnPlayerEntered(Collider obj)
        {
            _windowService.Open<SuitCaseWindow>();
        }
    }
}