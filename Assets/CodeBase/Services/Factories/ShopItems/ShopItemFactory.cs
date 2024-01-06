using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Constant;
using CodeBase.Enums;
using CodeBase.Gameplay.ShopItemSystem;
using CodeBase.Services.Providers.Asset;
using CodeBase.Services.Providers.Location;
using CodeBase.UI.Shop;
using UnityEngine;
using Zenject;

namespace CodeBase.Services.Factories.ShopItems
{
    public class ShopItemFactory
    {
        private readonly IInstantiator _instantiator;
        private readonly LocationProvider _locationProvider;
        private Dictionary<ShopItemTypeId, ShopItem> _shopItemPrefabs;

        public ShopItemFactory(IInstantiator instantiator, LocationProvider locationProvider)
        {
            _locationProvider = locationProvider;
            _instantiator = instantiator;

            InitShopItemPrefabs();
        }

        public ShopItem Create(ShopItemTypeId shopItemTypeId)
        {
            ShopItem prefab = _shopItemPrefabs[shopItemTypeId];

            Transform targetTransform = null;

            switch (shopItemTypeId)
            {
                case ShopItemTypeId.CircleRoulette:
                    targetTransform = _locationProvider.CircleRouletteSpawnPoint;
                    break;
                
                case ShopItemTypeId.MiningFarm:
                    targetTransform = _locationProvider.MiningFarmSpawnPoint;
                    break;
            }
            
            return _instantiator.InstantiatePrefabForComponent<ShopItem>(prefab,
                targetTransform.position, prefab.transform.rotation,
                targetTransform);
        }

        private void InitShopItemPrefabs()
        {
            _shopItemPrefabs = Resources.LoadAll<ShopItem>(AssetPath.ShopItems)
                .ToDictionary(x => x.ShopItemTypeId, x => x);
        }
        
    }
}