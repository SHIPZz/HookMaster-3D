namespace CodeBase.UI.Buttons
{
    public class PlayCircleRouletteButton : ButtonOpenerBase
    {
        protected override void Open()
        {
            WindowService.CloseAll();
            WindowService.Open<CircleRouletteWindow>();
        }
    }
}