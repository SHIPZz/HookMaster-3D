using System;

namespace CodeBase.Data
{
    [Serializable]
    public class WorldTimeData
    {
        public long CurrentTime;
        public long LastVisitedTime;
        public long LastEarnedProfitTime;
        public long LastSalaryPaymentTime;

    }
}