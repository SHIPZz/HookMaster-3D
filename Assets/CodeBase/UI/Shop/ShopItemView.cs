using System;
using CodeBase.Data;
using CodeBase.Enums;
using CodeBase.Gameplay.Wallet;
using CodeBase.Services.Camera;
using CodeBase.Services.ShopItemData;
using CodeBase.Services.ShopItemDataServices;
using CodeBase.Services.UI;
using CodeBase.UI.FloatingText;
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
        private GameItemService _gameItemService;
        private FloatingTextService _floatingTextService;
        private ShopItemDataService _shopItemDataService;

        [Inject]
        private void Construct(WalletService walletService, GameItemService gameItemService,
            FloatingTextService floatingTextService,
            ShopItemDataService shopItemDataService)
        {
            _shopItemDataService = shopItemDataService;
            _floatingTextService = floatingTextService;
            _gameItemService = gameItemService;
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
             _gameItemService.Create(GameItemType);

            _walletService.Set(ItemTypeId, -Price);
            Destroy(gameObject);
        }
    }
}