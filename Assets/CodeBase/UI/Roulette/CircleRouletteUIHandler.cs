using System;
using CodeBase.Animations;
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
        [SerializeField] private RectTransformScaleAnim _buttonScaleAnim;
        [SerializeField] private PlayCircleRouletteButton _playCircleRouletteButton;

        private PlayerRewardService _playerRewardService;

        [Inject]
        private void Construct(PlayerRewardService playerRewardService)
        {
            _playerRewardService = playerRewardService;
        }

        private void OnEnable()
        {
            _triggerObserver.TriggerEntered += OnPlayerApproached;
            _triggerObserver.TriggerExited += OnPlayerExited;
            _playCircleRouletteButton.onClick.AddListener(OnPlayClicked);
        }

        private void Start()
        {
            if (!_playerRewardService.CanPlayRouletteCircle)
            {
                _buttonScaleAnim.UnScale();
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
            if (!_playerRewardService.CanPlayRouletteCircle)
                return;

            _rouletteImageFadeAnim.FadeIn();
            _buttonScaleAnim.UnScale();
        }

        private void OnPlayerApproached(Collider obj)
        {
            if (!_playerRewardService.CanPlayRouletteCircle)
                return;

            _rouletteImageFadeAnim.FadeOut();
            _buttonScaleAnim.ToScale();
        }

        private void OnPlayClicked()
        {
            _rouletteImageFadeAnim.FadeOut();
            _buttonScaleAnim.UnScale();
            _playerRewardService.SetCanPlayRoulette(false);
        }
    }
}