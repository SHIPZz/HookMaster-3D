using System;
using System.Collections.Generic;

namespace CodeBase.Data
{
    [Serializable]
    public class PlayerData
    {
        public int QualificationType  = 1;
        public int Money;
        public int Tickets;
        public int Diamonds;
        public List<EmployeeData> PurchasedEmployees = new();
        
    }
}