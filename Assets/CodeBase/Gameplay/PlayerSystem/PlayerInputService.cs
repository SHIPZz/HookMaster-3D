using System;
using System.Threading;
using CodeBase.Services.CameraServices;
using CodeBase.Services.UI;
using CodeBase.Services.Window;
using CodeBase.UI;
using CodeBase.UI.Hud;
using Cysharp.Threading.Tasks;
using Zenject;

namespace CodeBase.Gameplay.PlayerSystem
{
    public class PlayerInputService : IInitializable, IDisposable
    {
        public PlayerInput PlayerInput { get; set; }

        private readonly UIService _uiService;
        private readonly WindowService _windowService;
        private readonly CameraFocus _cameraFocus;

        public PlayerInputService(UIService uiService, WindowService windowService, CameraFocus cameraFocus)
        {
            _cameraFocus = cameraFocus;
            _windowService = windowService;
            _uiService = uiService;
        }

        public void Initialize()
        {
            _windowService.Opened += WindowOpenedHandler;
            _cameraFocus.Moved += CameraFocusMovedHandler;
        }

        public void Dispose()
        {
            _windowService.Opened -= WindowOpenedHandler;
            _cameraFocus.Moved -= CameraFocusMovedHandler;
        }

        public void SetInputActive(bool isActive)
        {
            if (!isActive)
            {
                PlayerInput.SetBlocked(true);
                _uiService.SetActiveJoystickUI(false);
                return;
            }

            PlayerInput.SetBlocked(false);
            _uiService.SetActiveJoystickUI(true);
        }

        private async void CameraFocusMovedHandler()
        {
            SetInputActive(false);
            await UniTask.WaitUntil(() => _cameraFocus.HasFollow == false);
            SetInputActive(true);
        }

        private void WindowOpenedHandler(WindowBase window) => 
            SetInputActive(window.GetType() == typeof(HudWindow));
    }
}