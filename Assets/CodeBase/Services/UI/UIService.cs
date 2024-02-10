using System;
using CodeBase.Constant;
using CodeBase.Enums;
using CodeBase.Services.Factories.UI;
using CodeBase.Services.Providers.Location;
using CodeBase.Services.Window;
using CodeBase.UI;
using CodeBase.UI.Hud;
using CodeBase.UI.OfflineReward;
using UnityEngine;
using Zenject;

namespace CodeBase.Services.UI
{
    public class UIService
    {
        private readonly UIFactory _uiFactory;
        private readonly LocationProvider _locationProvider;
        private readonly WindowService _windowService;

        private bool _blockHud;
        private Canvas _joystickCanvas;

        public UIService(UIFactory uiFactory, LocationProvider locationProvider, WindowService windowService)
        {
            _windowService = windowService;
            _locationProvider = locationProvider;
            _uiFactory = uiFactory;
        }

        public void OpenOfflineRewardWindow(int totalEarnedProfit, int timeDifference)
        {
            var offlineRewardWindow = _windowService.Get<OfflineRewardWindow>();
            offlineRewardWindow.Init(totalEarnedProfit, timeDifference);
            offlineRewardWindow.Open();
            _blockHud = true;
            _windowService.Close<HudWindow>();
        }

        public void Init()
        {
            if (_blockHud == false)
                _windowService.Open<HudWindow>();
        }

        public void CreateJoystick(Camera camera)
        {
            _joystickCanvas = _uiFactory.CreateElement<Canvas>(AssetPath.JoystickCanvas, _locationProvider.UIParent);
            _joystickCanvas.worldCamera = camera;
            _joystickCanvas.planeDistance = 1;
            _joystickCanvas.sortingLayerName =
                Enum.GetName(typeof(SortingLayerTypeId), SortingLayerTypeId.JoystickUILayer);
            _joystickCanvas.gameObject.SetActive(true);
        }

        public void SetActiveJoystickUI(bool setActive)
        {
            if (_joystickCanvas != null)
                _joystickCanvas.enabled = setActive;
        }

        public void SetActiveUI(bool isEnabled)
        {
            if (_joystickCanvas != null)
                _joystickCanvas.enabled = isEnabled;

            if (!isEnabled)
                _windowService.CloseAll();
            else
                _windowService.Open<HudWindow>();
        }

        public void SetActiveUI<T>(bool isEnabled) where T : WindowBase
        {
            if (_joystickCanvas != null)
                _joystickCanvas.enabled = isEnabled;

            if (!isEnabled)
                _windowService.Close<T>();
            else
                _windowService.Open<HudWindow>();
        }
    }
}