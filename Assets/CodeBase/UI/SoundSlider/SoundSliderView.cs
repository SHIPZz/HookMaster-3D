using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Enums;
using CodeBase.Services.Sound;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
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
        [SerializeField] private float _initialValue;

        private SoundService _soundService;
        private AudioMixerGroup _audioMixerGroup;

        [Inject]
        private void Construct(SoundService soundService) => 
            _soundService = soundService;

        private void Awake()
        {
            _audioMixerGroup = _soundService.Get(Enum.GetName(typeof(MixerTypeId), _mixerTypeId));
            SetValue(_initialValue);
        }

        private void OnEnable() => 
            _slider.onValueChanged.AddListener(ChangeVolume);

        private void OnDisable() =>
            _slider.onValueChanged.RemoveListener(ChangeVolume);

        private void ChangeVolume(float volume) =>
            SetValue(volume);

        private void SetValue(float value) => 
            _audioMixerGroup.audioMixer.SetFloat(Enum.GetName(typeof(MixerTypeId), _mixerParameter), value);
    }
}