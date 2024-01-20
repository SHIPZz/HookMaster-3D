using System.Collections.Generic;
using System.Linq;
using CodeBase.Constant;
using CodeBase.Enums;
using CodeBase.Gameplay.Employees;
using UnityEngine;

namespace CodeBase.Services.Employees
{
    public class EmployeeStaticDataService
    {
        private readonly Dictionary<EmployeeTypeId, Employee> _employees;

        public EmployeeStaticDataService()
        {
            _employees = Resources.LoadAll<Employee>(AssetPath.Employees)
                .ToDictionary(x => x.EmployeeTypeId, x => x);
        }

        public EmployeeTypeId GetRandomId()
        {
          List<EmployeeTypeId> ids = _employees.Keys.ToList();

          var randomId = Random.Range(0, ids.Count);

          return ids[randomId];
        }
    }
}