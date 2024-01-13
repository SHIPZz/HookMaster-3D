using System.Collections.Generic;
using System.Linq;
using CodeBase.Constant;
using CodeBase.Data;
using CodeBase.Gameplay.Wallet;
using CodeBase.Services.Factories.UI;
using CodeBase.Services.Providers.Asset;
using CodeBase.Services.ShopItemData;
using CodeBase.Services.ShopItemDataServices;
using UnityEngine;
using Zenject;

namespace CodeBase.UI.Shop
{
    public class ShopTabView : MonoBehaviour
    {
        [field: SerializeField] public ItemTypeId ItemTypeId { get; private set; }
        [SerializeField] private Transform _parent;

        private IAssetProvider _assetProvider;
        private UIFactory _uiFactory;
        private ShopItemDataService _shopItemDataService;

        [Inject]
        private void Construct( IAssetProvider assetProvider, UIFactory uiFactory, ShopItemDataService shopItemDataService)
        {
            _shopItemDataService = shopItemDataService;
            _uiFactory = uiFactory;
            _assetProvider = assetProvider;
        }

        public void Init()
        {
            IEnumerable<ShopItemView> shopItemViews = _assetProvider.GetAll<ShopItemView>(AssetPath.ShopItemViews)
                .Where(x => x.ItemTypeId == ItemTypeId);

            foreach (ShopItemView shopItemView in shopItemViews)
            {
                if (_shopItemDataService.AlreadyPurchased(shopItemView.GameItemType))
                    continue;
                
                _uiFactory.CreateElement<ShopItemView>(shopItemView, _parent);
            }
        }
    }
}