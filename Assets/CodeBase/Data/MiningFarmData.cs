using System;

namespace CodeBase.Data
{
    [Serializable]
    public class MiningFarmData
    {
        public string Id;
        public long LastCleanTime;
        public bool NeedClean;
        public int WorkingMinutes;
        public int TargetTemperature;
        public int ProfitPerMinute;
        public VectorData Position;
    }
}