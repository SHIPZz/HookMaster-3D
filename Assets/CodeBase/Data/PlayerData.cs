using System;
using System.Collections.Generic;

namespace CodeBase.Data
{
    [Serializable]
    public class PlayerData
    {
        public int QualificationType = 1;
        public float Money;
        public List<EmployeeData> PurchasedEmployees = new();
    }
}