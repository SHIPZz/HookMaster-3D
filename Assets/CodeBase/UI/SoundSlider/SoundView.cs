using System;
using CodeBase.Enums;
using CodeBase.Services.DataService;
using CodeBase.Services.Sound;
using CodeBase.SO.Settings;
using CodeBase.UI.Toggle;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Audio;
using Zenject;

namespace CodeBase.UI.SoundSlider
{
    public class SoundView : SerializedMonoBehaviour
    {
        private const float DisableSoundValue = -80f;
        [SerializeField] private UnityEngine.UI.Toggle _toggle;
        [SerializeField] private MixerTypeId _mixerTypeId;
        [SerializeField] private MixerTypeId _mixerParameter;
        [SerializeField] private ToggleAnimation _toggleAnimation;

        private SettingsService _settingsService;
        private AudioMixerGroup _audioMixerGroup;
        private UIStaticDataService _uiStaticDataService;

        [Inject]
        private void Construct(SettingsService settingsService, UIStaticDataService uiStaticDataService)
        {
            _uiStaticDataService = uiStaticDataService;
            _settingsService = settingsService;
        }

        private void Start()
        {
            _audioMixerGroup = _settingsService.Get(Enum.GetName(typeof(MixerTypeId), _mixerTypeId));
            var isOn = _settingsService.GetTargetSoundValue(_mixerParameter);
            _toggle.isOn = isOn;
            _toggleAnimation.Initialize(isOn);
        }

        private void OnEnable() =>
            _toggle.onValueChanged.AddListener(ChangeVolume);

        private void OnDisable() =>
            _toggle.onValueChanged.RemoveListener(ChangeVolume);

        private void OnDestroy()
        {
            _settingsService.SetSoundSettings(_toggle.isOn, _mixerParameter);
        }

        private void ChangeVolume(bool isOn)
        {
            SettingSO settingData = _uiStaticDataService.GetSettingSo();
            _toggleAnimation.MoveHandleWithAnim(isOn);

            if (!isOn)
            {
                SetValue(DisableSoundValue);
                return;
            }

            switch (_mixerParameter)
            {
                case MixerTypeId.MusicVolume:
                    SetValue(settingData.MusicVolume);
                    break;

                case MixerTypeId.UIVolume:
                    SetValue(settingData.MainVolume);
                    break;
            }
        }

        private void SetValue(float value) =>
            _audioMixerGroup.audioMixer.SetFloat(Enum.GetName(typeof(MixerTypeId), _mixerParameter), value);
    }
}