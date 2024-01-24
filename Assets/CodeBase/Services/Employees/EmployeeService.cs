using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Data;
using CodeBase.Extensions;
using CodeBase.Gameplay.Employees;

namespace CodeBase.Services.Employees
{
    public class EmployeeService
    {
        public List<Employee> Employees = new();
        private readonly EmployeeDataService _employeeDataService;

        public event Action<Employee> Recovered;

        public EmployeeService(EmployeeDataService employeeDataService)
        {
            _employeeDataService = employeeDataService;
        }

        public void SetUpgrade(string id, bool isUpgrading)
        {
            foreach (Employee employee in Employees.Where(employee => employee.Id == id))
            {
                employee.SetUpgrading(isUpgrading);
            }
        }

        public void Upgrade(EmployeeData employeeData, Action<EmployeeData> onComplete = null)
        {
            _employeeDataService.UpgradeEmployeeData(employeeData, onComplete);
            
            foreach (Employee employee in Employees.Where(employee => employee.Id == employeeData.Id))
            {
                employee.SetProfit(employeeData.Profit)
                    .SetSalary(employeeData.Salary)
                    .SetQualificationType(employeeData.QualificationType)
                    .SetIsUpgrading(employeeData.IsUpgrading);
            }
        }

        public Employee Get(string id) =>
            Employees.FirstOrDefault(x => x.Id == id);
    }
}