using System;
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
        
        public static Employee SetName(this Employee employee, string name)
        {
            employee.Name = name;
            return employee;
        }
        
        public static Employee SetGuid(this Employee employee, Guid guid)
        {
            employee.Guid = guid;
            return employee;
        }

        public static Employee SetId(this Employee employee, string id)
        {
            employee.Id = id;
            return employee;
        }
        
        public static Employee SetIsUpgrading(this Employee employee, bool isUpgrading)
        {
            employee.IsUpgrading = isUpgrading;
            return employee;
        }

        public static Employee SetTableId(this Employee employee, string id)
        {
            employee.TableId = id;
            return employee;
        }
        
        public static Employee SetSalary(this Employee employee, float salary)
        {
            employee.Salary = salary;
            return employee;
        }
        
        public static Employee SetProfit(this Employee employee, float profit)
        {
            employee.Profit = profit;
            return employee;
        }

        public static Employee SetQualificationType(this Employee employee, int qualificationType)
        {
            employee.QualificationType = qualificationType;
            return employee;
        }

        public static EmployeeData SetSalary(this EmployeeData employeeData, float salary)
        {
            employeeData.Salary = salary;
            return employeeData;
        }
        
        public static EmployeeData SetProfit(this EmployeeData employeeData, float profit)
        {
            employeeData.Profit = profit;
            return employeeData;
        }

        public static EmployeeData SetQualificationType(this EmployeeData employeeData, int qualificationType)
        {
            employeeData.QualificationType = qualificationType;
            return employeeData;
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
