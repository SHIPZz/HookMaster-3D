using System;
using System.Collections.Generic;
using CodeBase.Enums;

namespace CodeBase.Data
{
    [Serializable]
    public class SettingsData
    {
        public Dictionary<ToggleTypeId, bool> Toggles = new()
        {
            { ToggleTypeId.PushAlarm, true }
        };

        public Dictionary<MixerTypeId, bool> Sounds = new()
        {
            { MixerTypeId.MusicVolume, true },
            { MixerTypeId.UIVolume, true }
        };
    }
}