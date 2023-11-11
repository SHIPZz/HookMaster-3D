using System;
using System.Collections.Generic;
using CodeBase.Enums;

namespace CodeBase.Data
{
    [Serializable]
    public class PlayerData
    {
        public int Money;
        public List<PlayerTypeId> PurchasedPlayers = new() { PlayerTypeId.Wolverine };
    }
}