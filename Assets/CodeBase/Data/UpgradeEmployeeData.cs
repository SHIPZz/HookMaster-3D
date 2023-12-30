using System;

namespace CodeBase.Data
{
    [Serializable]
    public class UpgradeEmployeeData
    {
        public EmployeeData EmployeeData;
        public float LastUpgradeTime = 3600f;
        public long LastUpgradeWindowOpenedTime;
        public bool Completed;
        public bool UpgradeStarted;
        public float UpgradeCost = 1000;
    }
}