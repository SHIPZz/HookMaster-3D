using System;
using CodeBase.Animations;
using CodeBase.Services.TriggerObserve;
using CodeBase.Services.UI;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.Gameplay.BurnableObjectSystem
{
    public class BurnableObjectUIHandler : SerializedMonoBehaviour
    {
        [OdinSerialize] private IBurnable _burnable;
        [SerializeField] private Button _recoverButton;
        [SerializeField] private CanvasAnimator _canvasAnimator;
        [SerializeField] private TriggerObserver _triggerObserver;
        [SerializeField] private float _additionalAnchoredPosY = 2.5f;
        [SerializeField] private float _duration = 0.3f;
        
        private FloatingButtonService _floatingButtonService;

        [Inject]
        private void Construct(FloatingButtonService floatingButtonService)
        {
            _floatingButtonService = floatingButtonService;
        }
        
        private void OnEnable()
        {
            _triggerObserver.TriggerEntered += OnPlayerEntered;
            _triggerObserver.TriggerExited += OnPlayerExited;
            _recoverButton.onClick.AddListener(_burnable.Recover);
        }
        private void OnDisable()
        {
            _triggerObserver.TriggerEntered -= OnPlayerEntered;
            _triggerObserver.TriggerExited -= OnPlayerExited;
            _recoverButton.onClick.RemoveListener(_burnable.Recover);
        }

        private void OnPlayerExited(Collider obj)
        {
            if(!_burnable.IsBurned)
                return;
            
            _canvasAnimator.FadeOutCanvas();
            _floatingButtonService.ShowFloatingButton(-_additionalAnchoredPosY, _duration, Quaternion.identity, false,
                _recoverButton);
        }

        private void OnPlayerEntered(Collider obj)
        {
            if(!_burnable.IsBurned)
                return;
            
            _canvasAnimator.FadeInCanvas();
            _floatingButtonService.ShowFloatingButton(_additionalAnchoredPosY, _duration, Quaternion.identity, false,
                _recoverButton);
        }
    }
}