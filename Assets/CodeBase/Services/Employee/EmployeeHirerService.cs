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
        private readonly EmployeeService _employeeService;
        private readonly TableService _tableService;
        private readonly IEmployeeFactory _employeeFactory;
        private readonly EmployeeDataService _employeeDataService;

        public EmployeeHirerService(EmployeeService employeeService, 
            TableService tableService,
            IEmployeeFactory employeeFactory,
            EmployeeDataService employeeDataService)
        {
            _employeeDataService = employeeDataService;
            _employeeFactory = employeeFactory;
            _tableService = tableService;
            _employeeService = employeeService;
        }

        public void ActivateCreatedEmployees()
        {
            foreach (Gameplay.EmployeeSystem.Employee employee in _employeeService.Employees.Where(employee => !employee.gameObject.activeSelf))
            {
                employee.gameObject.SetActive(true);
            }
        }

        public void SetEmployee(EmployeeData employeeData)
        {
            Table freeTable = _tableService.Tables.FirstOrDefault(x => x.IsFree);
            freeTable.SetIsFree(false);

            Gameplay.EmployeeSystem.Employee employee =  _employeeFactory.Create(employeeData, freeTable, false);
            employee.gameObject.SetActive(false);
            employee.TableId = freeTable.Id;
            _employeeService.Employees.Add(employee);
            
            _employeeDataService.SavePurchasedEmployee(employee.ToEmployeeData());
        }
    }
}