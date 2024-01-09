using System;
using System.Collections;
using CodeBase.Constant;
using CodeBase.Services.MiningFarm;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CodeBase.Gameplay.ShopItemSystem
{
    public class MiningFarm : ShopItemGameModel
    {
        public int ProfitPerMinute { get; private set; }
        private int _minTemperature = 65;
        private int _midTemperature = 85;

        private readonly WaitForSecondsRealtime _minute = new WaitForSecondsRealtime(60f);
        private int _workingMinutes;
        private MiningFarmService _miningFarmService;
        private int _maxTemperature;

        public int TargetTemperature { get; private set; }
        public bool IsWorking { get; private set; }
        public bool NeedClean { get; private set; }

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
            StartCoroutine(StartIncreaseWorkingMinutes());
        }

        public void SetTemperatures(int min, int mid, int max)
        {
            _maxTemperature = max;
            _midTemperature = mid;
            _minTemperature = min;
            TargetTemperature = Random.Range(_minTemperature, _midTemperature);
        }

        public void SetNeedClean(bool needClean)
        {
            NeedClean = needClean;
            
            if (needClean)
                TargetTemperature = _maxTemperature;
        }

        public void SetProfitPerMinute(int profitPerMinute)
        {
            ProfitPerMinute = profitPerMinute;
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
            SetNeedClean(true);
            _miningFarmService.ResetLastMiningTime();
        }
    }
}