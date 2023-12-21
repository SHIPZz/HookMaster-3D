using System;
using System.Collections;
using CodeBase.Constant;
using CodeBase.Data;
using CodeBase.Extensions;
using CodeBase.Services.Employee;
using CodeBase.Services.Time;
using CodeBase.Services.WorldData;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.UI.SpeedUp
{
    public class SpeedUpWindow : WindowBase
    {
        private const float CloseAdItemMinutes = 15;
        private const float UpgradeCompletedMinutes = 60;
        private const float InitialSliderValue = -60f;

        [SerializeField] private CanvasAnimator _canvasAnimator;
        [SerializeField] private TMP_Text _remainingTimeText;
        [SerializeField] private TMP_Text _skipTicketText;
        [SerializeField] private TMP_Text _skipDiamondText;
        [SerializeField] private Slider _remainingTimeSlider;
        [SerializeField] private GameObject _skipAdItem;
        [SerializeField] private GameObject _skipTicketItem;
        [SerializeField] private GameObject _skipDiamondItem;
        [SerializeField] private GameObject _completedItem;
        [SerializeField] private float _sliderFillSpeed = 15f;

        private float _totalTime = 3600f;
        private float _initialSliderValue;
        private float _currentTime;
        private UpgradeEmployeeData _upgradeEmployeeData;
        private IWorldDataService _worldDataService;
        private Coroutine _timeCoroutine;
        private WorldTimeService _worldTimeService;
        private EmployeeDataService _employeeDataService;

        [Inject]
        private void Construct(IWorldDataService worldDataService,
            WorldTimeService worldTimeService, EmployeeDataService employeeDataService)
        {
            _employeeDataService = employeeDataService;
            _worldTimeService = worldTimeService;
            _worldDataService = worldDataService;
        }

        private void OnDestroy() =>
            SaveLastUpgradeTime();

        public override void Open() =>
            _canvasAnimator.FadeInCanvas();

        public override void Close() =>
            _canvasAnimator.FadeOutCanvas(base.Close);

        public async UniTaskVoid Init(UpgradeEmployeeData upgradeEmployeeData, float lastEmployeeUpgradeTime,
            long lastUpgradeWindowOpenedTime)
        {
            _upgradeEmployeeData = upgradeEmployeeData;

            _remainingTimeSlider.value = InitialSliderValue;

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
            _skipAdItem.SetActive(false);
            _skipDiamondItem.SetActive(false);
            _skipTicketItem.SetActive(false);
            _completedItem.SetActive(true);
            _upgradeEmployeeData.Completed = true;
            _remainingTimeText.text = "Completed";
            SaveLastUpgradeTime();
        }

        private void SaveLastUpgradeTime()
        {
            _upgradeEmployeeData.LastUpgradeTime = Mathf.Abs(_totalTime);
            print(Mathf.Abs(_totalTime));
            _upgradeEmployeeData.LastUpgradeWindowOpenedTime = _worldDataService.WorldData.WorldTimeData.CurrentTime;
            _employeeDataService.SaveUpgradeEmployeeData(_upgradeEmployeeData);
        }

        private IEnumerator StartDecreaseTimeCoroutine()
        {
            _upgradeEmployeeData.UpgradeStarted = true;

            while (Math.Abs(_remainingTimeSlider.value - _remainingTimeSlider.maxValue) > 0.1f)
            {
                _totalTime -= Time.deltaTime;

                int minutes = Mathf.FloorToInt(Mathf.Abs(_totalTime) % TimeConstantValue.SecondsInHour /
                                               TimeConstantValue.SecondsInMinute);

                _remainingTimeSlider.value = Mathf.Lerp(_remainingTimeSlider.value, -Mathf.FloorToInt(
                    Mathf.Abs(_totalTime) % TimeConstantValue.SecondsInHour /
                    TimeConstantValue.SecondsInMinute), _sliderFillSpeed * Time.deltaTime);
                int seconds = Mathf.FloorToInt(Mathf.Abs(_totalTime) % TimeConstantValue.SecondsInMinute);

                if (NeedCloseAdItem(minutes))
                    _skipAdItem.SetActive(false);

                _remainingTimeText.text = $"{minutes}m {seconds}s";
                _skipDiamondText.text = $"{minutes}m";
                _skipTicketText.text = $"{minutes}m";
                yield return null;
            }

            _remainingTimeSlider.value = _remainingTimeSlider.maxValue;
            TryToSetCompleted(_remainingTimeSlider.value, 0);
        }

        private bool NeedCloseAdItem(float minutes) =>
            minutes <= CloseAdItemMinutes;
    }
}