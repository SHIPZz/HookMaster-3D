using CodeBase.Gameplay.GameItems;
using CodeBase.UI.Roulette;
using UnityEngine;

namespace CodeBase.UI.Buttons
{
    public class OpenCircleRouletteWindowButton : ButtonOpenerBase
    {
        [SerializeField] private CircleRouletteItem _circleRouletteItem;
        
        protected override void Open()
        {
            WindowService.CloseAll();
          var targetWindow =  WindowService.Get<CircleRouletteWindow>();
          targetWindow.Init(_circleRouletteItem);
          targetWindow.Open();
        }
    }
}