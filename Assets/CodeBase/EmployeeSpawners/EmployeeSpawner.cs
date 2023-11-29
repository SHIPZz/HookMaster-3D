using System;
using CodeBase.Gameplay.EmployeeSystem;
using CodeBase.Services.Factories.Employee;
using CodeBase.Services.Providers.EmployeeProvider;
using UnityEngine;
using Zenject;

namespace CodeBase.EmployeeSpawners
{
    public class EmployeeSpawner : MonoBehaviour
    {
        private IEmployeeFactory _employeeFactory;
        private EmployeeProvider _employeeProvider;

        [Inject]
        private void Construct(IEmployeeFactory employeeFactory, EmployeeProvider employeeProvider)
        {
            _employeeProvider = employeeProvider;
            _employeeFactory = employeeFactory;
        }

        private void Awake()
        {
            // Employee employee = _employeeFactory.Create(transform.position);
            // _employeeProvider.Employees.Add(employee);
        }
    }
}