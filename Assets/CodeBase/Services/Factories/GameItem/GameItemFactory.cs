﻿using CodeBase.Enums;
using CodeBase.Gameplay.GameItems;
using CodeBase.Gameplay.GameItems.RandomItems;
using CodeBase.Gameplay.ResourceItem;
using CodeBase.Services.DataService;
using CodeBase.Services.Providers.Asset;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace CodeBase.Services.Factories.GameItem
{
    public class GameItemFactory
    {
        private readonly IInstantiator _instantiator;
        private readonly GameStaticDataService _gameStaticDataService;
        private readonly IAssetProvider _assetProvider;

        public GameItemFactory(IInstantiator instantiator, GameStaticDataService gameStaticDataService,
            IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
            _gameStaticDataService = gameStaticDataService;
            _instantiator = instantiator;
        }

        public T Create<T>(Transform parent, Vector3 at, Quaternion rotation, string path) where T : Component
        {
            var prefab = _assetProvider.Get<T>(path);

            return _instantiator.InstantiatePrefabForComponent<T>(prefab,
                at, rotation,
                parent);
        }
        
        public T Create<T>(Transform parent, Vector3 at) where T : GameItemAbstract
        {
            var prefab = _gameStaticDataService.Get<T>();

            return _instantiator.InstantiatePrefabForComponent<T>(prefab,
                at, prefab.transform.rotation,
                parent);
        }
        
        [CanBeNull]
        public Resource CreateResourceItem(GameItemType gameItemType, Transform parent, Vector3 at, Quaternion rotation, bool localRotation = false)
        {
            Resource prefab = _gameStaticDataService.GetResourceItem(gameItemType);

            var createdPrefab = _instantiator.InstantiatePrefabForComponent<Resource>(prefab,
                at, rotation, parent);

            if (localRotation)
                createdPrefab.transform.localRotation = rotation;

            return createdPrefab;
        }

        public RandomItem CreateRandomItem(GameItemType gameItemType, Transform parent, Vector3 at)
        {
            RandomItem prefab = _gameStaticDataService.GetRandomItem(gameItemType);

            return _instantiator.InstantiatePrefabForComponent<RandomItem>(prefab,
                at, prefab.transform.rotation,
                parent);
        }
    }
}