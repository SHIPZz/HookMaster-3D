using CodeBase.Enums;
using CodeBase.Gameplay.ShopItemSystem;
using CodeBase.Services.DataService;
using CodeBase.Services.Providers.Asset;
using UnityEngine;
using Zenject;

namespace CodeBase.Services.Factories.ShopItems
{
    public class GameItemFactory
    {
        private readonly IInstantiator _instantiator;
        private readonly GameItemStaticDataService _gameItemStaticDataService;
        private readonly IAssetProvider _assetProvider;

        public GameItemFactory(IInstantiator instantiator, GameItemStaticDataService gameItemStaticDataService, IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
            _gameItemStaticDataService = gameItemStaticDataService;
            _instantiator = instantiator;
        }

        public T Create<T>(Transform parent, Vector3 at, Quaternion rotation, string path) where T : Component
        {
            var prefab = _assetProvider.Get<T>(path);
            
            return _instantiator.InstantiatePrefabForComponent<T>(prefab,
                at, rotation,
                parent);
        }

        public ShopItemGameModel Create(ShopItemTypeId shopItemTypeId, Transform parent, Vector3 at)
        {
            ShopItemGameModel prefab = _gameItemStaticDataService.Get(shopItemTypeId);

            return _instantiator.InstantiatePrefabForComponent<ShopItemGameModel>(prefab,
                at, prefab.transform.rotation,
                parent);
        }
    }
}