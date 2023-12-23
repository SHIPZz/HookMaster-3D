using System.Linq;
using CodeBase.Data;
using CodeBase.Services.Employee;
using CodeBase.Services.Providers.EmployeeProvider;
using CodeBase.UI.UpgradeEmployee;
using Zenject;

namespace CodeBase.UI.Buttons
{
    public class OpenProgressEmployeeWindowButton : ButtonOpenerBase
    {
        private EmployeeData _employeeData;
        private EmployeeService _employeeService;
        private EmployeeDataService _employeeDataService;

        [Inject]
        private void Construct(EmployeeDataService employeeDataService, EmployeeService employeeService)
        {
            _employeeDataService = employeeDataService;
            _employeeService = employeeService;
        }

        public void SetEmployeeData(EmployeeData employeeData) =>
            _employeeData = employeeData;

        protected override void Open()
        {
            UpgradeEmployeeData upgradeEmployeeData = _employeeDataService.GetUpgradeEmployeeData(_employeeData.Id);
            _employeeService.SetUpgrade(_employeeData.Id, true);
            Gameplay.EmployeeSystem.Employee targetEmployee = _employeeService.Get(_employeeData.Id);
            targetEmployee.SkipEmployeeProgressUIHandler.ActivateWindow(upgradeEmployeeData);
            WindowService.Close<UpgradeEmployeeWindow>();
        }
    }
}