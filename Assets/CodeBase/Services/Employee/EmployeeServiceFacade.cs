using CodeBase.Services.Providers.Tables;

namespace CodeBase.Services.Employee
{
    public class EmployeeServiceFacade
    {
        private readonly EmployeeLazinessService _employeeLazinessService;
        private readonly EmployeeSalaryService _employeeSalaryService;
        private readonly EmployeeHirerService _employeeHirerService;
        private readonly EmployeeDataService _employeeDataService;

        public EmployeeLazinessService EmployeeLazinessService => _employeeLazinessService;

        public EmployeeSalaryService EmployeeSalaryService => _employeeSalaryService;
        public EmployeeHirerService EmployeeHirerService => _employeeHirerService;

        public EmployeeDataService EmployeeDataService => _employeeDataService;

        public EmployeeServiceFacade(EmployeeLazinessService employeeLazinessService,
            EmployeeSalaryService employeeSalaryService,
            EmployeeHirerService employeeHirerService,
            EmployeeDataService employeeDataService)
        {
            _employeeLazinessService = employeeLazinessService;
            _employeeSalaryService = employeeSalaryService;
            _employeeHirerService = employeeHirerService;
            _employeeDataService = employeeDataService;
        }
    }
}