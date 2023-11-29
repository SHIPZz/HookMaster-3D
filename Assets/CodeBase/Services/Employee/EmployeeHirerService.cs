using System;
using System.Linq;
using CodeBase.Gameplay.EmployeeSystem;
using CodeBase.Gameplay.TableSystem;
using CodeBase.Services.Providers.EmployeeProvider;
using CodeBase.Services.Providers.Tables;
using UnityEngine;

namespace CodeBase.Services.EmployeeHirer
{
    public class EmployeeHirerService
    {
        private readonly EmployeeProvider _employeeProvider;
        private readonly TableProvider _tableProvider;
        
        public EmployeeHirerService(EmployeeProvider employeeProvider, TableProvider tableProvider)
        {
            _tableProvider = tableProvider;
            _employeeProvider = employeeProvider;
        }

        public void TryHire(string name)
        {
            foreach (Employee employee in _employeeProvider.Employees)
            {
                if (employee.Name != name) 
                    continue;

                foreach (Table table in _tableProvider.Tables.Where(table => table.IsFree))
                {
                    employee.GetComponent<EmployeeMovement>().SetTarget(table.transform.position);
                    return;
                }
            }
        }
    }
}