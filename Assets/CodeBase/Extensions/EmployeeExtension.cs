using System;
using CodeBase.Constant;
using CodeBase.Data;
using CodeBase.Enums;
using CodeBase.Gameplay.Employees;
using UnityEngine;

namespace CodeBase.Extensions
{
    public static class EmployeeExtension
    {
        public static EmployeeData ToEmployeeData(this Employee employee)
        {
            var potentialEmployeeData = new EmployeeData
            {
                Guid = employee.Guid,
                Name = employee.Name,
                TableId = employee.TableId,
                Id = employee.Id,
                IsWorking = employee.IsWorking,
                IsUpgrading = employee.IsUpgrading,
                IsBurned = employee.IsBurned,
                EmployeeTypeId = employee.EmployeeTypeId
            };

            return potentialEmployeeData;
        }

        public static Employee SetName(this Employee employee, string name)
        {
            employee.Name = name;
            return employee;
        }

        public static Employee SetSkin(this Employee employee, Mesh mesh)
        {
            employee.Renderer.sharedMesh = mesh;
            return employee;
        }

        public static Employee SetId(this Employee employee, EmployeeTypeId employeeTypeId)
        {
            employee.EmployeeTypeId = employeeTypeId;
            return employee;
        }
        
        public static Employee SetBurn(this Employee employee, bool isBurn)
        {
            employee.IsBurned = isBurn;

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

        public static Employee SetProcessPaperTime(this Employee employee, float time)
        {
            employee.ProcessPaperTime = time;
            return employee;
        }

        public static Employee SetTableId(this Employee employee, string id)
        {
            employee.TableId = id;
            return employee;
        }

        public static EmployeeData SetIsUpgrading(this EmployeeData employeeData, bool isUpgrading)
        {
            employeeData.IsUpgrading = isUpgrading;
            return employeeData;
        }
        
        public static EmployeeData SetProcessPaperTime(this EmployeeData employeeData, float time)
        {
            employeeData.PaperProcessTime = time;
            return employeeData;
        }

        public static UpgradeEmployeeData SetUpgradeCost(this UpgradeEmployeeData upgradeEmployeeData, int price)
        {
            upgradeEmployeeData.UpgradeCost = price;
            return upgradeEmployeeData;
        }

        public static UpgradeEmployeeData SetCompleted(this UpgradeEmployeeData upgradeEmployeeData, bool isCompleted)
        {
            upgradeEmployeeData.Completed = isCompleted;
            return upgradeEmployeeData;
        }

        public static UpgradeEmployeeData Reset(this UpgradeEmployeeData upgradeEmployeeData)
        {
            upgradeEmployeeData.SetCompleted(false)
                .SetUpgradeStarted(false)
                .SetLastUpgradeTime(TimeConstantValue.SecondsInHour)
                .SetLastUpgradeWindowOpenedTime(0);
            return upgradeEmployeeData;
        }

        public static UpgradeEmployeeData SetLastUpgradeTime(this UpgradeEmployeeData upgradeEmployeeData,
            float lastUpgradeTime)
        {
            upgradeEmployeeData.LastUpgradeTime = lastUpgradeTime;
            return upgradeEmployeeData;
        }

        public static UpgradeEmployeeData SetUpgradeStarted(this UpgradeEmployeeData upgradeEmployeeData,
            bool isStarted)
        {
            upgradeEmployeeData.UpgradeStarted = isStarted;
            return upgradeEmployeeData;
        }

        public static UpgradeEmployeeData SetEmployeeData(this UpgradeEmployeeData upgradeEmployeeData,
            EmployeeData employeeData)
        {
            upgradeEmployeeData.EmployeeData = employeeData;
            return upgradeEmployeeData;
        }

        public static UpgradeEmployeeData SetLastUpgradeWindowOpenedTime(this UpgradeEmployeeData upgradeEmployeeData,
            long lastUpgradeWindowOpenedTime)
        {
            upgradeEmployeeData.LastUpgradeWindowOpenedTime = lastUpgradeWindowOpenedTime;
            return upgradeEmployeeData;
        }

        public static EmployeeData ToEmployeeData(this UpgradeEmployeeData upgradeEmployeeData)
        {
            var employeeData = new EmployeeData
            {
                Guid = upgradeEmployeeData.EmployeeData.Guid,
                Name = upgradeEmployeeData.EmployeeData.Name,
                TableId = upgradeEmployeeData.EmployeeData.TableId,
                Id = upgradeEmployeeData.EmployeeData.Id,
                IsWorking = upgradeEmployeeData.EmployeeData.IsWorking,
                IsUpgrading = upgradeEmployeeData.EmployeeData.IsUpgrading
            };

            return employeeData;
        }
    }
}