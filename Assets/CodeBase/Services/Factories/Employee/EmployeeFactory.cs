using System;
using System.Collections.Generic;
using CodeBase.Constant;
using CodeBase.Data;
using CodeBase.Extensions;
using CodeBase.Gameplay.Employees;
using CodeBase.Gameplay.TableSystem;
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
        private readonly OfficeStaticDataService _officeStaticDataService;
        private readonly LocationProvider _locationProvider;
        private readonly IWorldDataService _worldDataService;
        
        private readonly List<string> _employeeNames = new();

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
            _diContainer = diContainer;
            _assetProvider = assetProvider;
            
            FillNames(employeeNameSo);
        }

        public EmployeeData Create()
        {
            CodeBase.Data.WorldData worldData = _worldDataService.WorldData;

            int qualificationType = worldData.PlayerData.QualificationType;
            OfficeSO officeSO = _officeStaticDataService.Get(qualificationType);

            
            var targetName = _employeeNames[Random.Range(0, _employeeNames.Count)];

            var potentialEmployeeData = new EmployeeData
            {
                Name =  targetName,
                QualificationType = qualificationType,
                Salary = Random.Range(officeSO.MinSalary, officeSO.MaxSalary),
                Profit = Random.Range(officeSO.MinProfit, officeSO.MaxSalary),
                Guid = Guid.NewGuid(),
            };

            _employeeNames.Remove(targetName);
            potentialEmployeeData.Id = potentialEmployeeData.Guid.ToString();

            return potentialEmployeeData;
        }

        public Gameplay.Employees.Employee Create(EmployeeData employeeData,
            Table targetTable, bool isSit)
        {
            var employeePrefab = _assetProvider.Get<Gameplay.Employees.Employee>(AssetPath.Employee);

            var employee = _diContainer.InstantiatePrefabForComponent<Gameplay.Employees.Employee>(employeePrefab,
                _locationProvider.EmployeeSpawnPoint.position,
                Quaternion.identity,
                null);

            employee.SetName(employeeData.Name)
                .SetQualificationType(employeeData.QualificationType)
                .SetProfit(employeeData.Profit)
                .SetSalary(employeeData.Salary)
                .SetGuid(employeeData.Guid)
                .SetId(employeeData.Id)
                .SetTableId(employeeData.TableId)
                .SetIsUpgrading(employeeData.IsUpgrading)
                .SetBurn(employeeData.IsBurned);

            EmployeeMovement employeeMovement = employee.GetComponent<EmployeeMovement>();
            employeeMovement.SetTable(targetTable);

            if (isSit)
                employeeMovement.SitAndWork();

            return employee;
        }

        private void FillNames(EmployeeNameSO employeeNameSo)
        {
            foreach (var name in employeeNameSo.Names)
            {
                _employeeNames.Add(name);
            }
        }
    }

    public interface IEmployeeFactory
    {
        Gameplay.Employees.Employee Create(EmployeeData employeeData, Table targetTable, bool isSit);

        EmployeeData Create();
    }
}