using CodeBase.Enums;
using CodeBase.Services.Factories.ShopItems;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.UI.Shop
{
    public class ShopItemView : MonoBehaviour
    {
        [field: SerializeField] public WalletValueTypeId WalletValueTypeId { get; private set; }
        [field: SerializeField] public ShopItemTypeId ShopItemTypeId { get; private set; }
        
        [SerializeField] private Button _buyButton;
        
        private ShopItemFactory _shopItemFactory;

        [Inject]
        private void Construct(ShopItemFactory shopItemFactory) =>
            _shopItemFactory = shopItemFactory;

        private void OnEnable() =>
            _buyButton.onClick.AddListener(OnBuyButtonClicked);

        private void OnDisable() =>
            _buyButton.onClick.RemoveListener(OnBuyButtonClicked);

        private void OnBuyButtonClicked()
        {
            _shopItemFactory.Create(ShopItemTypeId);
            Destroy(gameObject);
        }
    }
}