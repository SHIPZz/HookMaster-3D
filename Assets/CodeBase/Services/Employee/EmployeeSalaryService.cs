using CodeBase.Gameplay.Wallet;
using CodeBase.Services.Providers.EmployeeProvider;
using CodeBase.Services.Time;

namespace CodeBase.Services.Employee
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
            int passedDays = _worldTimeService.GetTimeDifferenceByDaysBetweenSalaryPaymentAndCurrentTime();

            if (passedDays == 0)
                return;
            
            _employeeService.Employees.ForEach(x => _walletService.Decrease(x.Salary * passedDays));
            _worldTimeService.SaveLastSalaryPaymentTime();
        }
    }
}