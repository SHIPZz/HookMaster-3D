using System;
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

        public void RecountUpgradePriceEmployee(UpgradeEmployeeData targetUpgradeEmployeeData)
        {
            var newUpgradeCost = targetUpgradeEmployeeData.UpgradeCost +
                                 _worldDataService.WorldData.PlayerData.QualificationType
                                 * MultiplyValueConstants.AdditionalUpgradeCost;

            targetUpgradeEmployeeData.SetUpgradeCost(newUpgradeCost);
            SaveUpgradeEmployeeData(targetUpgradeEmployeeData);
        }

        public void UpgradeEmployeeData(EmployeeData employeeData, Action<EmployeeData> onCompleted = null)
        {
            var targetSalary = employeeData.Salary +
                               _worldDataService.WorldData.PlayerData.QualificationType
                               * MultiplyValueConstants.AdditionalSalary;

            var targetProfit = employeeData.Profit +
                               _worldDataService.WorldData.PlayerData.QualificationType
                               * MultiplyValueConstants.AdditionalProfit;

            var targetQualificationType = employeeData.QualificationType;
            targetQualificationType++;

            employeeData.SetProfit(targetProfit).SetSalary(targetSalary)
                .SetIsUpgrading(false)
                .SetQualificationType(targetQualificationType);

            ResetUpdatedUpgradedData(employeeData.Id);

            OverwritePurchasedEmployeeData(employeeData);
            onCompleted?.Invoke(employeeData);
        }

        public UpgradeEmployeeData GetUpgradeEmployeeData(string id)
        {
            if (_worldDataService.WorldData.UpgradeEmployeeDatas.TryGetValue(id, out UpgradeEmployeeData data))
                return data;

            EmployeeData employeeData =
                _worldDataService.WorldData.PlayerData.PurchasedEmployees.FirstOrDefault(x => x.Id == id);

            data = new UpgradeEmployeeData() { EmployeeData = employeeData };
            return data;
        }

        public void TryAddUpgradeEmployeeData(UpgradeEmployeeData upgradeEmployeeData)
        {
            _worldDataService.WorldData.UpgradeEmployeeDatas.TryAdd(upgradeEmployeeData.EmployeeData.Id,
                upgradeEmployeeData);
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
            _worldDataService.WorldData.UpgradeEmployeeDatas[upgradeEmployeeData.EmployeeData.Id] = upgradeEmployeeData;
            _worldDataService.Save();
        }

        private void ResetUpdatedUpgradedData(string id)
        {
            _worldDataService.WorldData.UpgradeEmployeeDatas[id].Reset();

            _worldDataService.Save();
        }
    }
}