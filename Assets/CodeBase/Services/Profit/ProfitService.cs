using System;
using System.Collections;
using CodeBase.Data;
using CodeBase.Gameplay.Wallet;
using CodeBase.Services.Coroutine;
using CodeBase.Services.WorldData;
using UnityEngine;

namespace CodeBase.Services.Profit
{
    public class ProfitService
    {
        private const int MinutesInDay = 1440;
        private readonly IWorldDataService _worldDataService;
        private readonly WaitForSecondsRealtime _waitMinute = new(60f);
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly WalletService _walletService;

        public event Action<Guid, int> ProfitGot; 

        public ProfitService(IWorldDataService worldDataService,
            ICoroutineRunner coroutineRunner,
            WalletService walletService)
        {
            _walletService = walletService;
            _coroutineRunner = coroutineRunner;
            _worldDataService = worldDataService;
        }

        public void Init()
        {
            PlayerData playerData = _worldDataService.WorldData.PlayerData;
            _coroutineRunner.StartCoroutine(GetProfitEveryMinuteCoroutine(playerData));
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
    }
}