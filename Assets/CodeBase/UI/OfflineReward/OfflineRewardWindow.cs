using CodeBase.Animations;
using CodeBase.Services.Time;
using CodeBase.Services.Window;
using CodeBase.UI.Hud;
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
        private WindowService _windowService;

        [Inject]
        private void Construct(WindowService windowService)
        {
            _windowService = windowService;
        }

        public void Init(float totalProfit, int timeDifference)
        {
            _rewardedMoneyText.text = $"{totalProfit}$";
            _passedTimeSlider.value = timeDifference;
            print(timeDifference + " time");
        }

        public override void Open()
        {
            _canvasAnimator.FadeInCanvas();
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