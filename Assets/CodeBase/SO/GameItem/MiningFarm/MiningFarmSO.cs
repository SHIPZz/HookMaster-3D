﻿using UnityEngine;

namespace CodeBase.SO.GameItem.MiningFarm
{
    [CreateAssetMenu(fileName = "MiningFarmSO", menuName = "Gameplay/GameItem/MiningFarmSO")]
    public class MiningFarmSO :  GameItemAbstractSO
    {
        [Range(0, 100)] public int ProfitPerMinute;
        [Range(65, 75)] public int MinTemperature;
        [Range(65, 85)] public int MidTemperature;
        [Range(100,120)] public int MaxTemperature;
    }
}