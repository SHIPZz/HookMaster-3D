using System.Collections;
using CodeBase.Gameplay.Wallet;
using CodeBase.Services.Coroutine;
using CodeBase.Services.Providers.EmployeeProvider;
using CodeBase.Services.Time;
using UnityEngine;

namespace CodeBase.Services.EmployeeSalary
{
    public class EmployeeSalaryService
    {
        private const int DayForPayment = 1;
        private readonly WalletService _walletService;
        private readonly EmployeeProvider _employeeProvider;
        private readonly WorldTimeService _worldTimeService;
        private readonly ICoroutineRunner _coroutineRunner;

        private readonly WaitForSecondsRealtime _waitHour = new(3600f);

        public EmployeeSalaryService(WalletService walletService, 
            EmployeeProvider employeeProvider,
            WorldTimeService worldTimeService,
            ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
            _worldTimeService = worldTimeService;
            _walletService = walletService;
            _employeeProvider = employeeProvider;
        }

        public void Init()
        {
            _coroutineRunner.StartCoroutine(StartPaymentTimer());

            int passedHours = _worldTimeService.GetTimeDifferenceByHoursBetweenSalaryPaymentAndCurrentTime();

            if (passedHours == 0)
                return;

            _employeeProvider.Employees.ForEach(x => _walletService.Decrease(x.Salary * passedHours));
            _worldTimeService.SaveLastSalaryPaymentTime();
        }

        private IEnumerator StartPaymentTimer()
        {
            while (true)
            {
                yield return _waitHour;
                _employeeProvider.Employees.ForEach(x=>
                {
                    _walletService.Decrease(x.Salary);
                    x.AddSalary(x.Salary);
                });
            }
        }
    }
}