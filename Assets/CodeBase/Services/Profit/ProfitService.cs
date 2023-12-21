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
            int timeDifferenceByMinutes = _worldTimeService.GetTimeDifferenceByMinutesBetweenProfitAndCurrentTime();

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
                
                if(totalProfit == 0)
                    return;
                
                _walletService.Add(totalProfit);
                _worldTimeService.SaveLastProfitEarnedTime();
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
                    _worldTimeService.SaveLastProfitEarnedTime();
                }
            }
        }

        private async void OnFocusChanged(bool hasFocus)
        {
            if (!hasFocus)
                return;

            while (!_worldTimeService.TimeUpdated) 
                await UniTask.Yield();

            ReceiveProfit(_worldDataService.WorldData.PlayerData,
                _worldTimeService.GetTimeDifferenceByMinutesBetweenProfitAndCurrentTime());
        }
    }
}