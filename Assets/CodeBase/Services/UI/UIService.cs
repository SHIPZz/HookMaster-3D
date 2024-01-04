using System;
using CodeBase.Constant;
using CodeBase.Enums;
using CodeBase.Services.Factories.UI;
using CodeBase.Services.Providers.Location;
using CodeBase.Services.Window;
using CodeBase.UI.Hud;
using CodeBase.UI.OfflineReward;
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

        private bool _blockHud;

        public UIService(UIFactory uiFactory, UIProvider uiProvider, LocationProvider locationProvider,
            WindowService windowService)
        {
            _windowService = windowService;
            _locationProvider = locationProvider;
            _uiFactory = uiFactory;
            _uiProvider = uiProvider;
        }

        public void OpenOfflineRewardWindow(float totalEarnedProfit, int timeDifference)
        {
            var offlineRewardWindow = _windowService.Get<OfflineRewardWindow>();
            offlineRewardWindow.Init(totalEarnedProfit,timeDifference);
            offlineRewardWindow.Open();
            _blockHud = true;
            _windowService.Close<HudWindow>();
        }

        public void Init(Camera camera)
        {
            var joystickCanvas = _uiFactory.CreateElement<Canvas>(AssetPath.JoystickCanvas, _locationProvider.UIParent);
            joystickCanvas.worldCamera = camera;
            joystickCanvas.planeDistance = 1;
            joystickCanvas.sortingLayerName = Enum.GetName(typeof(SortingLayerTypeId), SortingLayerTypeId.JoystickUILayer);
            joystickCanvas.gameObject.SetActive(true);

            if (_blockHud == false)
                _windowService.Open<HudWindow>();
        }
    }
}