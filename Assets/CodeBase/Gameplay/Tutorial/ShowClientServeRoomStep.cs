using System;
using Abu;
using CodeBase.Constant;
using CodeBase.Services.Factories.UI;
using CodeBase.Services.Window;
using CodeBase.Services.WorldData;
using CodeBase.UI.Hud;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;

namespace CodeBase.Gameplay.Tutorial
{
    public class ShowClientServeRoomStep : TutorialStep, IDisposable
    {
        private Image _tutorialHand;
        private HudWindow _hud;

        public ShowClientServeRoomStep(UIFactory uiFactory, WindowService windowService,
            IWorldDataService worldDataService) : base(uiFactory, windowService, worldDataService) { }

        public override async void OnStart()
        {
            if (IsCompleted())
                return;

            while (!TutorialRunner.IsTutorialFinished<UpgradeEmployeeStep>())
                await UniTask.Yield();

            _hud = WindowService.Get<HudWindow>();

            _hud.TutorialContainer.TutorialFadeImage.gameObject.SetActive(true);
            _hud.ClientRoomNavigationButton.GetComponent<TutorialHighlight>().enabled = true;
            _hud.ClientRoomNavigationButton.gameObject.SetActive(true);
            _tutorialHand = UIFactory.CreateElement<Image>(AssetPath.TutorialHand,
                _hud.TutorialContainer.Get<ShowClientServeRoomStep>());
            _hud.ClientRoomNavigationButton.onClick.AddListener(OnFinished);
        }

        public override void OnFinished()
        {
            _tutorialHand?.gameObject.SetActive(false);
            _hud.ClientRoomNavigationButton.onClick.RemoveListener(OnFinished);
        }

        public void Dispose()
        {
            _hud?.ClientRoomNavigationButton.onClick.RemoveListener(OnFinished);
        }
    }
}