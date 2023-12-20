using System.Linq;
using CodeBase.Data;
using CodeBase.Extensions;
using CodeBase.Gameplay.TableSystem;
using CodeBase.Services.Factories.Employee;
using CodeBase.Services.Providers.EmployeeProvider;
using CodeBase.Services.Providers.Tables;
using CodeBase.Services.WorldData;
using UnityEngine;

namespace CodeBase.Services.Employee
{
    public class EmployeeHirerService
    {
        private readonly EmployeeProvider _employeeProvider;
        private readonly TableService _tableService;
        private readonly IEmployeeFactory _employeeFactory;
        private readonly EmployeeDataService _employeeDataService;

        public EmployeeHirerService(EmployeeProvider employeeProvider, 
            TableService tableService,
            IEmployeeFactory employeeFactory,
            EmployeeDataService employeeDataService)
        {
            _employeeDataService = employeeDataService;
            _employeeFactory = employeeFactory;
            _tableService = tableService;
            _employeeProvider = employeeProvider;
        }

        public void ActivateCreatedEmployees()
        {
            foreach (Gameplay.EmployeeSystem.Employee employee in _employeeProvider.Employees.Where(employee => !employee.gameObject.activeSelf))
            {
                employee.gameObject.SetActive(true);
            }
        }

        public void SetEmployee(EmployeeData employeeData)
        {
            Table freeTable = _tableService.Tables.FirstOrDefault(x => x.IsFree);
            freeTable.SetIsFree(false);

            Gameplay.EmployeeSystem.Employee employee =  _employeeFactory.Create(employeeData, freeTable);
            employee.gameObject.SetActive(false);
            employee.TableId = freeTable.Id;
            _employeeProvider.Employees.Add(employee);
            
            _employeeDataService.SavePurchasedEmployee(employee.ToEmployeeData());
        }
    }
}