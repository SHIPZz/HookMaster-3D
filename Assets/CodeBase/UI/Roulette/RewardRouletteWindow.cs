using System.Collections.Generic;
using CodeBase.Animations;
using CodeBase.Data;
using CodeBase.Gameplay.SoundPlayer;
using CodeBase.Services.Reward;
using CodeBase.Services.Window;
using CodeBase.UI.Hud;
using TMPro;
using UnityEngine;
using Zenject;

namespace CodeBase.UI.Roulette
{
    public class RewardRouletteWindow : WindowBase
    {
        [SerializeField] private CanvasAnimator _canvasAnimator;
        [SerializeField] private TMP_Text _moneyText;
        [SerializeField] private TMP_Text _ticketText;
        [SerializeField] private TMP_Text _diamondText;
        [SerializeField] private AppearanceEffect _appearanceEffect;
        [SerializeField] private SoundPlayerSystem _soundPlayer;
        [SerializeField] private TransformScaleAnim _buttonScaleAnim;
        [SerializeField] private AudioSource _increaseSound;

        private RewardService _rewardService;
        private WindowService _windowService;
        private NumberTextAnimService _numberTextAnimService;
        private IReadOnlyDictionary<ItemTypeId, int> _rouletteRewards;

        [Inject]
        private void Construct(RewardService rewardService, WindowService windowService, NumberTextAnimService numberTextAnimService)
        {
            _numberTextAnimService = numberTextAnimService;
            _windowService = windowService;
            _rewardService = rewardService;
        }

        public override void Open()
        {
            _canvasAnimator.FadeInCanvas(OnOpened);

            _rouletteRewards = _rewardService.RouletteRewards;
        }

        public override void Close()
        {
            _canvasAnimator.FadeOutCanvas(() =>
            {
                _windowService.Open<HudWindow>();
                base.Close();
            });
        }

        private async void OnOpened()
        {           
            _appearanceEffect.PlayTargetEffects();
            _soundPlayer.PlayActiveSound();
            await _numberTextAnimService.AnimateNumber(0, _rouletteRewards[ItemTypeId.Money], 1.5f, _moneyText, '$', _increaseSound);
            await _numberTextAnimService.AnimateNumber(0, _rouletteRewards[ItemTypeId.Ticket], 1.5f, _ticketText,  _increaseSound,true);
            await _numberTextAnimService.AnimateNumber(0, _rouletteRewards[ItemTypeId.Diamond], 1.5f, _diamondText,  _increaseSound,true);
            _buttonScaleAnim.ToScaleAsync();
            
        }
    }
}