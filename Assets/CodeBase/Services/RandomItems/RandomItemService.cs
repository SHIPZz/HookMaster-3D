using System.Collections;
using CodeBase.Constant;
using CodeBase.Enums;
using CodeBase.Services.Coroutine;
using CodeBase.Services.Factories.RandomItems;
using CodeBase.Services.Time;
using CodeBase.Services.WorldData;
using UnityEngine;

namespace CodeBase.Services.RandomItems
{
    public class RandomItemService
    {
        private readonly WorldTimeService _worldTimeService;
        private readonly RandomItemFactory _randomItemFactory;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly WaitForSecondsRealtime _minute = new(60f);
        private readonly IWorldDataService _worldDataService;

        public RandomItemService(WorldTimeService worldTimeService, RandomItemFactory randomItemFactory,
            ICoroutineRunner coroutineRunner, IWorldDataService worldDataService)
        {
            _worldDataService = worldDataService;
            _coroutineRunner = coroutineRunner;
            _worldTimeService = worldTimeService;
            _randomItemFactory = randomItemFactory;
        }

        public void Init()
        {
            var timeDifference = _worldTimeService.GetTimeDifferenceByLastSpawnedRandomItemInMinutes();
            var spawnTime = _worldDataService.WorldData.RandomItemData.SpawnMinutes;

            spawnTime = Mathf.Clamp(spawnTime + timeDifference, 0, TimeConstantValue.MinutesInHour);

            if (spawnTime >= TimeConstantValue.MinutesInHour)
            {
                _randomItemFactory.Create(RandomItemTypeId.SuitCase);
                ResetTime();
                return;
            }

            _coroutineRunner.StartCoroutine(StartSpawnTimer(spawnTime));
        }

        private IEnumerator StartSpawnTimer(int spawnTime)
        {
            while (spawnTime != TimeConstantValue.MinutesInHour)
            {
                yield return _minute;
                spawnTime++;
            }

            _randomItemFactory.Create(RandomItemTypeId.SuitCase);
            ResetTime();
        }

        private void ResetTime()
        {
            _worldDataService.WorldData.RandomItemData.SpawnMinutes = 0;
            _worldTimeService.ResetLastSpawnedRandomItemTime();
        }
    }
}