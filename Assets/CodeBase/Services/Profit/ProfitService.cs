using System;
using System.Collections;
using System.Linq;
using CodeBase.Constant;
using CodeBase.Data;
using CodeBase.Gameplay.Wallet;
using CodeBase.Services.Coroutine;
using CodeBase.Services.Time;
using CodeBase.Services.UI;
using CodeBase.Services.WorldData;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace CodeBase.Services.Profit
{
    public class ProfitService : IInitializable, IDisposable
    {
        private const int OfflineReward = 2;
        private readonly IWorldDataService _worldDataService;
        private readonly WaitForSecondsRealtime _waitMinute = new(60f);
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly WalletService _walletService;
        private readonly WorldTimeService _worldTimeService;
        private readonly UIService _uiService;

        private float _totalEarnedProfit;

        public event Action<string, float> ProfitGot;

        public ProfitService(IWorldDataService worldDataService,
            ICoroutineRunner coroutineRunner,
            WalletService walletService,
            WorldTimeService worldTimeService,
            UIService uiService)
        {
            _uiService = uiService;
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
            
            ReceiveOfflineProfit(playerData, timeDifferenceByMinutes);

            if (_totalEarnedProfit == 0)
                return;
            
            _uiService.OpenOfflineRewardWindow(_totalEarnedProfit, timeDifferenceByMinutes);
        }

        public void Initialize()
        {
            Application.focusChanged += OnFocusChanged;
        }

        public void Dispose()
        {
            Application.focusChanged -= OnFocusChanged;
        }

        private void ReceiveOfflineProfit(PlayerData playerData, int timeDifferenceByMinutes)
        {
            foreach (var totalProfit in playerData.PurchasedEmployees
                         .Select(employeeData => employeeData.Profit / TimeConstantValue.MinutesInDay * timeDifferenceByMinutes / OfflineReward))
            {
                _walletService.AddMoney(totalProfit);
                _worldTimeService.SaveLastProfitEarnedTime();
                _totalEarnedProfit += totalProfit;
            }
        }

        private void ReceiveProfit(PlayerData playerData, int timeDifferenceByMinutes)
        {
            foreach (EmployeeData employeeData in playerData.PurchasedEmployees)
            {
                int totalProfit = (employeeData.Profit / TimeConstantValue.MinutesInDay) * timeDifferenceByMinutes;

                if (totalProfit == 0)
                    return;

                _walletService.AddMoney(totalProfit);
                _worldTimeService.SaveLastProfitEarnedTime();
                _totalEarnedProfit += totalProfit;
                ProfitGot?.Invoke(employeeData.Id, totalProfit);
            }
        }

        private IEnumerator GetProfitEveryMinuteCoroutine(PlayerData playerData)
        {
            while (true)
            {
                yield return _waitMinute;

                foreach (EmployeeData employeeData in playerData.PurchasedEmployees)
                {
                    int minuteProfit = employeeData.Profit / TimeConstantValue.MinutesInDay;
                    _walletService.AddMoney(minuteProfit);
                    ProfitGot?.Invoke(employeeData.Id, minuteProfit);
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