using System;

namespace CodeBase.Data
{
    [Serializable]
    public class EmployeeData
    {
        public Guid Guid;
        public string Id;
        public string Name;
        public int QualificationType;
        public float Salary;
        public float Profit;
        public bool IsWorking;
        public string TableId;
        public bool IsUpgrading;
    }
}
