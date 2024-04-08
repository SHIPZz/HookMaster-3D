using System;
using CodeBase.Services.Focus;
using CodeBase.Services.Window;
using CodeBase.UI.Hud;
using Zenject;

namespace CodeBase.UI.Pause
{
    public class PauseWindowController : IInitializable, IDisposable
    {
        private readonly FocusService _focusService;
        private readonly WindowService _windowService;

        public PauseWindowController(FocusService focusService, WindowService windowService)
        {
            _focusService = focusService;
            _windowService = windowService;
        }

        public void Initialize()
        {
            _focusService.FocusChanged += OpenPauseWindow;
        }

        public void Dispose()
        {
            _focusService.FocusChanged -= OpenPauseWindow;
        }

        private void OpenPauseWindow()
        {
            if(!_windowService.IsWindowOfType<HudWindow>() || _windowService.IsWindowOfType<PauseWindow>())
                return;
            
            _windowService.Close<HudWindow>();
            _windowService.Open<PauseWindow>();
        }
    }
}