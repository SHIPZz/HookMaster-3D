using System.Collections.Generic;
using System.Linq;
using CodeBase.Data;
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

        private List<PurchaseableItem> _allPurchaseableItems = new();

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

            foreach (PurchaseableItem item in _allPurchaseableItems)
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
            }
        }

        public void SaveToData(PurchaseableItem purchaseableItem)
        {
            _worldDataService.WorldData.PurchaseableItemDatas[purchaseableItem.GameItemType] =
                purchaseableItem.ToData();
        }

        private void AddToAllItems()
        {
            foreach (PurchaseableItem purchaseableItem in _purchaseableItemProvider.PurchaseableItems.Values)
            {
                _allPurchaseableItems.Add(purchaseableItem);
            }
        }
    }
}