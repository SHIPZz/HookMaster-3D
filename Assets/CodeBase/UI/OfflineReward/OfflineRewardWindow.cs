using System.Collections.Generic;
using CodeBase.Animations;
using CodeBase.Services.Time;
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
        [SerializeField] private List<RectTransformScaleAnim> _buttonScaleAnims;
        
        private WindowService _windowService;
        private NumberTextAnimService _numberTextAnimService;
        private int _totalProfit;
        private int _timeDifference;

        [Inject]
        private void Construct(WindowService windowService, NumberTextAnimService numberTextAnimService)
        {
            _numberTextAnimService = numberTextAnimService;
            _windowService = windowService;
        }

        public void Init(int totalProfit, int timeDifference)
        {
            _timeDifference = timeDifference;
            _totalProfit = totalProfit;
            print(timeDifference + " time");
        }

        public override void Open()
        {
            _canvasAnimator.FadeInCanvas(OnOpened);
        }

        private async void OnOpened()
        {
            _passedTimeSlider.DOValue(_timeDifference, 1f);
            await _numberTextAnimService.AnimateNumber(0, _totalProfit, 1.5f, _rewardedMoneyText, '$', _increaseSound);
            _buttonScaleAnims.ForEach(x=>x.ToScale());
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