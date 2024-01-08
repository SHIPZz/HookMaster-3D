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
        private readonly ItemStaticDataService _itemStaticDataService;
        private readonly IInstantiator _instantiator;
        private readonly LocationProvider _locationProvider;

        public RandomItemFactory(ItemStaticDataService itemStaticDataService, IInstantiator instantiator, LocationProvider locationProvider)
        {
            _itemStaticDataService = itemStaticDataService;
            _instantiator = instantiator;
            _locationProvider = locationProvider;
        }

        public RandomItem Create(RandomItemTypeId randomItemTypeId)
        {
            RandomItem prefab = _itemStaticDataService.Get(randomItemTypeId);

            var randomId = Random.Range(0, _locationProvider.RandomItemSpawnPoints.Count);

            Transform targetTransform = _locationProvider.RandomItemSpawnPoints[randomId];
            
            return _instantiator.InstantiatePrefabForComponent<RandomItem>(prefab,
                targetTransform.position, prefab.transform.rotation,
                targetTransform);
        }
    }
}