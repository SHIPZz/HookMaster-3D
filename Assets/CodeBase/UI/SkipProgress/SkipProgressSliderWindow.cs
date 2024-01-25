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
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.UI.SkipProgress
{
    public class SkipProgressSliderWindow : WindowBase
    {
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
        private IWorldDataService _worldDataService;
        private Coroutine _timeCoroutine;

        [Inject]
        private void Construct(IWorldDataService worldDataService, EmployeeDataService employeeDataService)
        {
            _employeeDataService = employeeDataService;
            _worldDataService = worldDataService;
        }

        public override void Open() =>
            _canvasAnimator.FadeInCanvas();

        private void OnDestroy()
        {
            SaveLastUpgradeTime();
        }

        [Button]
        public void Set(float time)
        {
            TotalTime = time;
            SaveLastUpgradeTime();
        }

        public void Init(UpgradeEmployeeData upgradeEmployeeData, Quaternion targetRotation)
        {
            _upgradeEmployeeData = upgradeEmployeeData;
            transform.rotation = targetRotation;

            CheckLastUpgradeWindowOpenedTime();

            SetEmployeeDataToButton(upgradeEmployeeData);
            
            CalculateLastUpgradeTime();

            if (TimeFullPassed())
            {
                _skipProgressButton.gameObject.SetActive(false);

                SetMaxValueWithAnim();
                
                return;
            }

            TotalTime = _upgradeEmployeeData.LastUpgradeTime;

            InitializeSlider();

            LaunchCoroutine();

            Show();
        }

        private void SetMaxValueWithAnim()
        {
            DOTween.To(() => _slider.value, value =>
                {
                    _slider.value = value;
                    TotalTime = _slider.value;
                    UpdateRemainingText();
                }, _slider.maxValue, 2.5f)
                .OnComplete(SetCompleted);
        }

        private void CheckLastUpgradeWindowOpenedTime()
        {
            WorldData worldData = _worldDataService.WorldData;
            
            var lastUpgradeWindowOpenedTime = worldData.UpgradeEmployeeDatas[_upgradeEmployeeData.EmployeeData.Id]
                .LastUpgradeWindowOpenedTime;

            TimeSpan timeDifference = worldData.WorldTimeData.CurrentTime.ToDateTime() - lastUpgradeWindowOpenedTime.ToDateTime();

            if (timeDifference.TotalDays >= TimeConstantValue.DaysInYear)
                worldData.UpgradeEmployeeDatas[_upgradeEmployeeData.EmployeeData.Id].LastUpgradeWindowOpenedTime =
                    _worldDataService.WorldData.WorldTimeData.CurrentTime;
        }

        private void UpdateRemainingText()
        {
            int minutes =
                Mathf.FloorToInt(Mathf.Abs(TotalTime) % TimeConstantValue.SecondsInHour /
                                 TimeConstantValue.SecondsInMinute);
            int seconds = Mathf.FloorToInt(Mathf.Abs(TotalTime) % TimeConstantValue.SecondsInMinute);
            _remainingText.text = $"{minutes}m {seconds}s";
        }

        private void CalculateLastUpgradeTime()
        {
            _upgradeEmployeeData.LastUpgradeTime =
                Mathf.Clamp(_upgradeEmployeeData.LastUpgradeTime - GetPassedSeconds(), 0,
                    TimeConstantValue.SecondsInHour);
        }

        private bool TimeFullPassed() =>
            _upgradeEmployeeData.LastUpgradeTime <= 0;

        private float GetPassedSeconds()
        {
            WorldData worldData = _worldDataService.WorldData;
            var currentTime = worldData.WorldTimeData.CurrentTime;
            var lastUpgradeWindowOpenedTime = worldData.UpgradeEmployeeDatas[_upgradeEmployeeData.EmployeeData.Id]
                .LastUpgradeWindowOpenedTime;

            TimeSpan timeDifference = currentTime.ToDateTime() - lastUpgradeWindowOpenedTime.ToDateTime();

            return (float)timeDifference.TotalSeconds;
        }

        public override void Hide()
        {
            _canvasAnimator.FadeOutCanvas();
            SaveLastUpgradeTime();
        }

        public override void Show()
        {
            _canvasAnimator.FadeInCanvas();
        }

        private void LaunchCoroutine()
        {
            if (_timeCoroutine != null)
                StopCoroutine(StartDecreaseTimeCoroutine());

            _timeCoroutine = StartCoroutine(StartDecreaseTimeCoroutine());
        }

        private void SaveLastUpgradeTime()
        {
            _upgradeEmployeeData.LastUpgradeTime = Mathf.Abs(TotalTime);
            WorldData worldData = _worldDataService.WorldData;
            worldData.UpgradeEmployeeDatas[_upgradeEmployeeData.EmployeeData.Id].LastUpgradeWindowOpenedTime = worldData.WorldTimeData.CurrentTime;
            _employeeDataService.SaveUpgradeEmployeeData(_upgradeEmployeeData);
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


                _slider.value = Mathf.Lerp(_slider.value, -TotalTime, _sliderFillSpeed * Time.deltaTime);

                UpdateRemainingText();

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

        private void InitializeSlider()
        {
            _slider.value = -_upgradeEmployeeData.LastUpgradeTime;
        }
    }
}