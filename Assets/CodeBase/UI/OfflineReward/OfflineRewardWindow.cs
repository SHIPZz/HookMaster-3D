using System.Collections.Generic;
using CodeBase.Animations;
using CodeBase.Data;
using CodeBase.Services.Ad;
using CodeBase.Services.Reward;
using CodeBase.Services.Window;
using CodeBase.UI.Hud;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.UI.OfflineReward
{
    public class OfflineRewardWindow : WindowBase
    {
        [SerializeField] private CanvasAnimator _canvasAnimator;
        [SerializeField] private Slider _passedTimeSlider;
        [SerializeField] private TMP_Text _rewardedMoneyText;
        [SerializeField] private AudioSource _increaseSound;
        [SerializeField] private List<TransformScaleAnim> _buttonScaleAnims;
        [SerializeField] private Button _adButton;

        private WindowService _windowService;
        private NumberTextAnimService _numberTextAnimService;
        private int _totalProfit;
        private int _timeDifference;
        private RewardService _rewardService;
        private AdService _adService;

        [Inject]
        private void Construct(WindowService windowService, 
            NumberTextAnimService numberTextAnimService,
            RewardService rewardService,
            AdService adService)
        {
            _adService = adService;
            _rewardService = rewardService;
            _numberTextAnimService = numberTextAnimService;
            _windowService = windowService;
        }

        private void OnEnable() => 
            _adButton.onClick.AddListener(AdButtonClickHandler);

        private void OnDisable() => 
            _adButton.onClick.RemoveListener(AdButtonClickHandler);

        private void AdButtonClickHandler() =>
            _adService.ShowVideo(() =>
            {
                _rewardService.Add(ItemTypeId.Money, _totalProfit * 2);
                Close();
            });

        public void Init(int totalProfit, int timeDifference)
        {
            _timeDifference = timeDifference;
            _totalProfit = totalProfit;
        }

        public override void Open() => 
            _canvasAnimator.FadeInCanvas(OnOpened);

        public override void Close() => 
            _canvasAnimator.FadeOutCanvas(OnClosed);

        private void OnClosed()
        {
            _windowService.Open<HudWindow>();
            base.Close();
        }

        private async void OnOpened()
        {
            _passedTimeSlider.DOValue(_timeDifference, 1f).SetUpdate(true);
            await _numberTextAnimService.AnimateNumber(0, _totalProfit, 1.5f, _rewardedMoneyText, '$', _increaseSound);
            _buttonScaleAnims.ForEach(x => x.ToScale());
        }
    }
}