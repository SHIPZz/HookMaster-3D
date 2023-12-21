using System.Collections.Generic;
using System.Linq;
using CodeBase.Constant;
using CodeBase.Enums;
using CodeBase.SO.Sound;
using UnityEngine;
using UnityEngine.Audio;

namespace CodeBase.Services.Sound
{
    public class SoundService
    {
        private readonly AudioMixer _audioMixer;

        public SoundService()
        {
            _audioMixer = Resources.Load<AudioMixer>(AssetPath.AudioMixer);
        }

        public AudioMixerGroup Get(string name)
        {
            List<AudioMixerGroup> targetAudioMixerGroups = _audioMixer.FindMatchingGroups(name).ToList();
            
            if (targetAudioMixerGroups.Count(x => x.name ==name) != 0)
                return targetAudioMixerGroups
                    .FirstOrDefault(x => x.name == name);

            return null;
        }
    }
}