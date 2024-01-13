using System;
using System.Collections;
using CodeBase.Constant;
using CodeBase.Services.Mining;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CodeBase.Gameplay.GameItems
{
    public class MiningFarmItem : GameItemAbstract
    {
        public string Id;
        public int WorkingMinutes;
        
        public int ProfitPerMinute { get; private set; }
        private int _minTemperature;
        private int _midTemperature;

        private readonly WaitForSecondsRealtime _minute = new WaitForSecondsRealtime(60f);
        private MiningFarmService _miningFarmService;
        private int _maxTemperature;

        public int TargetTemperature { get; private set; }
        public bool IsWorking { get; private set; }
        public bool NeedClean { get; private set; }

        public event Action Stopped;

        public void Init(int minutes, MiningFarmService miningFarmService)
        {
            _miningFarmService = miningFarmService;
            WorkingMinutes = minutes;
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
            while (WorkingMinutes != TimeConstantValue.MinutesInTwoHour)
            {
                yield return _minute;
                WorkingMinutes++;
                _miningFarmService.SetProfit(ProfitPerMinute);
                _miningFarmService.SetWorkingMinutes(WorkingMinutes);
            }

            Stopped?.Invoke();
            IsWorking = false;
            _miningFarmService.SetWorkingMinutes(TimeConstantValue.MinutesInTwoHour);
            _miningFarmService.SetNeedClean(true);
            SetNeedClean(true);
            _miningFarmService.ResetLastMiningTime();
        }

        [Button]
        private void CreateId()
        {
            Id = Guid.NewGuid().ToString();
        }

        [Button]
        private void Test(int minutes)
        {
            WorkingMinutes = minutes;
        }
    }
}