using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Constant;
using CodeBase.Enums;
using CodeBase.Services.Providers.Asset;
using CodeBase.Services.Providers.Location;
using CodeBase.UI.Shop;
using UnityEngine;
using Zenject;

namespace CodeBase.Services.Factories.ShopItems
{
    public class ShopItemFactory
    {
        private readonly DiContainer _diContainer;
        private Dictionary<ShopItemTypeId, Func<ShopItem, ShopItem>> _createActions;
        private IAssetProvider _assetProvider;
        private Dictionary<ShopItemTypeId, ShopItem> _shopItemPrefabs;

        public ShopItemFactory(DiContainer diContainer, LocationProvider locationProvider, IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
            _diContainer = diContainer;

            InitShopItemPrefabs();
            InitCreateActions(locationProvider);
        }

        public ShopItem Create(ShopItemTypeId shopItemTypeId)
        {
            ShopItem prefab = _shopItemPrefabs[shopItemTypeId];
            
            return _createActions[shopItemTypeId]?.Invoke(prefab);
        }

        private void InitShopItemPrefabs()
        {
            _shopItemPrefabs = Resources.LoadAll<ShopItem>(AssetPath.ShopItems)
                .ToDictionary(x => x.ShopItemTypeId, x => x);
        }

        private void InitCreateActions(LocationProvider locationProvider)
        {
            _createActions = new()
            {
                {
                    ShopItemTypeId.CircleRoulette, shopItem =>
                        _diContainer.InstantiatePrefabForComponent<ShopItem>(shopItem,
                            locationProvider.CircleRouletteSpawnPoint.position,shopItem.transform.rotation, 
                            locationProvider.CircleRouletteSpawnPoint)
                },

                {
                    ShopItemTypeId.MiningFarm, shopItem =>
                        _diContainer.InstantiatePrefabForComponent<ShopItem>(shopItem,
                            locationProvider.MiningFarmSpawnPoint.position,shopItem.transform.rotation,
                            locationProvider.MiningFarmSpawnPoint)
                }
            };
        }
    }
}