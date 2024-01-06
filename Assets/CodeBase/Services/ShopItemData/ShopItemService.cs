using System.Collections.Generic;
using CodeBase.Enums;
using CodeBase.Services.Factories.ShopItems;
using CodeBase.Services.WorldData;
using CodeBase.UI.Shop;

namespace CodeBase.Services.ShopItemData
{
    public class ShopItemService
    {
        private readonly IWorldDataService _worldDataService;
        private readonly ShopItemFactory _shopItemFactory;
        private Dictionary<ShopItemTypeId, ShopItem> _createdShopItems = new();

        public ShopItemService(IWorldDataService worldDataService, ShopItemFactory shopItemFactory)
        {
            _worldDataService = worldDataService;
            _shopItemFactory = shopItemFactory;
        }

        public void Init()
        {
            foreach (ShopItemTypeId shopItemTypeId in _worldDataService.WorldData.ShopItemData.PurchasedShopItems)
            {
               ShopItem shopItem = _shopItemFactory.Create(shopItemTypeId);
               _createdShopItems[shopItem.ShopItemTypeId] = shopItem;
            }
        }

        public T Get<T>(ShopItemTypeId shopItemTypeId) where T : ShopItem
        {
            return (T)_createdShopItems[shopItemTypeId];
        }

        public void Add(CodeBase.UI.Shop.ShopItem shopItem)
        {
            _worldDataService.WorldData.ShopItemData.PurchasedShopItems.Add(shopItem.ShopItemTypeId);
            _worldDataService.Save();
        }

        public bool AlreadyPurchased(ShopItemTypeId shopItemTypeId) => 
            _worldDataService.WorldData.ShopItemData.PurchasedShopItems.Contains(shopItemTypeId);
    }
}   