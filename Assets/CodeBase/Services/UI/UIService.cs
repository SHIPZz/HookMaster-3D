using CodeBase.Constant;
using CodeBase.Services.Factories.UI;
using CodeBase.Services.Providers.Location;
using CodeBase.Services.Window;
using CodeBase.UI.Hud;
using UnityEngine;
using Zenject;

namespace CodeBase.Services.UI
{
    public class UIService
    {
        private readonly UIFactory _uiFactory;
        private readonly UIProvider _uiProvider;
        private readonly LocationProvider _locationProvider;
        private readonly WindowService _windowService;

        public UIService(UIFactory uiFactory, UIProvider uiProvider, LocationProvider locationProvider,
            WindowService windowService)
        {
            _windowService = windowService;
            _locationProvider = locationProvider;
            _uiFactory = uiFactory;
            _uiProvider = uiProvider;
        }

        public void Init(Camera camera)
        {
            var joystickCanvas = _uiFactory.CreateElement<Canvas>(AssetPath.JoystickCanvas, _locationProvider.UIParent);
            joystickCanvas.worldCamera = camera;
            joystickCanvas.planeDistance = 1;
            joystickCanvas.sortingLayerName = "JoystickUILayer";
            joystickCanvas.gameObject.SetActive(true);
            _windowService.Open<HudWindow>();
        }
    }
}