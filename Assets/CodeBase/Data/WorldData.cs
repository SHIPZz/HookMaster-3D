using System;
using System.Collections.Generic;
using CodeBase.Enums;

namespace CodeBase.Data
{
    [Serializable]
    public class WorldData
    {
        public PlayerData PlayerData = new();
        public List<EmployeeData> PotentialEmployeeList = new();
        public WorldTimeData WorldTimeData = new();
        public List<TableData> TableDatas = new();
        public List<UpgradeEmployeeData> UpgradeEmployeeDatas = new();
        public SettingsData SettingsData = new();
        public PlayerRewardData PlayerRewardData = new();
        public ShopItemData ShopItemData = new();
        public Dictionary<string, MiningFarmData> MiningFarmDatas = new();
        public Dictionary<string, CircleRouletteItemData> CircleRouletteItemDatas = new();
        public RandomItemData RandomItemData = new();
        public FireTimeData FireTimeData = new();
        public Dictionary<GameItemType, PurchaseableItemData> PurchaseableItemDatas = new();
    }
}