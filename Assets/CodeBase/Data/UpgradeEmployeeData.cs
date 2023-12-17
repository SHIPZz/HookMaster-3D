using System;

namespace CodeBase.Data
{
    [Serializable]
    public class UpgradeEmployeeData
    {
        public EmployeeData EmployeeData;
        public long RemainingUpdateTime;
        public float LastUpgradeTime = 3600f;
        public long LastUpgradeWindowOpenedTime;
    }
}