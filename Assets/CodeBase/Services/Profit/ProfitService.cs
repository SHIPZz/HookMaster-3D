using System;
using System.Collections;
using System.Linq;
using CodeBase.Constant;
using CodeBase.Data;
using CodeBase.Gameplay.Wallet;
using CodeBase.Services.Coroutine;
using CodeBase.Services.Providers.EmployeeProvider;
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
        private readonly WaitForSeconds _waitMinute = new(60f);
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly WalletService _walletService;
        private readonly WorldTimeService _worldTimeService;
        private readonly UIService _uiService;
        private readonly EmployeeService _employeeService;

        private float _totalEarnedProfit;

        public event Action<string, float> ProfitGot;

        public ProfitService(ICoroutineRunner coroutineRunner, 
            WalletService walletService,
            WorldTimeService worldTimeService,
            UIService uiService, 
            EmployeeService employeeService)
        {
            _employeeService = employeeService;
            _uiService = uiService;
            _worldTimeService = worldTimeService;
            _walletService = walletService;
            _coroutineRunner = coroutineRunner;
        }

        public void Init()
        {
            _coroutineRunner.StartCoroutine(GetProfitEveryMinuteCoroutine());
            int timeDifferenceByMinutes = _worldTimeService.GetTimeDifferenceByMinutesBetweenProfitAndCurrentTime();
            
            ReceiveOfflineProfit(timeDifferenceByMinutes);

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

        private void ReceiveOfflineProfit(int timeDifferenceByMinutes)
        {
            foreach (var totalProfit in _employeeService.Employees
                         .Select(employee => employee.Profit / TimeConstantValue.MinutesInDay * timeDifferenceByMinutes / OfflineReward))
            {
                _walletService.Set(ItemTypeId.Money, totalProfit);
                _worldTimeService.SaveLastProfitEarnedTime();
                _totalEarnedProfit += totalProfit;
            }
        }

        private void ReceiveProfit(int timeDifferenceByMinutes)
        {
            foreach (Gameplay.EmployeeSystem.Employee employee in _employeeService.Employees)
            {
                if(!employee.IsWorking)
                    continue;
                
                int totalProfit = (employee.Profit / TimeConstantValue.MinutesInDay) * timeDifferenceByMinutes;

                if (totalProfit == 0)
                    return;

                _walletService.Set(ItemTypeId.Money, totalProfit);
                _worldTimeService.SaveLastProfitEarnedTime();
                _totalEarnedProfit += totalProfit;
                ProfitGot?.Invoke(employee.Id, totalProfit);
            }
        }

        private IEnumerator GetProfitEveryMinuteCoroutine()
        {
            while (true)
            {
                yield return _waitMinute;

                foreach (Gameplay.EmployeeSystem.Employee employee in _employeeService.Employees)
                {
                    if(!employee.IsWorking)
                        continue;
                    
                    int minuteProfit = employee.Profit / TimeConstantValue.MinutesInDay;
                    _walletService.Set(ItemTypeId.Money, minuteProfit);
                    ProfitGot?.Invoke(employee.Id, minuteProfit);
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

            ReceiveProfit(_worldTimeService.GetTimeDifferenceByMinutesBetweenProfitAndCurrentTime());
        }
    }
}