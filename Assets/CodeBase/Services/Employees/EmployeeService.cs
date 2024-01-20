using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Data;
using CodeBase.Extensions;

namespace CodeBase.Services.Employees
{
    public class EmployeeService
    {
        public List<Gameplay.Employees.Employee> Employees = new();
        private readonly EmployeeDataService _employeeDataService;

        public EmployeeService(EmployeeDataService employeeDataService)
        {
            _employeeDataService = employeeDataService;
        }

        public void SetUpgrade(string id, bool isUpgrading)
        {
            foreach (Gameplay.Employees.Employee employee in Employees.Where(employee => employee.Id == id))
            {
                employee.SetUpgrading(isUpgrading);
            }
        }

        public void Upgrade(EmployeeData employeeData, Action<EmployeeData> onComplete = null)
        {
            _employeeDataService.UpgradeEmployeeData(employeeData, onComplete);
            
            foreach (Gameplay.Employees.Employee employee in Employees.Where(employee => employee.Id == employeeData.Id))
            {
                employee.SetProfit(employeeData.Profit)
                    .SetSalary(employeeData.Salary)
                    .SetQualificationType(employeeData.QualificationType)
                    .SetIsUpgrading(employeeData.IsUpgrading);
            }
        }

        public Gameplay.Employees.Employee Get(string id) =>
            Employees.FirstOrDefault(x => x.Id == id);
    }
}