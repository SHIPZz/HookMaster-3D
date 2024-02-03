using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Data;
using CodeBase.Enums;
using CodeBase.Extensions;
using CodeBase.Gameplay.PurchaseableSystem;
using CodeBase.Services.DataService;
using CodeBase.Services.Providers.PurchaseableItemProviders;
using CodeBase.Services.WorldData;
using CodeBase.SO.PurchaseaItems;

namespace CodeBase.Services.PurchaseableItemServices
{
    public class PurchaseableItemService
    {
        private readonly IWorldDataService _worldDataService;
        private readonly PurchaseableItemProvider _purchaseableItemProvider;
        private readonly GameStaticDataService _gameStaticDataService;

        private Dictionary<GameItemType, PurchaseableItem> _allPurchaseableItems = new();
        private Dictionary<GameItemType, PurchaseableItem> _purchasedItems = new();

        public event Action<PurchaseableItem> Purchased;

        public PurchaseableItemService(IWorldDataService worldDataService,
            PurchaseableItemProvider purchaseableItemProvider,
            GameStaticDataService gameStaticDataService)
        {
            _gameStaticDataService = gameStaticDataService;
            _worldDataService = worldDataService;
            _purchaseableItemProvider = purchaseableItemProvider;
        }

        public void Init()
        {
            IEnumerable<PurchaseableItemSO> purchaseableItemsSO =
                _gameStaticDataService.GetAllSO<PurchaseableItemSO>();

            AddToAllItems();

            foreach (PurchaseableItem item in _allPurchaseableItems.Values)
            {
                if (!_worldDataService.WorldData.PurchaseableItemDatas.ContainsKey(item.GameItemType))
                {
                    PurchaseableItemSO targetData = purchaseableItemsSO.First(x => x.GameItemType == item.GameItemType);
                    item.Price = targetData.Price;
                    item.IsAсcessible = false;
                    continue;
                }

                PurchaseableItemData data = _worldDataService.WorldData.PurchaseableItemDatas[item.GameItemType];
                item.Price = data.Price;
                item.IsAсcessible = data.IsAccessible;
                _purchasedItems[item.GameItemType] = item;
            }
        }

        public bool HasItem(GameItemType gameItemType) =>
            _purchasedItems.ContainsKey(gameItemType);

        public bool IsAccessible(GameItemType gameItemType) =>
            _worldDataService.WorldData.PurchaseableItemDatas.ContainsKey(gameItemType) &&
            _worldDataService.WorldData.PurchaseableItemDatas[gameItemType].IsAccessible;

        public void SaveToData(PurchaseableItem purchaseableItem)
        {
            _worldDataService.WorldData.PurchaseableItemDatas[purchaseableItem.GameItemType] =
                purchaseableItem.ToData();
            _purchasedItems[purchaseableItem.GameItemType] = purchaseableItem;
            Purchased?.Invoke(purchaseableItem);
        }

        private void AddToAllItems()
        {
            foreach (PurchaseableItem purchaseableItem in _purchaseableItemProvider.PurchaseableItems.Values)
            {
                if (purchaseableItem == null)
                    continue;

                _allPurchaseableItems[purchaseableItem.GameItemType] = purchaseableItem;
            }
        }
    }
}