using CodeBase.UI.Employee;
using CodeBase.UI.Hud;

namespace CodeBase.UI.Buttons
{
    public class OpenEmployeeWindowButton : ButtonOpenerBase
    {
        protected override void Open()
        {
            WindowService.Close<HudWindow>();
            WindowService.Open<EmployeeWindow>();
        }
    }
}