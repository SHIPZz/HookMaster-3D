using CodeBase.Enums;
using CodeBase.Gameplay.ShopItemSystem;
using CodeBase.Services.DataService;
using UnityEngine;
using Zenject;

namespace CodeBase.Services.Factories.ShopItems
{
    public class GameItemFactory
    {
        private readonly IInstantiator _instantiator;
        private readonly GameItemStaticDataService _gameItemStaticDataService;

        public GameItemFactory(IInstantiator instantiator, GameItemStaticDataService gameItemStaticDataService)
        {
            _gameItemStaticDataService = gameItemStaticDataService;
            _instantiator = instantiator;
        }

        public ShopItemGameModel Create(ShopItemTypeId shopItemTypeId, Transform parent, Vector3 at)
        {
            ShopItemGameModel prefab = _gameItemStaticDataService.Get(shopItemTypeId);

            return _instantiator.InstantiatePrefabForComponent<ShopItemGameModel>(prefab,
                at, prefab.transform.rotation,
                parent);
        }
        
        public ShopItemGameModel Create(ShopItemTypeId shopItemTypeId, Transform parent, Vector3 at, Quaternion rotation)
        {
            ShopItemGameModel prefab = _gameItemStaticDataService.Get(shopItemTypeId);

            return _instantiator.InstantiatePrefabForComponent<ShopItemGameModel>(prefab,
                at, rotation,
                parent);
        }
    }
}