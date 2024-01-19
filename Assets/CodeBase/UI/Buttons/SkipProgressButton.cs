using CodeBase.Data;
using CodeBase.Services.Employees;
using CodeBase.UI.SkipProgress;
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
        private void Construct(EmployeeDataService employeeDataService) => 
            _employeeDataService = employeeDataService;

        public void SetEmployeeData(EmployeeData employeeData) => 
            _employeeData = employeeData;

        protected override void Open()
        {
            var speedUpWindow = WindowService.Get<SpeedUpWindow>();
            
            WindowService.Close<SkipProgressSliderWindow>();
            UpgradeEmployeeData targetUpgradeEmployeeData = _employeeDataService.GetUpgradeEmployeeData(_employeeData.Id);
            speedUpWindow.Init(targetUpgradeEmployeeData);
            speedUpWindow.Open();
        }
    }
}