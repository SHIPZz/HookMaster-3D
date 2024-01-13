using CodeBase.Constant;
using CodeBase.Enums;
using CodeBase.Gameplay.GameItems;
using CodeBase.Services.DataService;
using CodeBase.Services.Providers.Asset;
using CodeBase.SO.GameItem;
using UnityEngine;
using Zenject;

namespace CodeBase.Services.Factories.ShopItems
{
    public class GameItemFactory
    {
        private readonly IInstantiator _instantiator;
        private readonly GameItemStaticDataService _gameItemStaticDataService;
        private readonly IAssetProvider _assetProvider;

        public GameItemFactory(IInstantiator instantiator, GameItemStaticDataService gameItemStaticDataService,
            IAssetProvider assetProvider)
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
        
        public T Create<T>(Transform parent, Vector3 at, Quaternion rotation) where T : GameItemAbstract
        {
            var prefab = _gameItemStaticDataService.Get<T>();

            return _instantiator.InstantiatePrefabForComponent<T>(prefab,
                at, rotation,
                parent);
        }
        
        public T Create<T>(Transform parent, Vector3 at) where T : GameItemAbstract
        {
            var prefab = _gameItemStaticDataService.Get<T>();

            return _instantiator.InstantiatePrefabForComponent<T>(prefab,
                at, prefab.transform.rotation,
                parent);
        }

        public GameItemAbstract Create(GameItemType gameItemType, Transform parent, Vector3 at)
        {
            GameItemAbstract prefab = _gameItemStaticDataService.Get(gameItemType);

            return _instantiator.InstantiatePrefabForComponent<GameItemAbstract>(prefab,
                at, prefab.transform.rotation,
                parent);
        }
    }
}