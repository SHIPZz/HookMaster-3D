using System;
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
        [SerializeField] private float _minTemperature = 65;
        [SerializeField] private float _maxTemperature = 100;
        [SerializeField] private float _midTemperature = 75;
        private ShopItemService _shopItemService;

        [Inject]
        private void Construct(ShopItemService shopItemService)
        {
            _shopItemService = shopItemService;
        }

        private void Start()
        {
            var miningFarm = _shopItemService.Get<Shop.MiningFarm>(ShopItemTypeId.MiningFarm);
            _perMinuteProfitText.text = $"{miningFarm.ProfitPerMinute}$";
            _temperatureText.text = $"{miningFarm.TargetTemperature} C°";
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