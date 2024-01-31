using CodeBase.Constant;
using CodeBase.Data;
using CodeBase.Gameplay.Wallet;
using CodeBase.Services.Time;
using CodeBase.Services.Wallet;

namespace CodeBase.Services.Employees
{
    public class EmployeeSalaryService
    {
        private readonly WalletService _walletService;
        private readonly EmployeeService _employeeService;
        private readonly WorldTimeService _worldTimeService;

        public EmployeeSalaryService(WalletService walletService, 
            EmployeeService employeeService,
            WorldTimeService worldTimeService)
        {
            _worldTimeService = worldTimeService;
            _walletService = walletService;
            _employeeService = employeeService;
        }

        public void Init()
        {
            int passedMinutes = _worldTimeService.GetTimeDifferenceByMinutesBetweenSalaryPaymentAndCurrentTime();

            if (passedMinutes < TimeConstantValue.MinutesInTwoHour)
                return;

            foreach (Gameplay.Employees.Employee employee in _employeeService.Employees)
            {
                var targetSalary = employee.Salary / TimeConstantValue.MinutesInDay;
                
                if (targetSalary == 0)
                    return;

                _walletService.Set(ItemTypeId.Money, -targetSalary);
            }

            _worldTimeService.SaveLastSalaryPaymentTime();
        }
    }
}