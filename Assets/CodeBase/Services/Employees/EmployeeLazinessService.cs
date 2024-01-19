using CodeBase.Services.Time;
using Zenject;

namespace CodeBase.Services.Employees
{
    public class EmployeeLazinessService : IInitializable
    {
        private const int LazinessInvokeDay = 1;
        private readonly WorldTimeService _worldTimeService;
        private readonly EmployeeService _employeeService;

        public EmployeeLazinessService(WorldTimeService worldTimeService,
            EmployeeService employeeService)
        {
            _employeeService = employeeService;
            _worldTimeService = worldTimeService;
        }

        public void Initialize()
        {
            if (_worldTimeService.GetTimeDifferenceByLastLazinessDays() < LazinessInvokeDay)
                return;

            _employeeService.Employees.ForEach(x => x.StopWorking());
            _worldTimeService.SaveLastLazyDay();
        }
    }
}