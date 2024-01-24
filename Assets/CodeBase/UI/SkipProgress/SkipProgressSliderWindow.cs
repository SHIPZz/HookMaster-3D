using System;
using System.Collections;
using CodeBase.Animations;
using CodeBase.Constant;
using CodeBase.Data;
using CodeBase.Extensions;
using CodeBase.Services.Employees;
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
        public float TotalTime { get; private set; } = 3600f;

        private UpgradeEmployeeData _upgradeEmployeeData;
        private EmployeeDataService _employeeDataService;
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
            TotalTime = time;
            SaveLastUpgradeTime();
        }

        public async void Init(UpgradeEmployeeData upgradeEmployeeData, float lastEmployeeUpgradeTime,
            long lastUpgradeWindowOpenedTime, Quaternion targetRotation)
        {
            _upgradeEmployeeData = upgradeEmployeeData;
            transform.rotation = targetRotation;

            if (_upgradeEmployeeData.Completed)
            {
                SetCompleted();
                return;
            }

            SetEmployeeDataToButton(upgradeEmployeeData);

            InitializeSlider();
            
            if (_upgradeEmployeeData.LastUpgradeTime != 0)
                TotalTime = lastEmployeeUpgradeTime;

            await _worldTimeService.UpdateWorldTime();
            
            var passedSeconds = GetPassedTime(lastUpgradeWindowOpenedTime, out var passedMinutes);

            if (_upgradeEmployeeData.UpgradeStarted && TryToSetCompleted(passedMinutes, UpgradeCompletedMinutes))
                return;

            if (_upgradeEmployeeData.UpgradeStarted)
                TotalTime -= Mathf.Abs(passedSeconds);

            LaunchCoroutine();
        }

        private float GetPassedTime(long lastUpgradeWindowOpenedTime, out double passedMinutes)
        {
            TimeSpan timePassed = _worldDataService.WorldData.WorldTimeData.CurrentTime.ToDateTime() - lastUpgradeWindowOpenedTime.ToDateTime();

            var passedSeconds = (float)timePassed.TotalSeconds;
            passedMinutes = timePassed.TotalMinutes;
            return passedSeconds;
        }

        private void LaunchCoroutine()
        {
            if (_timeCoroutine != null)
                StopCoroutine(StartDecreaseTimeCoroutine());

            _timeCoroutine = StartCoroutine(StartDecreaseTimeCoroutine());
        }

        private async void SaveLastUpgradeTime()
        {
            await _worldTimeService.UpdateWorldTime();
            _upgradeEmployeeData.LastUpgradeTime = Mathf.Abs(TotalTime);
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
            _employeeDataService.SaveUpgradeEmployeeData(_upgradeEmployeeData);
            SetCompletedUI();
        }

        private IEnumerator StartDecreaseTimeCoroutine()
        {
            _employeeDataService.TryAddUpgradeEmployeeData(_upgradeEmployeeData);
            _upgradeEmployeeData.SetUpgradeStarted(true);

            while (Math.Abs(_slider.value - _slider.maxValue) > 0.1f)
            {
                TotalTime -= Time.deltaTime;

                int minutes = Mathf.FloorToInt(Mathf.Abs(TotalTime) % TimeConstantValue.SecondsInHour /
                                               TimeConstantValue.SecondsInMinute);

                _slider.value = Mathf.Lerp(_slider.value, -Mathf.FloorToInt(
                    Mathf.Abs(TotalTime) % TimeConstantValue.SecondsInHour /
                    TimeConstantValue.SecondsInMinute), _sliderFillSpeed * Time.deltaTime);
                int seconds = Mathf.FloorToInt(Mathf.Abs(TotalTime) % TimeConstantValue.SecondsInMinute);

                _remainingText.text = $"{minutes}m {seconds}s";
                yield return null;
            }

            _slider.value = _slider.maxValue;
            SetCompleted();
        }

        private void SetCompletedUI()
        {
            _remainingText.text = "Completed";
            _upgradingText.text = "I become better!";
            _skipProgressButton.gameObject.SetActive(false);
            _claimUpgradeButton.gameObject.SetActive(true);
            _claimUpgradeButton.SetEmployeeData(_upgradeEmployeeData.EmployeeData);
            TotalTime = _upgradeEmployeeData.LastUpgradeTime;
            _slider.value = _slider.maxValue;
        }

        private void SetEmployeeDataToButton(UpgradeEmployeeData upgradeEmployeeData) => 
            _skipProgressButton.SetEmployeeData(upgradeEmployeeData.EmployeeData);

        private void InitializeSlider() => 
            _slider.value = InitialSliderValue;
    }
}