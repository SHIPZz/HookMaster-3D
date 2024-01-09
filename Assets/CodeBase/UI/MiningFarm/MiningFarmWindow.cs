using System;
using CodeBase.Animations;
using CodeBase.Enums;
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
        
        private ShopItemService _shopItemService;

        [Inject]
        private void Construct(ShopItemService shopItemService)
        {
            _shopItemService = shopItemService;
        }

        private void Start()
        {
            var miningFarm = _shopItemService.Get<Gameplay.ShopItemSystem.MiningFarm>(ShopItemTypeId.MiningFarm);
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