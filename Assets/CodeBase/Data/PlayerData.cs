using System;
using System.Collections.Generic;
using CodeBase.Enums;
using CodeBase.Gameplay.EmployeeSystem;

namespace CodeBase.Data
{
    [Serializable]
    public class PlayerData
    {
        public int QualificationType = 1;
        public int Money;
        public List<EmployeeData> PurchasedEmployees = new();
    }
}