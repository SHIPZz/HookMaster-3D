using System;
using CodeBase.Constant;
using CodeBase.Services.Factories.UI;
using CodeBase.Services.Window;
using CodeBase.Services.WorldData;
using CodeBase.UI;
using CodeBase.UI.Upgrade;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;

namespace CodeBase.Gameplay.Tutorial
{
    public class UpgradeEmployeeStep : TutorialStep, IDisposable
    {
        private UpgradeEmployeeWindow _upgradeEmployeeWindow;
        private Image _tutorialHand;
        public UpgradeEmployeeStep(UIFactory uiFactory, WindowService windowService, IWorldDataService worldDataService) : base(uiFactory, windowService, worldDataService) { }
        
        public override void OnStart()
        {
            if (IsCompleted())
                return;

            WindowService.Opened += OnWindowOpened;
        }

        public void Dispose()
        {
            WindowService.Opened -= OnWindowOpened;
        }

        public override void OnFinished()
        {
            _upgradeEmployeeWindow.TutorialFadeImage.enabled = false;
            _tutorialHand.gameObject.SetActive(false);
            WindowService.Opened -= OnWindowOpened;
            SetCompleteToData(true);
            IsFinished = true;
        }

        private async void OnWindowOpened(WindowBase window)
        {
            if(window.GetType() != typeof(UpgradeEmployeeWindow))
                return;

            _upgradeEmployeeWindow = WindowService.Get<UpgradeEmployeeWindow>();

            _upgradeEmployeeWindow.TutorialFadeImage.enabled = true;
            _tutorialHand = UIFactory.CreateElement<Image>(AssetPath.TutorialHand, _upgradeEmployeeWindow.TutorialHandParent);
            _upgradeEmployeeWindow.UpgradeEmployeeButton.SetTutorial();

            while (!_upgradeEmployeeWindow.UpgradeEmployeeButton.Clicked) 
                await UniTask.Yield();

            OnFinished();
        }
    }
}