using CodeBase.Animations;
using CodeBase.Gameplay.GameItems;
using CodeBase.Services.GameItemServices;
using CodeBase.Services.Mining;
using TMPro;
using UnityEngine;
using Zenject;

namespace CodeBase.UI.Mining
{
    public class MiningFarmWindow : WindowBase
    {
        [SerializeField] private CanvasAnimator _canvasAnimator;
        [SerializeField] private TMP_Text _temperatureText;
        [SerializeField] private TMP_Text _perMinuteProfitText;
        [SerializeField] private TMP_Text _needCleanText;
        [SerializeField] private GameObject _cleanButtonContainer;
        [SerializeField] private CleanMiningFarmButton _cleanMiningFarmButton;
        
        private GameItemService _gameItemService;
        private MiningFarmService _miningFarmService;
        private MiningFarmItem _miningFarmItem;

        [Inject]
        private void Construct(MiningFarmService miningFarmService)
        {
            _miningFarmService = miningFarmService;
        }

        public override void Open()
        {
            _canvasAnimator.FadeInCanvas();
            _perMinuteProfitText.text = $"{_miningFarmItem.ProfitPerMinute}$";
            _temperatureText.text = $"{_miningFarmItem.TargetTemperature} C°";
            _needCleanText.text = _miningFarmItem.NeedClean.ToString();

            if (_miningFarmItem.NeedClean) 
                _cleanButtonContainer.SetActive(true);
        }

        public override void Close()
        {
            _miningFarmItem.Changed -= OnConditionChanged;
            _canvasAnimator.FadeOutCanvas(() => base.Close());
        }

        public void Init(MiningFarmItem miningFarmItem)
        {
            _miningFarmItem = miningFarmItem;
            _cleanMiningFarmButton.SetMiningFarm(_miningFarmItem);
            _miningFarmItem.Changed += OnConditionChanged;
        }

        private void OnConditionChanged(float temperature, bool needClean)
        {
            _temperatureText.text = $"{temperature} C°";
            _needCleanText.text = needClean.ToString();
        }
    }
}