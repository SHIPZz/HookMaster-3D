using System.Collections.Generic;
using CodeBase.Data;
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

        private RewardService _rewardService;
        private WindowService _windowService;

        [Inject]
        private void Construct(RewardService rewardService, WindowService windowService)
        {
            _windowService = windowService;
            _rewardService = rewardService;
        }

        public override void Open()
        {
            _canvasAnimator.FadeInCanvas();

            IReadOnlyDictionary<ItemTypeId, int> rouletteRewards = _rewardService.RouletteRewards;

            _moneyText.text = $"{rouletteRewards[ItemTypeId.Money]}$";
            _ticketText.text = $"{rouletteRewards[ItemTypeId.Ticket]}";
            _diamondText.text = $"{rouletteRewards[ItemTypeId.Diamond]}";
        }

        public override void Close()
        {
            _canvasAnimator.FadeOutCanvas(() =>
            {
                _windowService.Open<HudWindow>();
                base.Close();
            });
        }
    }
}