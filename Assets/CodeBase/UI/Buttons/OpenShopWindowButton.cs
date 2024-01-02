using CodeBase.UI.Shop;

namespace CodeBase.UI.Buttons
{
    public class OpenShopWindowButton : ButtonOpenerBase
    {
        protected override void Open()
        {
            WindowService.CloseAll();
            WindowService.Open<ShopWindow>();
        }
    }
}