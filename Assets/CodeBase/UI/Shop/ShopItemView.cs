using CodeBase.Constant;
using CodeBase.Data;
using CodeBase.Enums;
using CodeBase.Gameplay.Wallet;
using CodeBase.Services.Factories.ShopItems;
using CodeBase.Services.Factories.UI;
using CodeBase.Services.GOPool;
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
        [SerializeField] private int _textCount = 10;

        private ShopItemFactory _shopItemFactory;
        private WalletService _walletService;
        private ShopItemService _shopItemService;
        private EnumObjectPool<FloatingTextView, Transform, FloatingTextType> _floatingTextPool;

        [Inject]
        private void Construct(ShopItemFactory shopItemFactory, WalletService walletService,
            ShopItemService shopItemService, UIFactory uiFactory)
        {
            _shopItemService = shopItemService;
            _walletService = walletService;
            _floatingTextPool = new EnumObjectPool<FloatingTextView, Transform, FloatingTextType>(uiFactory.CreateFloatingTextView, _textCount);
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
            if (!_walletService.HasEnough(ItemTypeId, Price))
            {
                FloatingTextView floatingTextView = _floatingTextPool.Pop(transform, FloatingTextType.NotEnoughMoney);
                floatingTextView.Init(transform.position, _floatingTextPool);
                return;
            }

            ShopItem shopItem = _shopItemFactory.Create(ShopItemTypeId);
            _shopItemService.Add(shopItem);
            _walletService.Set(ItemTypeId, -Price);
            Destroy(gameObject);
        }
    }
}