using System;

namespace CodeBase.Data
{
    [Serializable]
    public class UpgradeEmployeeData
    {
        public EmployeeData EmployeeData;
        public long RemainingUpdateTime;
        public float LastSavedTime;
        public long LastUpgradeWindowOpenedTime;
    }
}