using System;
using System.Collections.Generic;

namespace CodeBase.Data
{
    [Serializable]
    public class PlayerData
    {
        public int QualificationType = 1;
        public List<EmployeeData> PurchasedEmployees = new();
        public Dictionary<string, bool> PurchasedManagers = new();

        public Dictionary<ItemTypeId, int> WalletResources = new()
        {
            { ItemTypeId.Money, 0 },
            { ItemTypeId.Ticket, 0 },
            { ItemTypeId.Diamond, 0 },
        };
    }
}