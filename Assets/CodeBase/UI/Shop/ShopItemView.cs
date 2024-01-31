using System;
using CodeBase.Data;
using CodeBase.Enums;
using CodeBase.Gameplay.Wallet;
using CodeBase.Services.ShopItemDataServices;
using CodeBase.Services.UI;
using CodeBase.Services.Wallet;
using CodeBase.Services.Window;
using CodeBase.UI.FloatingText;
using CodeBase.UI.Hud;
using CodeBase.UI.PopupWindows;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.UI.Shop
{
    public class ShopItemView : MonoBehaviour
    {
        [field: SerializeField] public ItemTypeId ItemTypeId { get; private set; }
        [field: SerializeField] public GameItemType GameItemType { get; private set; }
        [field: SerializeField] public int Price { get; private set; }

        [SerializeField] private TMP_Text _priceText;
        [SerializeField] private Button _buyButton;

        private WalletService _walletService;
        private FloatingTextService _floatingTextService;
        private ShopItemDataService _shopItemDataService;
        private WindowService _windowService;

        [Inject]
        private void Construct(WalletService walletService,
            FloatingTextService floatingTextService,
            ShopItemDataService shopItemDataService, WindowService windowService)
        {
            _windowService = windowService;
            _shopItemDataService = shopItemDataService;
            _floatingTextService = floatingTextService;
            _walletService = walletService;
        }

        private void OnEnable()
        {
            _priceText.text = $"{Price}$";
            _buyButton.onClick.AddListener(OnBuyButtonClicked);
        }

        private void OnDisable() =>
            _buyButton.onClick.RemoveListener(OnBuyButtonClicked);

        private void OnBuyButtonClicked()
        {
            if (!_walletService.HasEnough(ItemTypeId, Price))
            {
                _floatingTextService.ShowFloatingText(FloatingTextType.NotEnoughMoney, transform, transform.position);
                return;
            }

            _shopItemDataService.Add(GameItemType);
            var popupWindow = _windowService.Get<PopupWindow>();
            popupWindow.Init(GameItemType, _windowService);
            popupWindow.Open();
            _walletService.Set(ItemTypeId, -Price);
            Destroy(gameObject);
        }
    }
}