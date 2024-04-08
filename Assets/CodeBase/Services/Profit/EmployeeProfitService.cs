using System;
using System.Collections;
using System.Linq;
using CodeBase.Constant;
using CodeBase.Data;
using CodeBase.Gameplay.Wallet;
using CodeBase.Services.Employees;
using CodeBase.Services.Time;
using CodeBase.Services.UI;
using CodeBase.Services.Wallet;
using CodeBase.Services.WorldData;
using CodeBase.SO.Employee;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace CodeBase.Services.Profit
{
    public class EmployeeProfitService
    {
        private const int OfflineReward = 2;
        private readonly WalletService _walletService;
        private readonly WorldTimeService _worldTimeService;
        private readonly UIService _uiService;
        private readonly EmployeeService _employeeService;
        private readonly IWorldDataService _worldDataService;
        private readonly EmployeeStatsSO _employeeStatsSo;

        private int _totalEarnedProfit;

        public event Action<string, int> ProfitGot;

        public EmployeeProfitService(WalletService walletService,
            WorldTimeService worldTimeService,
            UIService uiService,
            EmployeeService employeeService,
            IWorldDataService worldDataService,
            EmployeeStatsSO employeeStatsSo)
        {
            _employeeStatsSo = employeeStatsSo;
            _worldDataService = worldDataService;
            _employeeService = employeeService;
            _uiService = uiService;
            _worldTimeService = worldTimeService;
            _walletService = walletService;
        }

        public void Init()
        {
            int timeDifferenceByMinutes = _worldTimeService.GetTimeDifferenceByMinutesBetweenProfitAndCurrentTime();

            if (!_worldDataService.WorldData.PaperProcessedOnce)
                return;

            ReceiveOfflineProfit(timeDifferenceByMinutes);

            if (_totalEarnedProfit == 0)
                return;

            _uiService.OpenOfflineRewardWindow(_totalEarnedProfit, timeDifferenceByMinutes);
        }

        private void ReceiveOfflineProfit(int timeDifferenceByMinutes)
        {
            foreach (var employee in _employeeService.Employees)
            {
                var randomOfflineProfit = Random.Range(_employeeStatsSo.MinOfflineProfit, _employeeStatsSo.MaxOfflineProfit);
                
                var totalProfit = randomOfflineProfit / TimeConstantValue.MinutesInDay * timeDifferenceByMinutes / OfflineReward;

                _walletService.Set(ItemTypeId.Money, totalProfit);
                _worldTimeService.SaveLastProfitEarnedTime();
                _totalEarnedProfit += totalProfit;
            }
        }
    }
}