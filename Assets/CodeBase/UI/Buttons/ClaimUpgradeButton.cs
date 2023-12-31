using CodeBase.Data;
using CodeBase.UI.Upgrade;

namespace CodeBase.UI.Buttons
{
    public class ClaimUpgradeButton : ButtonOpenerBase
    {
        private EmployeeData _employeeData;

        public void SetEmployeeData(EmployeeData employeeData)
        {
            _employeeData = employeeData;
            print(_employeeData.Name);
        }
        
        protected override void Open()
        {
          var targetWindow =  WindowService.Get<UpgradeEmployeeCompletedWindow>();
          WindowService.Close<SkipProgressSliderWindow>();
          targetWindow.Init(_employeeData);
          targetWindow.Open();
        }
    }
}