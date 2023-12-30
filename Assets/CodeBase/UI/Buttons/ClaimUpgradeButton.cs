using CodeBase.Data;
using CodeBase.Services.Employee;
using CodeBase.UI.Upgrade;
using CodeBase.UI.UpgradeEmployee;
using Zenject;

namespace CodeBase.UI.Buttons
{
    public class ClaimUpgradeButton : ButtonOpenerBase
    {
        private EmployeeDataService _employeeDataService;
        private EmployeeData _employeeData;

        [Inject]
        private void Construct(EmployeeDataService employeeDataService)
        {
            _employeeDataService = employeeDataService;
        }

        public void SetEmployeeData(EmployeeData employeeData)
        {
            _employeeData = employeeData;
        }
        
        protected override void Open()
        {
          var targetWindow =  WindowService.Get<UpgradeEmployeeCompletedWindow>();
          targetWindow.Init(_employeeData);
          targetWindow.Open();
        }
    }
}