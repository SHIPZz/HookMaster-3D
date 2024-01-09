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
        private readonly GameItemFactory _gameItemFactory;
        private readonly LocationProvider _locationProvider;
        private readonly UIFactory _uiFactory;
        private Dictionary<ShopItemTypeId, ShopItemGameModel> _createdShopItems = new();

        public ShopItemService(IWorldDataService worldDataService, GameItemFactory gameItemFactory,
            LocationProvider locationProvider, UIFactory uiFactory)
        {
            _uiFactory = uiFactory;
            _locationProvider = locationProvider;
            _worldDataService = worldDataService;
            _gameItemFactory = gameItemFactory;
        }

        public void Init()
        {
            foreach (ShopItemTypeId shopItemTypeId in _worldDataService.WorldData.ShopItemData.PurchasedShopItems)
            {
                ShopItemGameModel shopItemGameModel = Create<ShopItemGameModel>(shopItemTypeId);
                _createdShopItems[shopItemGameModel.ShopItemTypeId] = shopItemGameModel;
            }
        }

        public ShopItemView CreateShopItemView(ShopItemView shopItemView, Transform parent) =>
            _uiFactory.CreateElement<ShopItemView>(shopItemView, parent);

        public T Create<T>(ShopItemTypeId shopItemTypeId) where T : ShopItemGameModel
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

        public T Get<T>(ShopItemTypeId shopItemTypeId) where T : ShopItemGameModel
        {
         return (T)_createdShopItems[shopItemTypeId];
        }

        public IEnumerable<T> GetAll<T>(ShopItemTypeId shopItemTypeId) where T : ShopItemGameModel =>
            _createdShopItems.Values.Where(shopItem => shopItem.ShopItemTypeId == shopItemTypeId).OfType<T>();


        public void Add(ShopItemGameModel shopItemGameModel)
        {
            _worldDataService.WorldData.ShopItemData.PurchasedShopItems.Add(shopItemGameModel.ShopItemTypeId);
            _worldDataService.Save();
        }

        public bool AlreadyPurchased(ShopItemTypeId shopItemTypeId) =>
            _worldDataService.WorldData.ShopItemData.PurchasedShopItems.Contains(shopItemTypeId);

        private T CreateMiningFarm<T>() where T : ShopItemGameModel
        {
            var target = (T)_gameItemFactory.Create(ShopItemTypeId.MiningFarm, _locationProvider.MiningFarmSpawnPoint, _locationProvider.MiningFarmSpawnPoint.position);
            _createdShopItems[target.ShopItemTypeId] = target;
            return target;
        }

        private T CreateCircleRoulette<T>() where T : ShopItemGameModel
        {
            var target = (T)_gameItemFactory.Create(ShopItemTypeId.CircleRoulette, _locationProvider.CircleRouletteSpawnPoint,
                _locationProvider.CircleRouletteSpawnPoint.position);
            _createdShopItems[target.ShopItemTypeId] = target;
            return target;
        }
    }
}