using System;

namespace CodeBase.Data
{
    [Serializable]
    public class SettingsData
    {
        public float MusicVolume = -25f;
        public float UIVolume = -10f;
        public bool IsPushAlarmOn = true;
    }
}