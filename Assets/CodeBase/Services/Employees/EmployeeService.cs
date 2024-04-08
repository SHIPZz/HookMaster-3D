using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CodeBase.Data;
using CodeBase.Extensions;
using CodeBase.Gameplay.Employees;
using CodeBase.Gameplay.TableSystem;
using CodeBase.Services.Factories.Employee;
using CodeBase.Services.Providers.Tables;
using CodeBase.SO.Employee;

namespace CodeBase.Services.Employees
{
    public class EmployeeService
    {
        private readonly EmployeeDataService _employeeDataService;
        private readonly TableService _tableService;
        private readonly IEmployeeFactory _employeeFactory;

        private List<Employee> _employees = new();
        private EmployeeStatsSO _employeeStatsSo;

        public IReadOnlyList<Employee> Employees => _employees;

        public event Action<Employee> EmployeeUpdated;

        public EmployeeService(EmployeeDataService employeeDataService, TableService tableService, IEmployeeFactory employeeFactory, EmployeeStatsSO employeeStatsSo)
        {
            _employeeStatsSo = employeeStatsSo;
            _employeeFactory = employeeFactory;
            _tableService = tableService;
            _employeeDataService = employeeDataService;
        }

        public void Init(List<EmployeeData> playerDataPurchasedEmployees)
        {
            foreach (EmployeeData employeeData in playerDataPurchasedEmployees)
            {
                Table targetTable = _tableService.Tables.FirstOrDefault(x => x.Id == employeeData.TableId);

                if (targetTable == null)
                    continue;

                Employee targetEmployee = _employeeFactory.Create(employeeData, targetTable, true);
                _employees.Add(targetEmployee);
            }
        }

        public bool UpdateMaxReached(string id)
        {
            var employee = Employees.FirstOrDefault(x => x.Id == id);

            return employee.ProcessPaperTime == _employeeStatsSo.MinPaperProcessTime;
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
                employee.SetIsUpgrading(employeeData.IsUpgrading)
                    .SetProcessPaperTime(employeeData.PaperProcessTime);

                EmployeeUpdated?.Invoke(employee);
            }
        }

        public void Add(Employee employee) =>
            _employees.Add(employee);

        public Employee Get(string id) =>
            Employees.FirstOrDefault(x => x.Id == id);
    }
}