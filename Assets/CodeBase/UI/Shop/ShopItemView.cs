using CodeBase.Data;
using CodeBase.Enums;
using CodeBase.Services.Factories.ShopItems;
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
        
        [SerializeField] public TMP_Text _priceText;
        [SerializeField] private Button _buyButton;
        
        private ShopItemFactory _shopItemFactory;

        [Inject]
        private void Construct(ShopItemFactory shopItemFactory) =>
            _shopItemFactory = shopItemFactory;

        private void OnEnable()
        {
            _priceText.text = $"{Price}$";
            _buyButton.onClick.AddListener(OnBuyButtonClicked);
        }

        private void OnDisable() =>
            _buyButton.onClick.RemoveListener(OnBuyButtonClicked);

        private void OnBuyButtonClicked()
        {
            _shopItemFactory.Create(ShopItemTypeId);
            Destroy(gameObject);
        }
    }
}