using System;
using System.Collections.Generic;
using CodeBase.Gameplay.TableSystem;
using UnityEngine;

namespace CodeBase.Gameplay.EmployeeSystem
{
    public class Employee : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private List<Material> _materials;

        public Guid Guid;
        public int QualificationType;
        public int Salary;
        public int Profit;
        public string Name;
        public Table Table;
        public Guid TableId;
        public bool IsWorking { get; private set; }

        public void StartWorking()
        {
            IsWorking = true;
            _meshRenderer.material = _materials[1];
        }

        public void StopWorking()
        {
            IsWorking = false;
            _meshRenderer.material = _materials[0];
        }
    }
}