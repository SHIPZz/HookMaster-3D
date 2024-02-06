using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CodeBase.Data;
using CodeBase.Extensions;
using CodeBase.Gameplay.Employees;
using CodeBase.Gameplay.PaperSystem;
using CodeBase.Gameplay.TableSystem;
using CodeBase.Services.Providers.Tables;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CodeBase.Services.Employees
{
    public class EmployeeService : IDisposable
    {
        public List<Employee> Employees = new();
        private readonly EmployeeDataService _employeeDataService;
        private readonly TableService _tableService;


        private CancellationTokenSource _cancellationToken = new();

        public event Action<Employee> EmployeeUpdated;

        public EmployeeService(EmployeeDataService employeeDataService, TableService tableService)
        {
            _tableService = tableService;
            _employeeDataService = employeeDataService;
        }

        public void SubscribeTableEvents() =>
            _tableService.Tables.ForEach(x => x.PaperAdded += OnTablePaperAdded);

        public void Dispose() =>
            _tableService.Tables.ForEach(x => x.PaperAdded -= OnTablePaperAdded);

        public void CancelProcessingPaper(Table table)
        {
            Employee targetEmployee = GetEmployeeByTable(table);
            targetEmployee?.CancelProcessPaper();
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

                EmployeeUpdated?.Invoke(employee);
            }
        }

        public Employee Get(string id) =>
            Employees.FirstOrDefault(x => x.Id == id);

        private async void OnTablePaperAdded(Table table)
        {
            while (GetEmployeeByTable(table) == null)
            {
                await UniTask.Yield();
            }

            Employee targetEmployee = GetEmployeeByTable(table);

            targetEmployee.CancelProcessPaper();

            try
            {
                await targetEmployee.ProcessPaper(table).AttachExternalCancellation(_cancellationToken.Token);
            }
            catch (Exception e) { }
        }

        private Employee GetEmployeeByTable(Table table) =>
            Employees.FirstOrDefault(x => x.TableId == table.Id);
    }
}