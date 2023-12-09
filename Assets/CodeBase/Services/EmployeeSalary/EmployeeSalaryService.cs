using CodeBase.Gameplay.Wallet;
using CodeBase.Services.Providers.EmployeeProvider;
using CodeBase.Services.Time;

namespace CodeBase.Services.EmployeeSalary
{
    public class EmployeeSalaryService
    {
        private const int MinimalPaymentDay = 1;
        private readonly WalletService _walletService;
        private readonly EmployeeProvider _employeeProvider;
        private readonly WorldTimeService _worldTimeService;

        public EmployeeSalaryService(WalletService walletService, 
            EmployeeProvider employeeProvider,
            WorldTimeService worldTimeService)
        {
            _worldTimeService = worldTimeService;
            _walletService = walletService;
            _employeeProvider = employeeProvider;
        }

        public void Init()
        {
            if(_worldTimeService.GetTimeDifferenceByDay() != MinimalPaymentDay)
                return;
            
            int passedDays = _worldTimeService.GetTimeDifferenceByDaysBetweenSalaryPaymentAndCurrentTime();

            if (passedDays == 0)
                return;

            _employeeProvider.Employees.ForEach(x => _walletService.Decrease(x.Salary * passedDays));
            _worldTimeService.SaveLastSalaryPaymentTime();
        }
    }
}