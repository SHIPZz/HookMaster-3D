using System;
using System.Collections;
using CodeBase.Constant;
using CodeBase.Services.MiningFarm;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CodeBase.Gameplay.ShopItemSystem
{
    public class MiningFarm : ShopItem
    {
        [field: SerializeField] public int ProfitPerMinute { get; private set; }
        [SerializeField] private int _minTemperature = 65;
        [SerializeField] private int _midTemperature = 85;

        private readonly WaitForSecondsRealtime _minute = new WaitForSecondsRealtime(60f);
        private int _workingMinutes;
        private MiningFarmService _miningFarmService;

        public int TargetTemperature { get; private set; }
        public bool IsWorking { get; private set; }

        public event Action Stopped;

        [Button]
        public void Test(int minutes)
        {
            _workingMinutes = minutes;
        }

        public void Init(int minutes, MiningFarmService miningFarmService)
        {
            _miningFarmService = miningFarmService;
            _workingMinutes = minutes;
            IsWorking = true;
            TargetTemperature = Random.Range(_minTemperature, _midTemperature);
            StartCoroutine(StartIncreaseWorkingMinutes());
        }

        private IEnumerator StartIncreaseWorkingMinutes()
        {
            while (_workingMinutes != TimeConstantValue.MinutesInTwoHour)
            {
                yield return _minute;
                _workingMinutes++;
                _miningFarmService.SetProfit(ProfitPerMinute);
                _miningFarmService.SetWorkingMinutes(_workingMinutes);
            }

            Stopped?.Invoke();
            IsWorking = false;
            _miningFarmService.SetWorkingMinutes(TimeConstantValue.MinutesInTwoHour);
            _miningFarmService.SetNeedClean(true);
            _miningFarmService.ResetLastMiningTime();
        }
    }
}