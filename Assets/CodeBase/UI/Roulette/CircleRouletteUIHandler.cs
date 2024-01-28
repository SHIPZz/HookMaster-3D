using System;
using CodeBase.Animations;
using CodeBase.Gameplay.GameItems;
using CodeBase.Services.CircleRouletteServices;
using CodeBase.Services.Player;
using CodeBase.Services.TriggerObserve;
using CodeBase.UI.Buttons;
using UnityEngine;
using Zenject;

namespace CodeBase.UI.Roulette
{
    public class CircleRouletteUIHandler : MonoBehaviour
    {
        [SerializeField] private TriggerObserver _triggerObserver;
        [SerializeField] private ImageFadeAnim _rouletteImageFadeAnim;
        [SerializeField] private TransformScaleAnim _buttonScaleAnim;
        [SerializeField] private OpenCircleRouletteWindowButton _openCircleRouletteWindowButton;
        [SerializeField] private CircleRouletteItem _circleRouletteItem;

        private CircleRouletteService _circleRouletteService;

        [Inject]
        private void Construct(CircleRouletteService circleRouletteService)
        {
            _circleRouletteService = circleRouletteService;
        }

        private void OnEnable()
        {
            _triggerObserver.TriggerEntered += OnPlayerApproached;
            _triggerObserver.TriggerExited += OnPlayerExited;
            _openCircleRouletteWindowButton.onClick.AddListener(OnPlayClicked);
        }

        private void Start()
        {
            _buttonScaleAnim.UnScale();
            
            if (!_circleRouletteService.CanPlay(_circleRouletteItem.Id))
            {
                _rouletteImageFadeAnim.FadeOut();
            }
        }

        private void OnDisable()
        {
            _triggerObserver.TriggerEntered -= OnPlayerApproached;
            _triggerObserver.TriggerExited -= OnPlayerExited;
        }

        private void OnPlayerExited(Collider obj)
        {
            if(!_circleRouletteService.CanPlay(_circleRouletteItem.Id))
                return;

            _rouletteImageFadeAnim.FadeIn();
            _buttonScaleAnim.UnScale();
        }

        private void OnPlayerApproached(Collider obj)
        {
            if(!_circleRouletteService.CanPlay(_circleRouletteItem.Id))
                return;

            _rouletteImageFadeAnim.FadeOut();
            _buttonScaleAnim.ToScale();
        }

        private void OnPlayClicked()
        {
            _rouletteImageFadeAnim.FadeOut();
            _buttonScaleAnim.UnScale();
        }
    }
}