using System;
using System.Collections;
using System.Collections.Generic;
using CodeBase.Animations;
using CodeBase.Constant;
using CodeBase.Data;
using CodeBase.Extensions;
using CodeBase.Services.Ad;
using CodeBase.Services.Employees;
using CodeBase.Services.Window;
using CodeBase.Services.WorldData;
using CodeBase.UI.Hud;
using CodeBase.UI.SkipProgress;
using CodeBase.UI.Upgrade;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.UI.SpeedUp
{
    public class SpeedUpWindow : WindowBase
    {
        private const float InitialSliderValue = -60f;

        [SerializeField] private CanvasAnimator _canvasAnimator;
        [SerializeField] private TMP_Text _remainingTimeText;
        [SerializeField] private Slider _remainingTimeSlider;
        [SerializeField] private List<TransformScaleAnim> _transformScaleAnims;
        [SerializeField] private List<TransformScaleAnim> _completedTextsAnims;
        [SerializeField] private float _sliderFillSpeed = 15f;
        [SerializeField] private Button _adButton;
        [SerializeField] private float _destroyDelayOnCompleted = 0.5f;

        private float _initialSliderValue;
        private float _currentTime;
        private float _totalTime;
        private UpgradeEmployeeData _upgradeEmployeeData;
        private IWorldDataService _worldDataService;
        private Coroutine _timeCoroutine;
        private EmployeeDataService _employeeDataService;
        private WindowService _windowService;
        private AdService _adService;

        [Inject]
        private void Construct(IWorldDataService worldDataService,
            EmployeeDataService employeeDataService,
            WindowService windowService,
            AdService adService)
        {
            _adService = adService;
            _windowService = windowService;
            _employeeDataService = employeeDataService;
            _worldDataService = worldDataService;
        }

        private void OnEnable() =>
            _adButton.onClick.AddListener(AdButtonClickHandler);

        private void OnDisable() =>
            _adButton.onClick.RemoveListener(AdButtonClickHandler);

        public void Init(UpgradeEmployeeData upgradeEmployeeData, float totalTime)
        {
            _upgradeEmployeeData = upgradeEmployeeData;
            _totalTime = totalTime;

            _remainingTimeSlider.value = InitialSliderValue;

            if (_timeCoroutine != null)
                StopCoroutine(StartDecreaseTimeCoroutine());

            _timeCoroutine = StartCoroutine(StartDecreaseTimeCoroutine());
        }

        private void AdButtonClickHandler()
        {
            CloseButton.gameObject.SetActive(false);
            _adService.ShowVideo(Close);
        }

        [Button]
        public void SetValue(float time)
        {
            _remainingTimeSlider.value = time;
        }

        public override void Open()
        {
            _windowService.Close<HudWindow>();
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
        }

        private void SetCompleted()
        {
            _upgradeEmployeeData.SetCompleted(true);
            SetCompletedUI();

            DOTween.Sequence().AppendInterval(_destroyDelayOnCompleted).OnComplete(Close).SetUpdate(true);
        }

        private IEnumerator StartDecreaseTimeCoroutine()
        {
            _upgradeEmployeeData.SetUpgradeStarted(true);

            while (Math.Abs(_remainingTimeSlider.value - _remainingTimeSlider.maxValue) > 0.1f)
            {
                _totalTime -= Time.unscaledDeltaTime;

                int minutes = Mathf.FloorToInt(Mathf.Abs(_totalTime) % TimeConstantValue.SecondsInHour /
                                               TimeConstantValue.SecondsInMinute);

                var seconds = CalculateSliderValue();

                SetValuesToTexts(minutes, seconds);
                yield return null;
            }

            _remainingTimeSlider.DOValue(_remainingTimeSlider.maxValue, 0.5f).SetUpdate(true);
            SetCompleted();
        }

        private int CalculateSliderValue()
        {
            _remainingTimeSlider.value = Mathf.Lerp(_remainingTimeSlider.value, -Mathf.FloorToInt(
                Mathf.Abs(_totalTime) % TimeConstantValue.SecondsInHour /
                TimeConstantValue.SecondsInMinute), _sliderFillSpeed * Time.unscaledDeltaTime);
            int seconds = Mathf.FloorToInt(Mathf.Abs(_totalTime) % TimeConstantValue.SecondsInMinute);
            return seconds;
        }

        private void TryOpenCompletedWindow()
        {
            if (!_upgradeEmployeeData.Completed)
            {
                SaveLastUpgradeTime();
                _windowService.Open<HudWindow>();
                return;
            }

            var upgradeCompletedWindow = _windowService.Get<UpgradeEmployeeCompletedWindow>();
            upgradeCompletedWindow.Init(_upgradeEmployeeData.EmployeeData);
            _windowService.OpenCreatedWindow<UpgradeEmployeeCompletedWindow>();
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
        }
    }
}