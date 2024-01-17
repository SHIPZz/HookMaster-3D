using System;
using System.Collections;
using CodeBase.Constant;
using CodeBase.Services.Mining;
using CodeBase.Services.UI;
using CodeBase.UI.FloatingText;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace CodeBase.Gameplay.GameItems
{
    public class MiningFarmItem : GameItemAbstract
    {
        public string Id;
        public int WorkingMinutes;
        public int ProfitPerMinute;
        public int TargetTemperature;
        public bool NeedClean;

        private int _minTemperature;
        private int _midTemperature;

        private readonly WaitForSecondsRealtime _minute = new WaitForSecondsRealtime(60f);
        private MiningFarmService _miningFarmService;
        private int _maxTemperature;
        private FloatingTextService _floatingTextService;

        public bool IsWorking { get; private set; }

        public event Action Stopped;

        [Inject]
        private void Construct(FloatingTextService floatingTextService)
        {
            _floatingTextService = floatingTextService;
        }

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

        private IEnumerator StartIncreaseWorkingMinutes()
        {
            while (WorkingMinutes != TimeConstantValue.MinutesInTwoHour)
            {
                yield return _minute;
                WorkingMinutes++;
                _miningFarmService.SetProfit(ProfitPerMinute);
                _floatingTextService.ShowFloatingText(FloatingTextType.MoneyProfit, transform, transform.position,
                    $"{ProfitPerMinute}$");
                _miningFarmService.SetWorkingMinutes(Id, WorkingMinutes);
            }

            Stopped?.Invoke();
            IsWorking = false;
            _miningFarmService.SetWorkingMinutes(Id, TimeConstantValue.MinutesInTwoHour);
            _miningFarmService.SetNeedClean(Id, true);
            SetNeedClean(true);
            _miningFarmService.ResetLastMiningTime();
        }

        [Button]
        private void Test()
        {
            _floatingTextService.ShowFloatingText(FloatingTextType.MoneyProfit, transform, transform.position,
                $"{ProfitPerMinute}$");
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