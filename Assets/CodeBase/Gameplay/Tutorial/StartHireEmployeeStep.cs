using System;
using Abu;
using CodeBase.Constant;
using CodeBase.Gameplay.PlayerSystem;
using CodeBase.Services.CameraServices;
using CodeBase.Services.Factories.UI;
using CodeBase.Services.Providers.Location;
using CodeBase.Services.Window;
using CodeBase.Services.WorldData;
using CodeBase.UI;
using CodeBase.UI.Buttons;
using CodeBase.UI.Hud;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;

namespace CodeBase.Gameplay.Tutorial
{
    public class StartHireEmployeeStep : TutorialStep, IDisposable
    {
        private readonly PlayerInputService _playerInputService;
        private readonly CameraFocus _cameraFocus;
        private readonly LocationProvider _locationProvider;

        private Image _tutorialHand;
        private OpenEmployeeWindowButton _targetButton;
        private TutorialFadeImage _tutorialFadeImage;
        private bool _onFinished;
        private bool _hudOpened;

        public StartHireEmployeeStep(UIFactory uiFactory, WindowService windowService,
            IWorldDataService worldDataService, PlayerInputService playerInputService,
            CameraFocus cameraFocus,
            LocationProvider locationProvider)
            : base(uiFactory, windowService, worldDataService)
        {
            _locationProvider = locationProvider;
            _cameraFocus = cameraFocus;
            _playerInputService = playerInputService;
        }

        public override void OnStart()
        {
            if (IsCompleted())
                return;

            WindowService.HudOpened += OnWindowOpened;
        }

        public override void OnFinished()
        {
            _tutorialFadeImage.gameObject.SetActive(false);
            _tutorialHand.gameObject.SetActive(false);
            _targetButton.onClick.RemoveListener(OnFinished);
            _targetButton.GetComponent<TutorialHighlight>().enabled = false;
            _playerInputService.SetInputActive(true);
            WindowService.HudOpened -= OnWindowOpened;
            SetCompleteToData(true);
        }

        public void Dispose()
        {
            WindowService.HudOpened -= OnWindowOpened;
        }

        private void OnWindowOpened(WindowBase obj)
        {
            if (IsCompleted())
                return;

            var hudWindow = WindowService.Get<HudWindow>();
            _tutorialFadeImage = hudWindow.TutorialContainer.TutorialFadeImage;
            _tutorialFadeImage.gameObject.SetActive(true);
            _tutorialHand = UIFactory.CreateElement<Image>(AssetPath.TutorialHand,
                hudWindow.TutorialContainer.Get<StartHireEmployeeStep>());
            _targetButton = hudWindow.OpenEmployeeWindowButton;
            _targetButton.GetComponent<TutorialHighlight>().enabled = true;
            _targetButton.onClick.AddListener(OnFinished);
            _playerInputService.SetInputActive(false);
            _hudOpened = true;
        }
    }
}