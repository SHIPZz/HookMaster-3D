using System;
using CodeBase.Gameplay.TableSystem;

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
    }
}
