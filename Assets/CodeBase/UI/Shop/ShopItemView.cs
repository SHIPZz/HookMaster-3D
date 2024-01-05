using CodeBase.Data;
using CodeBase.Enums;
using CodeBase.Gameplay.Wallet;
using CodeBase.Services.Factories.ShopItems;
using CodeBase.Services.ShopItemData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.UI.Shop
{
    public class ShopItemView : MonoBehaviour
    {
        [field: SerializeField] public ItemTypeId ItemTypeId { get; private set; }
        [field: SerializeField] public ShopItemTypeId ShopItemTypeId { get; private set; }
        [field: SerializeField] public int Price { get; private set; }

        [SerializeField] private TMP_Text _priceText;
        [SerializeField] private Button _buyButton;

        private ShopItemFactory _shopItemFactory;
        private WalletService _walletService;
        private ShopItemService _shopItemService;

        [Inject]
        private void Construct(ShopItemFactory shopItemFactory, WalletService walletService,
            ShopItemService shopItemService)
        {
            _shopItemService = shopItemService;
            _walletService = walletService;
            _shopItemFactory = shopItemFactory;
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
            ShopItem shopItem = _shopItemFactory.Create(ShopItemTypeId);
            _shopItemService.Add(shopItem);
            _walletService.Set(ItemTypeId, -Price);
            Destroy(gameObject);
        }
    }
}