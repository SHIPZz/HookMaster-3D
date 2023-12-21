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