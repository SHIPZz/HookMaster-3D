using System;
using System.Collections;
using CodeBase.Constant;
using CodeBase.Gameplay.Fire;
using CodeBase.Services.Coroutine;
using CodeBase.Services.Providers.Fire;
using CodeBase.Services.Time;
using CodeBase.Services.WorldData;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CodeBase.Services.Fire
{
    public class FireService
    {
        private readonly FireProvider _fireProvider;
        private readonly WorldTimeService _worldTimeService;
        private readonly IWorldDataService _worldDataService;
        private readonly WaitForSeconds _minute = new(60f);
        private readonly ICoroutineRunner _coroutineRunner;
        private float _fireInvokeTime;

        public FireService(FireProvider fireProvider,
            WorldTimeService worldTimeService,
            IWorldDataService worldDataService,
            ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
            _worldDataService = worldDataService;
            _worldTimeService = worldTimeService;
            _fireProvider = fireProvider;
        }

        public void Init()
        {
            var timeDifference = _worldTimeService.GetTimeDifferenceLastFireTimeByMinutes();

            timeDifference = Mathf.Clamp(timeDifference, 0, TimeConstantValue.TwentyMinutes);
            
            Debug.Log(timeDifference);

            if (timeDifference == TimeConstantValue.TwentyMinutes)
            {
                InitRandomFire();
                return;
            }

            _fireInvokeTime = _worldDataService.WorldData.FireTimeData.TargetFireInvokeTime;

            _fireInvokeTime = Mathf.Clamp(_fireInvokeTime + timeDifference, 0, TimeConstantValue.TwentyMinutes);

            _coroutineRunner.StartCoroutine(StartIncreaseFireInvokeTime());
        }

        public async void Reset()
        {
            _fireInvokeTime = TimeConstantValue.TenMinutes;
            _worldDataService.WorldData.FireTimeData.TargetFireInvokeTime = _fireInvokeTime;
            await _worldTimeService.UpdateWorldTime();
            _worldTimeService.SaveLastFireTime();
        }

        private IEnumerator StartIncreaseFireInvokeTime()
        {
            while (_fireInvokeTime != 0)
            {
                _fireInvokeTime--;
                _worldDataService.WorldData.FireTimeData.TargetFireInvokeTime = _fireInvokeTime;
                yield return _minute;
            }

            InitRandomFire();
            _worldDataService.WorldData.FireTimeData.TargetFireInvokeTime = 0;
            _worldTimeService.SaveLastFireTime();
        }

        private void InitRandomFire()
        {
            var randomId = Random.Range(0, _fireProvider.FireSpawners.Count);
            FireSpawner randomSpawner = _fireProvider.FireSpawners[randomId];
            randomSpawner.Init();
        }
    }
}