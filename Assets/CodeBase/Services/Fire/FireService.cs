using System;
using System.Collections;
using CodeBase.Constant;
using CodeBase.Gameplay.Fire;
using CodeBase.Services.Coroutine;
using CodeBase.Services.Providers.Fire;
using CodeBase.Services.Providers.Tables;
using CodeBase.Services.Time;
using CodeBase.Services.Window;
using CodeBase.Services.WorldData;
using CodeBase.UI;
using CodeBase.UI.Hud;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CodeBase.Services.Fire
{
    public class FireService : IDisposable
    {
        private const int MinimalBusyTableCountToInitFire = 3;

        private readonly FireProvider _fireProvider;
        private readonly WorldTimeService _worldTimeService;
        private readonly IWorldDataService _worldDataService;
        private readonly WaitForSeconds _minute = new(60f);
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly WindowService _windowService;
        private readonly TableService _tableService;

        public bool IsFired { get; private set; }

        private float _fireInvokeTime;
        private bool _isHudOpened;

        public event Action FireStarted;
        public event Action FirePutOut;

        public FireService(FireProvider fireProvider,
            WorldTimeService worldTimeService,
            IWorldDataService worldDataService,
            ICoroutineRunner coroutineRunner,
            WindowService windowService,
            TableService tableService)
        {
            _tableService = tableService;
            _windowService = windowService;
            _coroutineRunner = coroutineRunner;
            _worldDataService = worldDataService;
            _worldTimeService = worldTimeService;
            _fireProvider = fireProvider;
        }

        public async UniTaskVoid Init()
        {
            var timeDifference = _worldTimeService.GetTimeDifferenceLastFireTimeByMinutes();
            _windowService.Opened += OnWindowOpened;

            await UniTask.WaitUntil(() => _tableService.AvailableTableCount == MinimalBusyTableCountToInitFire);
            
            timeDifference = Mathf.Clamp(timeDifference, 0, TimeConstantValue.TwentyMinutes);

            if (timeDifference == TimeConstantValue.TwentyMinutes)
            {
                InitRandomFire();
                return;
            }

            SetFireInvokeTime(timeDifference);

            _coroutineRunner.StartCoroutine(StartIncreaseFireInvokeTime());
        }

        public void Dispose() =>
            _windowService.Opened -= OnWindowOpened;

        private void OnWindowOpened(WindowBase windowBase)
        {
            if (windowBase.GetType() != typeof(HudWindow))
            {
                _isHudOpened = false;
                return;
            }

            _isHudOpened = true;
        }

        public async UniTaskVoid Reset()
        {
            _fireInvokeTime = TimeConstantValue.TenMinutes;
            _worldDataService.WorldData.FireTimeData.TargetFireInvokeTime = _fireInvokeTime;
            IsFired = false;
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

        private void SetFireInvokeTime(int timeDifference)
        {
            _fireInvokeTime = _worldDataService.WorldData.FireTimeData.TargetFireInvokeTime;

            _fireInvokeTime = Mathf.Clamp(_fireInvokeTime + timeDifference, 0, TimeConstantValue.TwentyMinutes);
        }

        private void InitRandomFire()
        {
            var randomId = Random.Range(0, _fireProvider.FireSpawners.Count);
            FireSpawner randomSpawner = _fireProvider.FireSpawners[randomId];
            randomSpawner.Init();
            IsFired = true;
            FireStarted?.Invoke();
        }

        public void NotifyFirePutOut() =>
            FirePutOut?.Invoke();
    }
}