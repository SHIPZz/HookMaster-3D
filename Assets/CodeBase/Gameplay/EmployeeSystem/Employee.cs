using System;
using System.Collections.Generic;
using CodeBase.Data;
using CodeBase.Extensions;
using CodeBase.Services.Employee;
using CodeBase.Services.WorldData;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.EmployeeSystem
{
    public class Employee : MonoBehaviour
    {
        [SerializeField] private SkinnedMeshRenderer _meshRenderer;
        [SerializeField] private List<Material> _materials;

        public Guid Guid;
        public string Id;
        public int QualificationType;
        public int Salary;
        public int Profit;
        public string Name;
        public string TableId;
        public bool IsWorking;
        private EmployeeDataService _employeeDataService;

        [Inject]
        private void Construct( EmployeeDataService employeeDataService)
        {
            _employeeDataService = employeeDataService;
        }

        public void StartWorking()
        {
            IsWorking = true;
            _employeeDataService.OverwritePurchasedEmployeeData(this.ToEmployeeData());
        }

        public void DecreaseSalary(int amount)
        {
            Salary -= amount;
        }

        public void StopWorking()
        {
            IsWorking = false;
        }

        public void AddSalary(int salary)
        {
            Salary += salary;
        }
    }
}