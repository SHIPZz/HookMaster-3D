using System;
using CodeBase.Enums;
using CodeBase.Services.Sound;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.UI.SoundSlider
{
    public class SoundSliderView : SerializedMonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private MixerTypeId _mixerTypeId;
        [SerializeField] private MixerTypeId _mixerParameter;

        private SettingsService _settingsService;
        private AudioMixerGroup _audioMixerGroup;

        [Inject]
        private void Construct(SettingsService settingsService) =>
            _settingsService = settingsService;

        private void Awake()
        {
            _audioMixerGroup = _settingsService.Get(Enum.GetName(typeof(MixerTypeId), _mixerTypeId));
            var targetVolume = _settingsService.GetTargetVolume(_mixerParameter);
            _slider.value = targetVolume;
            _audioMixerGroup.audioMixer.SetFloat(Enum.GetName(typeof(MixerTypeId), _mixerParameter), targetVolume);
        }

        private void OnEnable() =>
            _slider.onValueChanged.AddListener(ChangeVolume);

        private void OnDisable() => 
            _slider.onValueChanged.RemoveListener(ChangeVolume);

        private void OnDestroy() => 
            _settingsService.SetSoundSettings(_slider.value, _mixerParameter);

        private void ChangeVolume(float volume) =>
            SetValue(volume);

        private void SetValue(float value) => 
            _audioMixerGroup.audioMixer.SetFloat(Enum.GetName(typeof(MixerTypeId), _mixerParameter), value);
    }
}