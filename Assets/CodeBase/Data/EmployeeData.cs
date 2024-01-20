using System;
using CodeBase.Enums;

namespace CodeBase.Data
{
    [Serializable]
    public class EmployeeData
    {
        public Guid Guid;
        public string Id;
        public string Name;
        public int QualificationType;
        public int Salary;
        public int Profit;
        public bool IsWorking;
        public string TableId;
        public bool IsUpgrading;
        public bool IsBurned;
        public EmployeeTypeId EmployeeTypeId = EmployeeTypeId.Default;
    }
}
