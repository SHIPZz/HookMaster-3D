using CodeBase.Services.Providers.EmployeeProvider;
using CodeBase.Services.Time;
using Zenject;

namespace CodeBase.Services.Employee
{
    public class EmployeeLazinessService : IInitializable
    {
        private const int LazinessInvokeDay = 1;
        private readonly WorldTimeService _worldTimeService;
        private readonly EmployeeProvider _employeeProvider;

        public EmployeeLazinessService(WorldTimeService worldTimeService, 
            EmployeeProvider employeeProvider)
        {
            _employeeProvider = employeeProvider;
            _worldTimeService = worldTimeService;
        }

        public void Initialize()
        {
            if (_worldTimeService.GetTimeDifferenceByDay() >= LazinessInvokeDay)
            {
                _employeeProvider.Employees.ForEach(x => x.StopWorking());
            }
        }
    }
}