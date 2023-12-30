using CodeBase.Data;
using CodeBase.Gameplay.EmployeeSystem;

namespace CodeBase.Extensions
{
    public static class EmployeeExtension
    {
        public static EmployeeData ToEmployeeData(this Employee employee)
        {
            var potentialEmployeeData = new EmployeeData
            {
                Guid = employee.Guid,
                Profit = employee.Profit,
                Name = employee.Name,
                Salary = employee.Salary,
                QualificationType = employee.QualificationType,
                TableId = employee.TableId,
                Id = employee.Id,
                IsWorking = employee.IsWorking,
                IsUpgrading = employee.IsUpgrading
            };

            return potentialEmployeeData;
        }

        public static UpgradeEmployeeData ToUpgradeEmployeeData(this EmployeeData employeeData)
        {
            var upgradeEmployeeData = new UpgradeEmployeeData
            {
                EmployeeData = employeeData
            };

            return upgradeEmployeeData;
        }

        public static UpgradeEmployeeData SetUpgradeCost(this UpgradeEmployeeData upgradeEmployeeData, float price)
        {
            upgradeEmployeeData.UpgradeCost = price;
            return upgradeEmployeeData;
        }

        public static UpgradeEmployeeData SetCompleted(this UpgradeEmployeeData upgradeEmployeeData, bool isCompleted)
        {
            upgradeEmployeeData.Completed = isCompleted;
            return upgradeEmployeeData;
        }

        public static UpgradeEmployeeData SetLastUpgradeTime(this UpgradeEmployeeData upgradeEmployeeData, float lastUpgradeTime)
        {
            upgradeEmployeeData.LastUpgradeTime = lastUpgradeTime;
            return upgradeEmployeeData;
        }

        public static UpgradeEmployeeData SetUpgradeStarted(this UpgradeEmployeeData upgradeEmployeeData, bool isStarted)
        {
            upgradeEmployeeData.UpgradeStarted = isStarted;
            return upgradeEmployeeData;
        }

        public static UpgradeEmployeeData SetEmployeeData(this UpgradeEmployeeData upgradeEmployeeData, EmployeeData employeeData)
        {
            upgradeEmployeeData.EmployeeData = employeeData;
            return upgradeEmployeeData;
        }

        public static UpgradeEmployeeData SetLastUpgradeWindowOpenedTime(this UpgradeEmployeeData upgradeEmployeeData, long lastUpgradeWindowOpenedTime)
        {
            upgradeEmployeeData.LastUpgradeWindowOpenedTime = lastUpgradeWindowOpenedTime;
            return upgradeEmployeeData;
        }

        public static EmployeeData ToEmployeeData(this UpgradeEmployeeData upgradeEmployeeData)
        {
            var employeeData = new EmployeeData
            {
                Guid = upgradeEmployeeData.EmployeeData.Guid,
                Profit = upgradeEmployeeData.EmployeeData.Profit,
                Name = upgradeEmployeeData.EmployeeData.Name,
                Salary = upgradeEmployeeData.EmployeeData.Salary,
                QualificationType = upgradeEmployeeData.EmployeeData.QualificationType,
                TableId = upgradeEmployeeData.EmployeeData.TableId,
                Id = upgradeEmployeeData.EmployeeData.Id,
                IsWorking = upgradeEmployeeData.EmployeeData.IsWorking,
                IsUpgrading = upgradeEmployeeData.EmployeeData.IsUpgrading
            };

            return employeeData;
        }
    }
}
