using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Constant;
using CodeBase.Enums;
using CodeBase.Services.Providers.Asset;
using CodeBase.Services.WorldData;
using UnityEngine;
using UnityEngine.Audio;

namespace CodeBase.Services.Sound
{
    public class SettingsService
    {
        private readonly IWorldDataService _worldDataService;
        private readonly IAssetProvider _assetProvider;
        private AudioMixer _audioMixer;

        public SettingsService(IWorldDataService worldDataService, IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
            _worldDataService = worldDataService;
        }

        public void Init()
        {
            _audioMixer = _assetProvider.GetObject<AudioMixer>(AssetPath.AudioMixer);
        }

        public void Save() =>
            _worldDataService.Save();

        public bool GetTargetToggleValue(ToggleTypeId toggleTypeId)
        {
            return _worldDataService.WorldData.SettingsData.Toggles[toggleTypeId];
        }
        
        public bool GetTargetSoundValue(MixerTypeId mixerTypeId)
        {
            return _worldDataService.WorldData.SettingsData.Sounds[mixerTypeId];
        }
        
        public void SetToggleSetting(bool isOn, ToggleTypeId toggleTypeId)
        {
            _worldDataService.WorldData.SettingsData.Toggles[toggleTypeId] = isOn;
        }

        public void SetSoundSettings(bool value, MixerTypeId mixerTypeId)
        {
            _worldDataService.WorldData.SettingsData.Sounds[mixerTypeId] = value;
        }

        public AudioMixerGroup Get(string name)
        {
            List<AudioMixerGroup> targetAudioMixerGroups = _audioMixer.FindMatchingGroups(name).ToList();

            if (targetAudioMixerGroups.Count(x => x.name == name) != 0)
                return targetAudioMixerGroups
                    .FirstOrDefault(x => x.name == name);

            return null;
        }
    }
}