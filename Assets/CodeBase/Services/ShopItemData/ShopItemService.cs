using System.Collections.Generic;
using System.Linq;
using CodeBase.Enums;
using CodeBase.Gameplay.ShopItemSystem;
using CodeBase.Services.Factories.ShopItems;
using CodeBase.Services.Factories.UI;
using CodeBase.Services.Providers.Location;
using CodeBase.Services.WorldData;
using CodeBase.UI.Shop;
using UnityEngine;

namespace CodeBase.Services.ShopItemData
{
    public class ShopItemService
    {
        private readonly IWorldDataService _worldDataService;
        private readonly ShopItemFactory _shopItemFactory;
        private Dictionary<ShopItemTypeId, ShopItemModel> _createdShopItems = new();
        private readonly LocationProvider _locationProvider;
        private UIFactory _uiFactory;

        public ShopItemService(IWorldDataService worldDataService, ShopItemFactory shopItemFactory,
            LocationProvider locationProvider, UIFactory uiFactory)
        {
            _uiFactory = uiFactory;
            _locationProvider = locationProvider;
            _worldDataService = worldDataService;
            _shopItemFactory = shopItemFactory;
        }

        public void Init()
        {
            foreach (ShopItemTypeId shopItemTypeId in _worldDataService.WorldData.ShopItemData.PurchasedShopItems)
            {
                ShopItemModel shopItemModel = Create<ShopItemModel>(shopItemTypeId);
                _createdShopItems[shopItemModel.ShopItemTypeId] = shopItemModel;
            }
        }

        public ShopItemView CreateShopItemView(ShopItemView shopItemView, Transform parent) =>
            _uiFactory.CreateElement<ShopItemView>(shopItemView, parent);

        public T Create<T>(ShopItemTypeId shopItemTypeId) where T : ShopItemModel
        {
            switch (shopItemTypeId)
            {
                case ShopItemTypeId.MiningFarm:
                    return CreateMiningFarm<T>();

                case ShopItemTypeId.CircleRoulette:
                    return CreateCircleRoulette<T>();
            }

            return null;
        }

        public T Get<T>(ShopItemTypeId shopItemTypeId) where T : ShopItemModel
        {
            return (T)_createdShopItems[shopItemTypeId];
        }

        public IEnumerable<T> GetAll<T>(ShopItemTypeId shopItemTypeId) where T : ShopItemModel =>
            _createdShopItems.Values.Where(shopItem => shopItem.ShopItemTypeId == shopItemTypeId).OfType<T>();


        public void Add(ShopItemModel shopItemModel)
        {
            _worldDataService.WorldData.ShopItemData.PurchasedShopItems.Add(shopItemModel.ShopItemTypeId);
            _worldDataService.Save();
        }

        public bool AlreadyPurchased(ShopItemTypeId shopItemTypeId) =>
            _worldDataService.WorldData.ShopItemData.PurchasedShopItems.Contains(shopItemTypeId);

        private T CreateMiningFarm<T>() where T : ShopItemModel
        {
            return (T)_shopItemFactory.Create(ShopItemTypeId.MiningFarm, _locationProvider.MiningFarmSpawnPoint,
                _locationProvider.MiningFarmSpawnPoint.position);
        }

        private T CreateCircleRoulette<T>() where T : ShopItemModel
        {
            return (T)_shopItemFactory.Create(ShopItemTypeId.CircleRoulette, _locationProvider.CircleRouletteSpawnPoint,
                _locationProvider.CircleRouletteSpawnPoint.position);
        }
    }
}