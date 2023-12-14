using System;
using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Gameplay.EmployeeSystem
{
    public class Employee : MonoBehaviour
    {
        [SerializeField] private SkinnedMeshRenderer _meshRenderer;
        [SerializeField] private List<Material> _materials;

        public Guid Guid;
        public int QualificationType;
        public int Salary;
        public int Profit;
        public string Name;
        public string TableId;
        public bool IsWorking { get; private set; }

        public void StartWorking()
        {
            IsWorking = true;
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