using System.Collections.Generic;
using System.Linq;
using CodeBase.Data;
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

        public UpgradeEmployeeData GetUpgradeEmployeeData(string id)
        {
            UpgradeEmployeeData targetUpgradeEmployeeData = _worldDataService.WorldData
                .UpgradeEmployeeDatas
                .FirstOrDefault(x =>
                    x.EmployeeData.Id == id);
            
            return targetUpgradeEmployeeData ?? new();
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

            if (upgradeEmployeeDatas.Contains(upgradeEmployeeData))
                upgradeEmployeeDatas.RemoveAll(x =>
                    x.EmployeeData.Id == upgradeEmployeeData.EmployeeData.Id);

            _worldDataService.WorldData.UpgradeEmployeeDatas.Add(upgradeEmployeeData);
            _worldDataService.Save();
        }
    }
}