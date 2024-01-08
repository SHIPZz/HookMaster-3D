using CodeBase.Data;
using CodeBase.Enums;
using CodeBase.Gameplay.ShopItemSystem;
using CodeBase.Gameplay.Wallet;
using CodeBase.Services.ShopItemData;
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
        [field: SerializeField] public ShopItemTypeId ShopItemTypeId { get; private set; }
        [field: SerializeField] public int Price { get; private set; }

        [SerializeField] private TMP_Text _priceText;
        [SerializeField] private Button _buyButton;

        private WalletService _walletService;
        private ShopItemService _shopItemService;
        private FloatingTextService _floatingTextService;

        [Inject]
        private void Construct(WalletService walletService, ShopItemService shopItemService,
            FloatingTextService floatingTextService)
        {
            _floatingTextService = floatingTextService;
            _shopItemService = shopItemService;
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

            ShopItemModel shopItemModel = _shopItemService.Create<ShopItemModel>(ShopItemTypeId);
            _shopItemService.Add(shopItemModel);
            _walletService.Set(ItemTypeId, -Price);
            Destroy(gameObject);
        }
    }
}