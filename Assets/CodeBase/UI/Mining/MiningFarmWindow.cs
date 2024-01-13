using System;
using CodeBase.Animations;
using CodeBase.Enums;
using CodeBase.Gameplay.GameItems;
using CodeBase.Services.Mining;
using CodeBase.Services.ShopItemData;
using TMPro;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace CodeBase.UI.MiningFarm
{
    public class MiningFarmWindow : WindowBase
    {
        [SerializeField] private CanvasAnimator _canvasAnimator;
        [SerializeField] private TMP_Text _temperatureText;
        [SerializeField] private TMP_Text _perMinuteProfitText;
        [SerializeField] private TMP_Text _needCleanText;
        
        private GameItemService _gameItemService;
        private MiningFarmService _miningFarmService;

        [Inject]
        private void Construct(MiningFarmService miningFarmService)
        {
            _miningFarmService = miningFarmService;
        }

        private void Start()
        {
            MiningFarmItem miningFarm = _miningFarmService.Get();
            _perMinuteProfitText.text = $"{miningFarm.ProfitPerMinute}$";
            _temperatureText.text = $"{miningFarm.TargetTemperature} C°";
            _needCleanText.text = miningFarm.NeedClean.ToString();
        }

        public override void Open()
        {
            _canvasAnimator.FadeInCanvas();
        }

        public override void Close()
        {
            _canvasAnimator.FadeOutCanvas(() => base.Close());
        }
    }
}