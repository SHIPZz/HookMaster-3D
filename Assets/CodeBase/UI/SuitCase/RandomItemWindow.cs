using System;
using CodeBase.Animations;
using CodeBase.Data;
using CodeBase.Enums;
using CodeBase.Gameplay.SoundPlayer;
using CodeBase.Services.Ad;
using CodeBase.Services.RandomItems;
using CodeBase.Services.Reward;
using CodeBase.Services.Wallet;
using CodeBase.Services.Window;
using CodeBase.SO.GameItem.RandomItems;
using CodeBase.UI.Hud;
using I2.Loc;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.UI.SuitCase
{
    public class RandomItemWindow : WindowBase
    {
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _profit;
        [SerializeField] private Image _image;
        [SerializeField] private CanvasAnimator _canvasAnimator;
        [SerializeField] private SoundPlayerSystem _soundPlayerSystem;
        [SerializeField] private Button _adButton;
        [SerializeField] private Localize _localize;

        private WindowService _windowService;
        private AdService _adService;
        private RandomItemService _randomItemService;
        private RewardService _rewardService;
        private RandomItemSO _randomItemSo;

        [Inject]
        private void Construct(WindowService windowService, 
            AdService adService, 
            RandomItemService randomItemService,
            RewardService rewardService)
        {
            _rewardService = rewardService;
            _randomItemService = randomItemService;
            _adService = adService;
            _windowService = windowService;
        }

        private void OnEnable() => 
            _adButton.onClick.AddListener(InvokeAdService);

        private void OnDisable() => 
            _adButton.onClick.RemoveListener(InvokeAdService);

        private void InvokeAdService()
        {
            _adService.ShowVideo(() =>
            {
                _randomItemService.DestroyItem(_randomItemSo.GameItemType);
                _rewardService.Add(ItemTypeId.Money,_randomItemSo.Profit);
                Close();
            });
        }

        public override void Open()
        {
            _windowService.Close<HudWindow>();
            _soundPlayerSystem.PlayActiveSound();
            _canvasAnimator.FadeInCanvas();
        }

        public void Init(RandomItemSO randomItemSo)
        {
            _randomItemSo = randomItemSo;
            _name.text =  _randomItemSo.Name;
            _profit.text = $"{randomItemSo.Profit}$";
            _image.rectTransform.anchoredPosition = _randomItemSo.IconPosition;
            _image.sprite = _randomItemSo.Icon;
            _localize.OnLocalize(true);
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