using CodeBase.Data;
using CodeBase.UI.Hud;
using CodeBase.UI.SkipProgress;
using CodeBase.UI.Upgrade;

namespace CodeBase.UI.Buttons
{
    public class ClaimUpgradeButton : ButtonOpenerBase
    {
        private EmployeeData _employeeData;

        public void SetEmployeeData(EmployeeData employeeData)
        {
            _employeeData = employeeData;
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