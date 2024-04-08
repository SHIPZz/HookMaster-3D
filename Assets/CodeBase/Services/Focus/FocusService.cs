using System;
using CodeBase.Services.Pause;
using CodeBase.Services.Window;
using CodeBase.UI;
using CodeBase.UI.Hud;
using UnityEngine;
using Zenject;

namespace CodeBase.Services.Focus
{
    public class FocusService : IInitializable, IDisposable
    {
        private readonly IPauseService _pauseService;
        private readonly WindowService _windowService;

        public event Action FocusChanged;

        public FocusService(IPauseService pauseService, WindowService windowService)
        {
            _pauseService = pauseService;
            _windowService = windowService;
        }

        public void Initialize()
        {
            _windowService.Opened += SetPause;
            Application.focusChanged += OnFocusChanged;
        }

        public void Dispose()
        {
            _windowService.Opened -= SetPause;
        }

        private void OnFocusChanged(bool hasFocus)
        {
            if (!hasFocus)
            {
                // FocusChanged?.Invoke();
            }
        }

        private void SetPause(WindowBase windowBase)
        {
            if(windowBase.GetType() == typeof(HudWindow))
                _pauseService.Run();
            else
                _pauseService.Stop();
        }
    }
}