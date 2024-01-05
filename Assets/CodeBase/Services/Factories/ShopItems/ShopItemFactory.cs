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
            
            return _instantiator.InstantiatePrefabForComponent<ShopItem>(prefab,
                _locationProvider.CircleRouletteSpawnPoint.position, prefab.transform.rotation,
                _locationProvider.CircleRouletteSpawnPoint);
        }

        private void InitShopItemPrefabs()
        {
            _shopItemPrefabs = Resources.LoadAll<ShopItem>(AssetPath.ShopItems)
                .ToDictionary(x => x.ShopItemTypeId, x => x);
        }
        
    }
}