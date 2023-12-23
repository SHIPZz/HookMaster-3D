﻿using System.Collections.Generic;
using System.Linq;

namespace CodeBase.Services.Providers.EmployeeProvider
{
    public class EmployeeService
    {
        public List<Gameplay.EmployeeSystem.Employee> Employees = new();

        public void SetUpgrade(string id, bool isUpgrading)
        {
            foreach (Gameplay.EmployeeSystem.Employee employee in Employees.Where(employee => employee.Id == id))
            {
                employee.SetUpgrading(isUpgrading);
            }
        }

        public Gameplay.EmployeeSystem.Employee Get(string id) => 
            Employees.FirstOrDefault(x => x.Id == id);
    }
}