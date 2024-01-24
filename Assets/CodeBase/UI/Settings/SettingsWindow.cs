﻿using CodeBase.Animations;
using CodeBase.Gameplay.SoundPlayer;
using CodeBase.Services.Sound;
using CodeBase.Services.Window;
using CodeBase.UI.Hud;
using UnityEngine;
using Zenject;

namespace CodeBase.UI.Settings
{
    public class SettingsWindow : WindowBase
    {
        [SerializeField] private CanvasAnimator _canvasAnimator;
        [SerializeField] private SoundPlayerSystem _soundPlayerSystem;
        [SerializeField] private RectTransformScaleAnim _windowScaleAnim;

        private WindowService _windowService;
        private SettingsService _settingsService;

        [Inject]
        private void Construct(WindowService windowService, SettingsService settingsService)
        {
            _settingsService = settingsService;
            _windowService = windowService;
        }

        public override void Open()
        {
            // _soundPlayerSystem.PlayActiveSound();
            _windowScaleAnim.ToScale();
            _canvasAnimator.FadeInCanvas();
        }

        public override void Close()
        {
            _settingsService.Save();

            _canvasAnimator.FadeOutCanvas(() =>
            {
                _windowService.Open<HudWindow>();
                base.Close();
            });
        }
    }
}