using CodeBase.Enums;
using CodeBase.Gameplay.RandomItemSystem;
using CodeBase.Services.DataService;
using CodeBase.Services.Providers.Location;
using UnityEngine;
using Zenject;

namespace CodeBase.Services.Factories.RandomItems
{
    public class RandomItemFactory
    {
        private readonly GameItemStaticDataService _gameItemStaticDataService;
        private readonly IInstantiator _instantiator;
        private readonly LocationProvider _locationProvider;

        public RandomItemFactory(GameItemStaticDataService gameItemStaticDataService, IInstantiator instantiator, LocationProvider locationProvider)
        {
            _gameItemStaticDataService = gameItemStaticDataService;
            _instantiator = instantiator;
            _locationProvider = locationProvider;
        }

        public RandomItemGameModel Create(RandomItemTypeId randomItemTypeId)
        {
            RandomItemGameModel prefab = _gameItemStaticDataService.Get(randomItemTypeId);

            var randomId = Random.Range(0, _locationProvider.RandomItemSpawnPoints.Count);

            Transform targetTransform = _locationProvider.RandomItemSpawnPoints[randomId];
            
            return _instantiator.InstantiatePrefabForComponent<RandomItemGameModel>(prefab,
                targetTransform.position, prefab.transform.rotation,
                targetTransform);
        }
    }
}