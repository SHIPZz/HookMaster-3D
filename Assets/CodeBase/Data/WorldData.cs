using System;
using System.Collections.Generic;

namespace CodeBase.Data
{
    [Serializable]
    public class WorldData
    {
        public PlayerData PlayerData = new();
        public List<PotentialEmployeeData> PotentialEmployeeList = new();
        public PotentialEmployeeData LastPotentialEmployeeData;
    }
}