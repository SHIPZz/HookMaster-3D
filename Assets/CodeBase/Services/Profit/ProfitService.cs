using System;
using System.Collections;
using CodeBase.Data;
using CodeBase.Gameplay.Wallet;
using CodeBase.Services.Coroutine;
using CodeBase.Services.Time;
using CodeBase.Services.WorldData;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace CodeBase.Services.Profit
{
    public class ProfitService : IInitializable, IDisposable
    {
        private const int MinutesInDay = 1440;
        private readonly IWorldDataService _worldDataService;
        private readonly WaitForSecondsRealtime _waitMinute = new(60f);
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly WalletService _walletService;
        private readonly WorldTimeService _worldTimeService;

        public event Action<Guid, int> ProfitGot;

        public ProfitService(IWorldDataService worldDataService,
            ICoroutineRunner coroutineRunner,
            WalletService walletService,
            WorldTimeService worldTimeService)
        {
            _worldTimeService = worldTimeService;
            _walletService = walletService;
            _coroutineRunner = coroutineRunner;
            _worldDataService = worldDataService;
        }

        public void Init()
        {
            PlayerData playerData = _worldDataService.WorldData.PlayerData;
            _coroutineRunner.StartCoroutine(GetProfitEveryMinuteCoroutine(playerData));
            var timeDifferenceByMinutes = (int)_worldTimeService.GetTimeDifferenceByMinutes();
            Debug.Log(timeDifferenceByMinutes + " time difference");

            ReceiveProfit(playerData, timeDifferenceByMinutes);
        }

        public void Initialize()
        {
            Application.focusChanged += OnFocusChanged;
        }

        public void Dispose()
        {
            Application.focusChanged -= OnFocusChanged;
        }

        private void ReceiveProfit(PlayerData playerData, int timeDifferenceByMinutes)
        {
            foreach (EmployeeData employeeData in playerData.PurchasedEmployees)
            {
                int totalProfit = (employeeData.Profit / MinutesInDay) * timeDifferenceByMinutes;
                _walletService.Add(totalProfit);
                ProfitGot?.Invoke(employeeData.Guid, totalProfit);
            }
        }

        private IEnumerator GetProfitEveryMinuteCoroutine(PlayerData playerData)
        {
            while (true)
            {
                yield return _waitMinute;

                foreach (EmployeeData employeeData in playerData.PurchasedEmployees)
                {
                    int minuteProfit = employeeData.Profit / MinutesInDay;
                    _walletService.Add(minuteProfit);
                    ProfitGot?.Invoke(employeeData.Guid, minuteProfit);
                }
            }
        }

        private async void OnFocusChanged(bool hasFocus)
        {
            if (!hasFocus)
                return;

            while (!_worldTimeService.TimeUpdated)
            {
                await UniTask.Yield();
            }
            
            Debug.Log( (int)_worldTimeService.GetTimeDifferenceByMinutes() + " TIME DIFFERENCE");
            ReceiveProfit(_worldDataService.WorldData.PlayerData,
                (int)_worldTimeService.GetTimeDifferenceByMinutes());
        }
    }
}