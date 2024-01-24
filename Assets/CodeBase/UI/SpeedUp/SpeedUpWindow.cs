using System;
using System.Collections;
using System.Collections.Generic;
using CodeBase.Animations;
using CodeBase.Constant;
using CodeBase.Data;
using CodeBase.Extensions;
using CodeBase.Services.Employees;
using CodeBase.Services.Window;
using CodeBase.Services.WorldData;
using CodeBase.UI.Buttons;
using CodeBase.UI.SkipProgress;
using CodeBase.UI.Upgrade;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.UI.SpeedUp
{
    public class SpeedUpWindow : WindowBase
    {
        private const float CloseAdItemMinutes = 15;
        private const float InitialSliderValue = -60f;

        [SerializeField] private CanvasAnimator _canvasAnimator;
        [SerializeField] private TMP_Text _remainingTimeText;
        [SerializeField] private TMP_Text _skipTicketText;
        [SerializeField] private TMP_Text _skipDiamondText;
        [SerializeField] private Slider _remainingTimeSlider;
        [SerializeField] private GameObject _skipAdItem;
        [SerializeField] private List<RectTransformScaleAnim> _transformScaleAnims;
        [SerializeField] private List<RectTransformScaleAnim> _completedTextsAnims;
        [SerializeField] private float _sliderFillSpeed = 15f;
        [SerializeField] private List<CheckOutButton> _checkOutButtons;
        [SerializeField] private float _destroyDelayOnCompleted = 0.5f;

        private float _initialSliderValue;
        private float _currentTime;
        private float _totalTime;
        private UpgradeEmployeeData _upgradeEmployeeData;
        private IWorldDataService _worldDataService;
        private Coroutine _timeCoroutine;
        private EmployeeDataService _employeeDataService;
        private WindowService _windowService;

        [Inject]
        private void Construct(IWorldDataService worldDataService,
            EmployeeDataService employeeDataService,
            WindowService windowService)
        {
            _windowService = windowService;
            _employeeDataService = employeeDataService;
            _worldDataService = worldDataService;
        }

        public override void Open()
        {
            _checkOutButtons.ForEach(x => x.Successful += SetAnimatedFinished);
            _canvasAnimator.FadeInCanvas();
        }

        public override void Close()
        {
            _canvasAnimator.FadeOutCanvas(() =>
            {
                TryOpenCompletedWindow();
                var skipProgressWindow = _windowService.Get<SkipProgressSliderWindow>();
                skipProgressWindow.Set(_upgradeEmployeeData.LastUpgradeTime);
                base.Close();
            });

            _checkOutButtons.ForEach(x => x.Successful -= SetAnimatedFinished);
        }

        public void Init(UpgradeEmployeeData upgradeEmployeeData, float totalTime)
        {
            _upgradeEmployeeData = upgradeEmployeeData;
            _totalTime = totalTime;

            _remainingTimeSlider.value = InitialSliderValue;

            if (_timeCoroutine != null)
                StopCoroutine(StartDecreaseTimeCoroutine());

            _timeCoroutine = StartCoroutine(StartDecreaseTimeCoroutine());
        }

        private void TryToSetCompleted(double passedMinutes, float targetCompletedValue)
        {
            if (!(passedMinutes >= targetCompletedValue)) 
                return;
            
            SetCompleted();
        }

        private async void SetCompleted()
        {
            _upgradeEmployeeData.SetCompleted(true);
            SetCompletedUI();

            await UniTask.WaitForSeconds(_destroyDelayOnCompleted);

            Close();
        }

        private IEnumerator StartDecreaseTimeCoroutine()
        {
            _upgradeEmployeeData.SetUpgradeStarted(true);

            while (Math.Abs(_remainingTimeSlider.value - _remainingTimeSlider.maxValue) > 0.1f)
            {
                _totalTime -= Time.deltaTime;

                int minutes = Mathf.FloorToInt(Mathf.Abs(_totalTime) % TimeConstantValue.SecondsInHour /
                                               TimeConstantValue.SecondsInMinute);

                var seconds = CalculateSliderValue();

                if (NeedCloseAdItem(minutes))
                    _skipAdItem.SetActive(false);

                SetValuesToTexts(minutes, seconds);
                yield return null;
            }

            _remainingTimeSlider.value = _remainingTimeSlider.maxValue;
            TryToSetCompleted(_remainingTimeSlider.value, 0);
        }

        private int CalculateSliderValue()
        {
            _remainingTimeSlider.value = Mathf.Lerp(_remainingTimeSlider.value, -Mathf.FloorToInt(
                Mathf.Abs(_totalTime) % TimeConstantValue.SecondsInHour /
                TimeConstantValue.SecondsInMinute), _sliderFillSpeed * Time.deltaTime);
            int seconds = Mathf.FloorToInt(Mathf.Abs(_totalTime) % TimeConstantValue.SecondsInMinute);
            return seconds;
        }

        private void TryOpenCompletedWindow()
        {
            if (!_upgradeEmployeeData.Completed)
            {
                SaveLastUpgradeTime();
                return;
            }

            _employeeDataService.UpgradeEmployeeData(_upgradeEmployeeData.EmployeeData);

            var upgradeCompletedWindow = _windowService.Get<UpgradeEmployeeCompletedWindow>();
            upgradeCompletedWindow.Init(_upgradeEmployeeData.EmployeeData);
            upgradeCompletedWindow.Open();
        }

        private void SaveLastUpgradeTime()
        {
            _upgradeEmployeeData.SetLastUpgradeTime(Mathf.Abs(_totalTime))
                .SetLastUpgradeWindowOpenedTime(_worldDataService.WorldData.WorldTimeData.CurrentTime);
            _employeeDataService.SaveUpgradeEmployeeData(_upgradeEmployeeData);
        }

        private void SetCompletedUI()
        {
            CloseButton.gameObject.SetActive(false);
            _remainingTimeText.gameObject.SetActive(false);
            _transformScaleAnims.ForEach(x => x.UnScale());
            _completedTextsAnims.ForEach(x =>
            {
                x.gameObject.SetActive(true);
                x.ToScale();
            });
        }

        private void SetValuesToTexts(int minutes, int seconds)
        {
            _remainingTimeText.text = $"{minutes}m {seconds}s";
            _skipDiamondText.text = $"{minutes}m";
            _skipTicketText.text = $"{minutes}m";
        }

        private void SetAnimatedFinished()
        {
            _transformScaleAnims.ForEach(x => x.UnScale());
            StopCoroutine(StartDecreaseTimeCoroutine());
            _remainingTimeSlider.DOValue(_remainingTimeSlider.maxValue, 0.5f).OnComplete(SetCompleted);
        }

        private bool NeedCloseAdItem(float minutes) =>
            minutes <= CloseAdItemMinutes;
    }
}