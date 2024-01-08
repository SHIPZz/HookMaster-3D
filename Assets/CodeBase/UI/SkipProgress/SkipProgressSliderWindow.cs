using System;
using System.Collections;
using CodeBase.Animations;
using CodeBase.Constant;
using CodeBase.Data;
using CodeBase.Extensions;
using CodeBase.Services.Employee;
using CodeBase.Services.Time;
using CodeBase.Services.WorldData;
using CodeBase.UI.Buttons;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.UI.SkipProgress
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
        [SerializeField] private TMP_Text _upgradingText;
        [SerializeField] private ClaimUpgradeButton _claimUpgradeButton;
        
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

        public override void Close()
        {
            SaveLastUpgradeTime();
            _canvasAnimator.FadeOutCanvas(() => base.Close());
        }

        [Button]
        public void Set(float time)
        {
            _totalTime = time;
        }
        
        public async UniTaskVoid Init(UpgradeEmployeeData upgradeEmployeeData, float lastEmployeeUpgradeTime,
            long lastUpgradeWindowOpenedTime, Quaternion targetRotation)
        {
            _upgradeEmployeeData = upgradeEmployeeData;
            transform.rotation = targetRotation;

            if (_upgradeEmployeeData.Completed)
            {
                SetCompleted();
                return;
            }
            
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

            _timeCoroutine = StartCoroutine(StartDecreaseTimeCoroutine());
        }


        private void SaveLastUpgradeTime()
        {
            _upgradeEmployeeData.LastUpgradeTime = Mathf.Abs(_totalTime);
            _upgradeEmployeeData.LastUpgradeWindowOpenedTime = _worldDataService.WorldData.WorldTimeData.CurrentTime;
            _employeeDataService.SaveUpgradeEmployeeData(_upgradeEmployeeData);
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
            _upgradeEmployeeData.SetCompleted(true);
            _remainingText.text = "Completed";
            _upgradingText.text = "I become better!";
            _skipProgressButton.gameObject.SetActive(false);
            _claimUpgradeButton.gameObject.SetActive(true);
            _claimUpgradeButton.SetEmployeeData(_upgradeEmployeeData.EmployeeData);
            _slider.value = _slider.maxValue;
        }

        private IEnumerator StartDecreaseTimeCoroutine()
        {
            _employeeDataService.TryAddUpgradeEmployeeData(_upgradeEmployeeData);
            _upgradeEmployeeData.SetUpgradeStarted(true);

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
            SetCompleted();
        }
    }
}