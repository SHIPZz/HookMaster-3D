using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Constant;
using CodeBase.Gameplay.EmployeeSystem;
using CodeBase.Services.Data;
using CodeBase.Services.Providers.Asset;
using CodeBase.Services.Providers.Location;
using CodeBase.Services.SaveSystem;
using CodeBase.SO.Employee;
using Cysharp.Threading.Tasks;
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
        private readonly ISaveSystem _saveSystem;
        private readonly LocationProvider _locationProvider;

        public EmployeeFactory(IAssetProvider assetProvider,
            DiContainer diContainer,
            EmployeeNameSO employeeNameSo,
            OfficeStaticDataService officeStaticDataService,
            ISaveSystem saveSystem,
            LocationProvider locationProvider)
        {
            _locationProvider = locationProvider;
            _saveSystem = saveSystem;
            _officeStaticDataService = officeStaticDataService;
            _employeeNames = employeeNameSo.Names;
            _diContainer = diContainer;
            _assetProvider = assetProvider;
        }

        public async UniTask<PotentialEmployeeData> Create()
        {
            CodeBase.Data.WorldData worldData = await _saveSystem.Load();

            int qualificationType = worldData.PlayerData.QualificationType;
            OfficeSO officeSO = _officeStaticDataService.Get(qualificationType);

            var potentialEmployeeData = new PotentialEmployeeData
            {
                Name = _employeeNames[Random.Range(0, _employeeNames.Count)],
                QualificationType = qualificationType,
                Salary = Random.Range(officeSO.MinSalary, officeSO.MaxSalary),
                Profit = Random.Range(officeSO.MinProfit, officeSO.MaxSalary),
                Guid = Guid.NewGuid()
            };

            return potentialEmployeeData;
        }

        public Gameplay.EmployeeSystem.Employee Create(PotentialEmployeeData potentialEmployeeData,
            Vector3 targetPosition)
        {
            var employeePrefab = _assetProvider.Get<Gameplay.EmployeeSystem.Employee>(AssetPath.Employee);

            var employee = _diContainer.InstantiatePrefabForComponent<Gameplay.EmployeeSystem.Employee>(employeePrefab,
                _locationProvider.PlayerSpawnPoint.position,
                Quaternion.identity,
                null);

            employee.Name = potentialEmployeeData.Name;
            employee.QualificationType = potentialEmployeeData.QualificationType;
            employee.Salary = potentialEmployeeData.Salary;
            employee.Profit = potentialEmployeeData.Profit;
            employee.Guid = potentialEmployeeData.Guid;
            employee.GetComponent<EmployeeMovement>().SetTarget(targetPosition);
            return employee;
        }
    }

    public interface IEmployeeFactory
    {
        Gameplay.EmployeeSystem.Employee Create(PotentialEmployeeData potentialEmployeeData, Vector3 targetPosition);

        UniTask<PotentialEmployeeData> Create();
    }
}