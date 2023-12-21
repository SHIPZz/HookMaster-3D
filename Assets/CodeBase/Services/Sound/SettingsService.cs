using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Constant;
using CodeBase.Enums;
using CodeBase.Services.WorldData;
using UnityEngine;
using UnityEngine.Audio;

namespace CodeBase.Services.Sound
{
    public class SettingsService
    {
        private readonly AudioMixer _audioMixer;
        private readonly IWorldDataService _worldDataService;
        private Dictionary<MixerTypeId, Action<float>> _updateVolumeActions;
        private Dictionary<MixerTypeId, float> _targetVolumes;
        private Dictionary<ToggleTypeId, bool> _targetToggleValues;
        private Dictionary<ToggleTypeId, Action<bool>> _updateToggleValues;

        public SettingsService(IWorldDataService worldDataService)
        {
            _worldDataService = worldDataService;
            _audioMixer = Resources.Load<AudioMixer>(AssetPath.AudioMixer);

            FillDictionaries();
        }

        public void Save()
        {
            _worldDataService.Save();
        }

        public bool GetTargetToggleValue(ToggleTypeId toggleTypeId) =>
            _targetToggleValues[toggleTypeId];
        
        public float GetTargetVolume(MixerTypeId mixerTypeId) =>
            _targetVolumes[mixerTypeId];

        public void SetToggleSetting(bool isOn, ToggleTypeId toggleTypeId)
        {
            _updateToggleValues[toggleTypeId]?.Invoke(isOn);
            _targetToggleValues[toggleTypeId] = isOn;
        }

        public void SetSoundSettings(float value, MixerTypeId mixerTypeId)
        {
            _updateVolumeActions[mixerTypeId]?.Invoke(value);
            _targetVolumes[mixerTypeId] = value;
        }

        public AudioMixerGroup Get(string name)
        {
            List<AudioMixerGroup> targetAudioMixerGroups = _audioMixer.FindMatchingGroups(name).ToList();

            if (targetAudioMixerGroups.Count(x => x.name == name) != 0)
                return targetAudioMixerGroups
                    .FirstOrDefault(x => x.name == name);

            return null;
        }

        private void FillDictionaries()
        {
            _updateVolumeActions = new()
            {
                { MixerTypeId.MusicVolume, value => _worldDataService.WorldData.SettingsData.MusicVolume = value },
                { MixerTypeId.UIVolume, value => _worldDataService.WorldData.SettingsData.UIVolume = value },
            };

            _targetToggleValues = new()
            {
                { ToggleTypeId.PushAlarm, _worldDataService.WorldData.SettingsData.IsPushAlarmOn }
            };

            _updateToggleValues = new()
            {
                { ToggleTypeId.PushAlarm, value => _worldDataService.WorldData.SettingsData.IsPushAlarmOn = value }
            };

            _targetVolumes = new()
            {
                { MixerTypeId.MusicVolume, _worldDataService.WorldData.SettingsData.MusicVolume },
                { MixerTypeId.UIVolume, _worldDataService.WorldData.SettingsData.UIVolume }
            };
        }
    }
}