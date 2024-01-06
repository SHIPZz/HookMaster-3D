using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CodeBase.UI.Shop
{
    public class MiningFarm : ShopItem
    {
        [field: SerializeField] public int ProfitPerMinute { get; private set; }
        [SerializeField] private int _minTemperature = 65;
        [SerializeField] private int _maxTemperature = 100;
        [SerializeField] private int _midTemperature = 75;
        
        public int TargetTemperature { get; private set; }

        private void Start()
        {
            TargetTemperature= Random.Range(_minTemperature, _midTemperature);
        }
    }
}