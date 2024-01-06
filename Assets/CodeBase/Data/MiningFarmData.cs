using System;

namespace CodeBase.Data
{
    [Serializable]
    public class MiningFarmData
    {
        public string Id;
        public long LastWorkingTime;
        public bool NeedClean;
        public int WorkingMinutes;
    }
}