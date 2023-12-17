using CodeBase.UI.SpeedUp;
using CodeBase.UI.UpgradeEmployee;

namespace CodeBase.UI.Buttons
{
    public class UpgradeEmployeeButton : ButtonOpenerBase
    {
        protected override void Open()
        {
            WindowService.Close<UpgradeEmployeeWindow>();
            WindowService.Open<SpeedUpWindow>();
        }
    }
}