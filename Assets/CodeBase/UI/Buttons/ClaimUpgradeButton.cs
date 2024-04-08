using CodeBase.Data;
using CodeBase.UI.Hud;
using CodeBase.UI.SkipProgress;
using CodeBase.UI.Upgrade;

namespace CodeBase.UI.Buttons
{
    public class ClaimUpgradeButton : ButtonOpenerBase
    {
        private EmployeeData _employeeData;
        private SkipProgressSliderWindow _skipProgressSliderWindow;

        public void SetEmployeeData(EmployeeData employeeData, SkipProgressSliderWindow skipProgressSliderWindow)
        {
            _skipProgressSliderWindow = skipProgressSliderWindow;
            _employeeData = employeeData;
        }
        
        protected override void Open()
        {
          var targetWindow =  WindowService.Get<UpgradeEmployeeCompletedWindow>();
          _skipProgressSliderWindow.Hide();
          targetWindow.Init(_employeeData);
          WindowService.OpenCreatedWindow<UpgradeEmployeeCompletedWindow>();
        }
    }
}