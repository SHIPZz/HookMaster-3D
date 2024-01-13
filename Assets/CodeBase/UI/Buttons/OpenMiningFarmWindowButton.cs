using CodeBase.Gameplay.GameItems;
using CodeBase.UI.MiningFarm;
using UnityEngine;

namespace CodeBase.UI.Buttons
{
    public class OpenMiningFarmWindowButton : ButtonOpenerBase
    {
        [SerializeField] private MiningFarmItem _miningFarmItem;

        protected override void Open()
        {
            var targetWindow = WindowService.Get<MiningFarmWindow>();
            targetWindow.Init(_miningFarmItem);
            targetWindow.Open();
        }
    }
}