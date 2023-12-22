using System;
using System.Collections;
using CodeBase.Constant;
using CodeBase.Data;
using CodeBase.Extensions;
using CodeBase.Services.Employee;
using CodeBase.Services.Time;
using CodeBase.Services.WorldData;
using CodeBase.UI.Buttons;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.UI
{
    public class SkipProgressSliderWindow : WindowBase
    {
        private const float UpgradeCompletedMinutes = 60;
        private const float InitialSliderValue = -60f;
        
        [SerializeField] private Slider _slider;
        [SerializeField] private CanvasAnimator _canvasAnimator;
        [SerializeField] private float _sliderFillSpeed = 15f;
        [SerializeField] private TMP_Text _remainingText;
        [SerializeField] private SkipProgressButton _skipProgressButton;
        
        private UpgradeEmployeeData _upgradeEmployeeData;
        private EmployeeDataService _employeeDataService;
        private float _totalTime = 3600f;
        private WorldTimeService _worldTimeService;
        private IWorldDataService _worldDataService;
        private Coroutine _timeCoroutine;
        
        [Inject]
        private void Construct(IWorldDataService worldDataService,
            WorldTimeService worldTimeService, EmployeeDataService employeeDataService)
        {
            _employeeDataService = employeeDataService;
            _worldTimeService = worldTimeService;
            _worldDataService = worldDataService;
        }
        
        public override void Open() => 
            _canvasAnimator.FadeInCanvas();

        public override void Close() => 
            _canvasAnimator.FadeOutCanvas(() => base.Close());

        private void OnDestroy() => 
            SaveLastUpgradeTime();

        public async UniTaskVoid Init(UpgradeEmployeeData upgradeEmployeeData, float lastEmployeeUpgradeTime,
            long lastUpgradeWindowOpenedTime, Quaternion targetRotation)
        {
            _upgradeEmployeeData = upgradeEmployeeData;
            transform.rotation = targetRotation;
            _skipProgressButton.SetEmployeeData(upgradeEmployeeData.EmployeeData);

            _slider.value = InitialSliderValue;

            if (_upgradeEmployeeData.LastUpgradeTime != 0) 
                _totalTime = lastEmployeeUpgradeTime;

            await _worldTimeService.UpdateWorldTime();
            
            TimeSpan timePassed = _worldDataService.WorldData.WorldTimeData.CurrentTime.ToDateTime() -
                                  lastUpgradeWindowOpenedTime.ToDateTime();

            var passedSeconds = (float)timePassed.TotalSeconds;
            var passedMinutes = timePassed.TotalMinutes;

            if (_upgradeEmployeeData.UpgradeStarted && TryToSetCompleted(passedMinutes, UpgradeCompletedMinutes))
                return;

            if (_upgradeEmployeeData.UpgradeStarted)
                _totalTime -= Mathf.Abs(passedSeconds);

            if (_timeCoroutine != null)
                StopCoroutine(StartDecreaseTimeCoroutine());

            if (_totalTime >= TimeConstantValue.SecondsInHour && _upgradeEmployeeData.UpgradeStarted)
            {
                SetCompleted();
                return;
            }

            _timeCoroutine = StartCoroutine(StartDecreaseTimeCoroutine());
        }

        private bool TryToSetCompleted(double passedMinutes, float targetCompletedValue)
        {
            if (passedMinutes >= targetCompletedValue)
            {
                SetCompleted();
                return true;
            }

            return false;
        }
        
        private void SetCompleted()
        {
            _upgradeEmployeeData.Completed = true;
            _remainingText.text = "Completed";
            SaveLastUpgradeTime();
        }
        
        private void SaveLastUpgradeTime()
        {
            _upgradeEmployeeData.LastUpgradeTime = Mathf.Abs(_totalTime);
            _upgradeEmployeeData.LastUpgradeWindowOpenedTime = _worldDataService.WorldData.WorldTimeData.CurrentTime;
            _employeeDataService.SaveUpgradeEmployeeData(_upgradeEmployeeData);
        }
        
        private IEnumerator StartDecreaseTimeCoroutine()
        {
            _upgradeEmployeeData.UpgradeStarted = true;

            while (Math.Abs(_slider.value - _slider.maxValue) > 0.1f)
            {
                _totalTime -= Time.deltaTime;

                int minutes = Mathf.FloorToInt(Mathf.Abs(_totalTime) % TimeConstantValue.SecondsInHour /
                                               TimeConstantValue.SecondsInMinute);

                _slider.value = Mathf.Lerp(_slider.value, -Mathf.FloorToInt(
                    Mathf.Abs(_totalTime) % TimeConstantValue.SecondsInHour /
                    TimeConstantValue.SecondsInMinute), _sliderFillSpeed * Time.deltaTime);
                int seconds = Mathf.FloorToInt(Mathf.Abs(_totalTime) % TimeConstantValue.SecondsInMinute);

                _remainingText.text = $"{minutes}m {seconds}s";
                yield return null;
            }

            _slider.value = _slider.maxValue;
            TryToSetCompleted(_slider.value, 0);
        }
    }
}