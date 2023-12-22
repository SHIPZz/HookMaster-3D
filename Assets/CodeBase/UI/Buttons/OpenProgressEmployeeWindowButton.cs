using System.Linq;
using CodeBase.Data;
using CodeBase.Services.Employee;
using CodeBase.Services.Providers.Camera;
using CodeBase.Services.Providers.EmployeeProvider;
using CodeBase.Services.WorldData;
using CodeBase.UI.SpeedUp;
using CodeBase.UI.UpgradeEmployee;
using UnityEngine;
using Zenject;

namespace CodeBase.UI.Buttons
{
    public class OpenProgressEmployeeWindowButton : ButtonOpenerBase
    {
        private EmployeeData _employeeData;
        private CameraProvider _cameraProvider;
        private EmployeeService _employeeService;
        private EmployeeDataService _employeeDataService;

        [Inject]
        private void Construct(CameraProvider cameraProvider,
            EmployeeDataService employeeDataService, EmployeeService employeeService)
        {
            _employeeDataService = employeeDataService;
            _employeeService = employeeService;
            _cameraProvider = cameraProvider;
        }

        public void SetEmployeeData(EmployeeData employeeData) =>
            _employeeData = employeeData;

        protected override void Open()
        {
            var speedUpWindow = WindowService.Get<SkipProgressSliderWindow>();

            UpgradeEmployeeData upgradeEmployeeData = _employeeDataService.GetUpgradeEmployeeData(_employeeData.Id);
            _employeeService.SetUpgrade(_employeeData.Id, true);
            
            speedUpWindow.Init(upgradeEmployeeData, upgradeEmployeeData.LastUpgradeTime,
                upgradeEmployeeData.LastUpgradeWindowOpenedTime,
                Quaternion.LookRotation(_cameraProvider.Camera.transform.forward));
            speedUpWindow.Open();
            WindowService.Close<UpgradeEmployeeWindow>();
        }
    }
}