using System;
using System.Linq;
using CodeBase.Data;
using CodeBase.Extensions;
using CodeBase.Gameplay.Employees;
using CodeBase.Gameplay.TableSystem;
using CodeBase.Services.Factories.Employee;
using CodeBase.Services.Providers.Tables;

namespace CodeBase.Services.Employees
{
    public class EmployeeHirerService
    {
        private readonly EmployeeService _employeeService;
        private readonly TableService _tableService;
        private readonly IEmployeeFactory _employeeFactory;
        private readonly EmployeeDataService _employeeDataService;

        public event Action<Employee> EmployeeHired;

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
            foreach (Employee employee in _employeeService.Employees.Where(employee => !employee.gameObject.activeSelf))
            {
                employee.gameObject.SetActive(true);
            }
            
            EmployeeHired?.Invoke(_employeeService.Employees.FirstOrDefault());
        }

        public void SetEmployee(EmployeeData employeeData)
        {
            Table freeTable = _tableService.Tables.FirstOrDefault(x => x.IsFree);
            freeTable.SetIsFree(false);

            Employee employee =  _employeeFactory.Create(employeeData, freeTable, false);
            employee.gameObject.SetActive(false);
            employee.TableId = freeTable.Id;
            _employeeService.Employees.Add(employee);
            
            _employeeDataService.SavePurchasedEmployee(employee.ToEmployeeData());
        }
    }
}