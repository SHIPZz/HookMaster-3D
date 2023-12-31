using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Data;
using CodeBase.Extensions;
using CodeBase.Services.Employee;

namespace CodeBase.Services.Providers.EmployeeProvider
{
    public class EmployeeService
    {
        public List<Gameplay.EmployeeSystem.Employee> Employees = new();

        private readonly EmployeeDataService _employeeDataService;

        public EmployeeService(EmployeeDataService employeeDataService)
        {
            _employeeDataService = employeeDataService;
        }

        public void SetUpgrade(string id, bool isUpgrading)
        {
            foreach (Gameplay.EmployeeSystem.Employee employee in Employees.Where(employee => employee.Id == id))
            {
                employee.SetUpgrading(isUpgrading);
            }
        }

        public void Upgrade(EmployeeData employeeData, Action<EmployeeData> onComplete = null)
        {
            _employeeDataService.UpgradeEmployeeData(employeeData, onComplete);
            
            foreach (Gameplay.EmployeeSystem.Employee employee in Employees.Where(employee => employee.Id == employeeData.Id))
            {
                employee.SetProfit(employeeData.Profit)
                    .SetSalary(employeeData.Salary)
                    .SetQualificationType(employeeData.QualificationType)
                    .SetIsUpgrading(employeeData.IsUpgrading);
            }
        }

        public Gameplay.EmployeeSystem.Employee Get(string id) =>
            Employees.FirstOrDefault(x => x.Id == id);
    }
}