using System.Collections;
using CodeBase.Constant;
using CodeBase.Enums;
using CodeBase.Gameplay.GameItems;
using CodeBase.Services.Coroutine;
using CodeBase.Services.Factories.ShopItems;
using CodeBase.Services.Providers.Location;
using CodeBase.Services.Time;
using CodeBase.Services.WorldData;
using UnityEngine;

namespace CodeBase.Services.RandomItems
{
    public class RandomItemService
    {
        private readonly WorldTimeService _worldTimeService;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly IWorldDataService _worldDataService;
        private readonly GameItemFactory _gameItemFactory;
        private readonly LocationProvider _locationProvider;
        private GameItemAbstract _lastItem;

        public RandomItemService(WorldTimeService worldTimeService,
            GameItemFactory gameItemFactory,
            ICoroutineRunner coroutineRunner,
            IWorldDataService worldDataService, LocationProvider locationProvider)
        {
            _locationProvider = locationProvider;
            _gameItemFactory = gameItemFactory;
            _worldDataService = worldDataService;
            _coroutineRunner = coroutineRunner;
            _worldTimeService = worldTimeService;
        }

        public void Init()
        {
            var timeDifference = _worldTimeService.GetTimeDifferenceByLastSpawnedRandomItemInMinutes();
            var spawnTime = _worldDataService.WorldData.RandomItemData.SpawnMinutes;

            spawnTime = Mathf.Clamp(spawnTime + timeDifference, 0, TimeConstantValue.ThreeMinutes);

            if (spawnTime >= TimeConstantValue.ThreeMinutes)
            {
                Transform randomSpawnPoint = GetRandomPosition();
                _lastItem = _gameItemFactory.Create(GameItemType.SuitCase, randomSpawnPoint, randomSpawnPoint.position);

                ResetTime();
                return;
            }

            _coroutineRunner.StartCoroutine(StartSpawnTimer());
        }

        private IEnumerator StartSpawnTimer()
        {
            while (true)
            {
                yield return new WaitForSeconds(TimeConstantValue.ThreeMinutesInSeconds);
                Transform randomSpawnPoint = GetRandomPosition();

                if (_lastItem != null)
                    Object.Destroy(_lastItem.gameObject);

                _lastItem = _gameItemFactory.Create(GameItemType.SuitCase, randomSpawnPoint, randomSpawnPoint.position);
                ResetTime();
                // spawnTime++;
            }
        }

        private Transform GetRandomPosition()
        {
            var randomId = Random.Range(0, _locationProvider.RandomItemSpawnPoints.Count);
            return _locationProvider.RandomItemSpawnPoints[randomId];
        }

        private void ResetTime()
        {
            _worldDataService.WorldData.RandomItemData.SpawnMinutes = 0;
            _worldTimeService.ResetLastSpawnedRandomItemTime();
        }
    }
}