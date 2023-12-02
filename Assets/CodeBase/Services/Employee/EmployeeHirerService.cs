using System.Linq;
using CodeBase.Gameplay.TableSystem;
using CodeBase.Services.Factories.Employee;
using CodeBase.Services.Providers.EmployeeProvider;
using CodeBase.Services.Providers.Tables;

namespace CodeBase.Services.EmployeeHirer
{
    public class EmployeeHirerService
    {
        private readonly EmployeeProvider _employeeProvider;
        private readonly TableService _tableService;
        private readonly IEmployeeFactory _employeeFactory;

        public EmployeeHirerService(EmployeeProvider employeeProvider, TableService tableService, IEmployeeFactory employeeFactory)
        {
            _employeeFactory = employeeFactory;
            _tableService = tableService;
            _employeeProvider = employeeProvider;
        }

        public void Hire(PotentialEmployeeData potentialEmployeeData)
        {
            Table freeTable = _tableService.Tables.FirstOrDefault(x => x.IsFree);
            freeTable.SetCondition(false);

            _employeeFactory.Create(potentialEmployeeData, freeTable.transform.position);
        }
    }
}