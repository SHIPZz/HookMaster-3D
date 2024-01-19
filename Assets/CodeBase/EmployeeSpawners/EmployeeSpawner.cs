using System;
using CodeBase.Services.Employees;
using CodeBase.Services.Factories.Employee;
using UnityEngine;
using Zenject;

namespace CodeBase.EmployeeSpawners
{
    public class EmployeeSpawner : MonoBehaviour
    {
        private IEmployeeFactory _employeeFactory;
        private EmployeeService _employeeService;

        [Inject]
        private void Construct(IEmployeeFactory employeeFactory, EmployeeService employeeService)
        {
            _employeeService = employeeService;
            _employeeFactory = employeeFactory;
        }

        private void Awake()
        {
            // Employee employee = _employeeFactory.Create(transform.position);
            // _employeeProvider.Employees.Add(employee);
        }
    }
}