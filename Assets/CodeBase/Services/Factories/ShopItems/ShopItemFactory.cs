using CodeBase.Enums;
using CodeBase.Gameplay.ShopItemSystem;
using CodeBase.Services.DataService;
using UnityEngine;
using Zenject;

namespace CodeBase.Services.Factories.ShopItems
{
    public class ShopItemFactory
    {
        private readonly IInstantiator _instantiator;
        private readonly ItemStaticDataService _itemStaticDataService;

        public ShopItemFactory(IInstantiator instantiator, ItemStaticDataService itemStaticDataService)
        {
            _itemStaticDataService = itemStaticDataService;
            _instantiator = instantiator;
        }

        public ShopItemModel Create(ShopItemTypeId shopItemTypeId, Transform parent, Vector3 at)
        {
            ShopItemModel prefab = _itemStaticDataService.Get(shopItemTypeId);

            return _instantiator.InstantiatePrefabForComponent<ShopItemModel>(prefab,
                at, prefab.transform.rotation,
                parent);
        }
        
        public ShopItemModel Create(ShopItemTypeId shopItemTypeId, Transform parent, Vector3 at, Quaternion rotation)
        {
            ShopItemModel prefab = _itemStaticDataService.Get(shopItemTypeId);

            return _instantiator.InstantiatePrefabForComponent<ShopItemModel>(prefab,
                at, rotation,
                parent);
        }
    }
}