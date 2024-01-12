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
        public  List<TableData> TableDatas = new();
        public List<UpgradeEmployeeData> UpgradeEmployeeDatas = new();
        public SettingsData SettingsData = new();
        public PlayerRewardData PlayerRewardData = new();
        public ShopItemData ShopItemData = new();
        public MiningFarmData MiningFarmData = new();
        public RandomItemData RandomItemData = new();
        public FireTimeData FireTimeData = new();
        public BurnableItemData BurnableItemData = new();
        public List<BurnableItemData> BurnableItemDatas = new();
    }
}