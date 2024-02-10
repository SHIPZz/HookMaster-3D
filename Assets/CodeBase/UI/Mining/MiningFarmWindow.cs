using CodeBase.Animations;
using CodeBase.Gameplay.GameItems;
using CodeBase.Services.GameItemServices;
using TMPro;
using UnityEngine;

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
        private MiningFarmItem _miningFarmItem;

        public override void Open()
        {
            _canvasAnimator.FadeInCanvas();
            _perMinuteProfitText.text = $"{_miningFarmItem.ProfitPerMinute}$";
            _temperatureText.text = $"{_miningFarmItem.TargetTemperature} C°";

            TrySetNeedClean();
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

            TrySetNeedClean();
        }

        private void TrySetNeedClean()
        {
            string needCleanText = "-";

            if (_miningFarmItem.NeedClean)
            {
                needCleanText = "+";
                SetUI(true, needCleanText);
                return;
            }
            
            SetUI(false, needCleanText);
        }

        private void SetUI(bool isButtonActive, string needCleanText)
        {
            _cleanButtonContainer.SetActive(isButtonActive);
            _needCleanText.text = needCleanText;
        }
    }
}