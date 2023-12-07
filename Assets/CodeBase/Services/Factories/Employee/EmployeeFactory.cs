using System;
using System.Collections.Generic;
using CodeBase.Constant;
using CodeBase.Data;
using CodeBase.Gameplay.EmployeeSystem;
using CodeBase.Services.DataService;
using CodeBase.Services.Providers.Asset;
using CodeBase.Services.Providers.Location;
using CodeBase.Services.WorldData;
using CodeBase.SO.Employee;
using CodeBase.SO.Office;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace CodeBase.Services.Factories.Employee
{
    public class EmployeeFactory : IEmployeeFactory
    {
        private readonly IAssetProvider _assetProvider;
        private readonly DiContainer _diContainer;
        private readonly List<string> _employeeNames;
        private readonly OfficeStaticDataService _officeStaticDataService;
        private readonly LocationProvider _locationProvider;
        private readonly IWorldDataService _worldDataService;

        public EmployeeFactory(IAssetProvider assetProvider,
            DiContainer diContainer,
            EmployeeNameSO employeeNameSo,
            OfficeStaticDataService officeStaticDataService,
            LocationProvider locationProvider,
            IWorldDataService worldDataService)
        {
            _worldDataService = worldDataService;
            _locationProvider = locationProvider;
            _officeStaticDataService = officeStaticDataService;
            _employeeNames = employeeNameSo.Names;
            _diContainer = diContainer;
            _assetProvider = assetProvider;
        }

        public EmployeeData Create()
        {
            CodeBase.Data.WorldData worldData = _worldDataService.WorldData;

            int qualificationType = worldData.PlayerData.QualificationType;
            OfficeSO officeSO = _officeStaticDataService.Get(qualificationType);

            var potentialEmployeeData = new EmployeeData
            {
                Name = _employeeNames[Random.Range(0, _employeeNames.Count)],
                QualificationType = qualificationType,
                Salary = Random.Range(officeSO.MinSalary, officeSO.MaxSalary),
                Profit = Random.Range(officeSO.MinProfit, officeSO.MaxSalary),
                Guid = Guid.NewGuid(),
            };

            return potentialEmployeeData;
        }

        public Gameplay.EmployeeSystem.Employee Create(EmployeeData employeeData,
            Vector3 targetPosition)
        {
            var employeePrefab = _assetProvider.Get<Gameplay.EmployeeSystem.Employee>(AssetPath.Employee);

            var employee = _diContainer.InstantiatePrefabForComponent<Gameplay.EmployeeSystem.Employee>(employeePrefab,
                _locationProvider.EmployeeSpawnPoint.position,
                Quaternion.identity,
                null);

            employee.Name = employeeData.Name;
            employee.QualificationType = employeeData.QualificationType;
            employee.Salary = employeeData.Salary;
            employee.Profit = employeeData.Profit;
            employee.Guid = employeeData.Guid;
            employee.transform.position = targetPosition;
            return employee;
        }
    }

    public interface IEmployeeFactory
    {
        Gameplay.EmployeeSystem.Employee Create(EmployeeData employeeData, Vector3 targetPosition);

        EmployeeData Create();
    }
}