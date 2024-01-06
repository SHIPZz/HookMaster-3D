using CodeBase.UI.MiningFarm;

namespace CodeBase.UI.Buttons
{
    public class OpenMiningFarmWindowButton : ButtonOpenerBase
    {
        protected override void Open()
        {
            WindowService.Open<MiningFarmWindow>();
            gameObject.SetActive(false);
        }
    }
}