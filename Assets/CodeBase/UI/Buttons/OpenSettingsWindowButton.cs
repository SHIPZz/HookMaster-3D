using CodeBase.UI.Employee;
using CodeBase.UI.Hud;
using CodeBase.UI.Settings;

namespace CodeBase.UI.Buttons
{
    public class OpenSettingsWindowButton : ButtonOpenerBase
    {
        protected override void Open()
        {
            WindowService.Close<HudWindow>();
            WindowService.Open<SettingsWindow>();
        }
    }
}