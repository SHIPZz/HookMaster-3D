﻿using System;
using System.Collections.Generic;

namespace CodeBase.Data
{
    [Serializable]
    public class WorldData
    {
        public PlayerData PlayerData = new();
        public List<EmployeeData> PotentialEmployeeList = new();
        public WorldTimeData WorldTimeData = new();
        public TableData TableData = new();
    }
}