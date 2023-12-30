﻿using CodeBase.Data;
using CodeBase.Services.Employee;
using CodeBase.UI.SpeedUp;
using UnityEngine;
using Zenject;

namespace CodeBase.UI.Buttons
{
    public class SkipProgressButton : ButtonOpenerBase
    {
        private EmployeeData _employeeData;
        private EmployeeDataService _employeeDataService;

        [Inject]
        private void Construct(EmployeeDataService employeeDataService)
        {
            _employeeDataService = employeeDataService;
        }

        public void SetEmployeeData(EmployeeData employeeData)
        {
            _employeeData = employeeData;
            Debug.Log(employeeData.Name);
        }

        protected override void Open()
        {
            var speedUpWindow = WindowService.Get<SpeedUpWindow>();

            UpgradeEmployeeData targetUpgradeEmployeeData =
                _employeeDataService.GetUpgradeEmployeeData(_employeeData.Id);

            speedUpWindow.Init(targetUpgradeEmployeeData, targetUpgradeEmployeeData.LastUpgradeTime);
            speedUpWindow.Open();
            WindowService.Close<SkipProgressSliderWindow>();
        }
    }
}