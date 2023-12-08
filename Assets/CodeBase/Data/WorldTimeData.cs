using System;

namespace CodeBase.Data
{
    [Serializable]
    public class WorldTimeData
    {
        public Date CurrentTime = new();
        public Date LastVisitedTime = new();
        public Date LastEarnedProfitTime = new();
    }
}