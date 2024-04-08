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
        public bool IsWorking;
        public string TableId;
        public float PaperProcessTime;
        public bool IsUpgrading;
        public bool IsBurned;
        public EmployeeTypeId EmployeeTypeId = EmployeeTypeId.Default;
    }
}
