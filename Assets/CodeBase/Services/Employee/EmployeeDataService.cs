using System.Collections.Generic;
using System.Linq;
using CodeBase.Constant;
using CodeBase.Data;
using CodeBase.Extensions;
using CodeBase.Services.WorldData;
using UnityEngine;

namespace CodeBase.Services.Employee
{
    public class EmployeeDataService
    {
        private readonly IWorldDataService _worldDataService;

        public EmployeeDataService(IWorldDataService worldDataService)
        {
            _worldDataService = worldDataService;
        }

        public EmployeeData Get(string id) =>
            _worldDataService.WorldData.PlayerData.PurchasedEmployees.FirstOrDefault(x => x.Id == id);

        public void RecountUpgradePriceEmployee(UpgradeEmployeeData targetUpgradeEmployeeData)
        {
            var newUpgradeCost = targetUpgradeEmployeeData.UpgradeCost *
                                 _worldDataService.WorldData.PlayerData.QualificationType
                                 + MultiplyValueConstants.AdditionalUpgradeCost;

            targetUpgradeEmployeeData.SetUpgradeCost(newUpgradeCost);
            SaveUpgradeEmployeeData(targetUpgradeEmployeeData);
        }

        public UpgradeEmployeeData GetUpgradeEmployeeData(string id)
        {
            UpgradeEmployeeData targetUpgradeEmployeeData = GetTargetUpgradeEmployeeData(id);

            if (targetUpgradeEmployeeData == null)
            {
                EmployeeData employeeData =
                    _worldDataService.WorldData.PlayerData.PurchasedEmployees.FirstOrDefault(x => x.Id == id);

                targetUpgradeEmployeeData = new()
                {
                    EmployeeData = employeeData
                };
            }

            return targetUpgradeEmployeeData;
        }
        
        public void TryAddUpgradeEmployeeData(UpgradeEmployeeData upgradeEmployeeData)
        {
            if (_worldDataService.WorldData.UpgradeEmployeeDatas
                    .Count(x => x.EmployeeData.Id == upgradeEmployeeData.EmployeeData.Id) == 0)
            {
                _worldDataService.WorldData.UpgradeEmployeeDatas.Add(upgradeEmployeeData);
            }
        }

        public void OverwritePurchasedEmployeeData(EmployeeData employeeData)
        {
            List<EmployeeData> purchasedEmployees = _worldDataService.WorldData.PlayerData.PurchasedEmployees.ToList();

            int targetIndex = purchasedEmployees.FindIndex(x => x.Id == employeeData.Id);

            if (targetIndex != -1)
                purchasedEmployees[targetIndex] = employeeData;

            _worldDataService.WorldData.PlayerData.PurchasedEmployees = purchasedEmployees;
            _worldDataService.Save();
        }

        public void SavePurchasedEmployee(EmployeeData purchasedEmployee)
        {
            _worldDataService.WorldData.PlayerData.PurchasedEmployees.Add(purchasedEmployee);
            _worldDataService.WorldData.PotentialEmployeeList.RemoveAll(x => x.Id == purchasedEmployee.Id);
            _worldDataService.Save();
        }

        public void SaveUpgradeEmployeeData(UpgradeEmployeeData upgradeEmployeeData)
        {
            List<UpgradeEmployeeData> upgradeEmployeeDatas = _worldDataService.WorldData.UpgradeEmployeeDatas;

            if (upgradeEmployeeDatas.Count(x => x.EmployeeData.Id == upgradeEmployeeData.EmployeeData.Id) > 0)
                upgradeEmployeeDatas.RemoveAll(x =>
                    x.EmployeeData.Id == upgradeEmployeeData.EmployeeData.Id);

            _worldDataService.WorldData.UpgradeEmployeeDatas.Add(upgradeEmployeeData);
            _worldDataService.Save();
        }
        
        private UpgradeEmployeeData GetTargetUpgradeEmployeeData(string id) =>
            _worldDataService.WorldData
                .UpgradeEmployeeDatas
                .FirstOrDefault(x =>
                    x.EmployeeData.Id == id);
    }
}