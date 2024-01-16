﻿using System;
using System.Collections;
using System.Collections.Generic;
using CodeBase.Animations;
using CodeBase.Constant;
using CodeBase.Data;
using CodeBase.Extensions;
using CodeBase.Services.Employee;
using CodeBase.Services.WorldData;
using CodeBase.UI.Buttons;
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
        [SerializeField] private GameObject _completedItem;
        [SerializeField] private List<RectTransformScaleAnim> _transformScaleAnims;
        [SerializeField] private float _sliderFillSpeed = 15f;
        [SerializeField] private List<CheckOutButton> _checkOutButtons;
        
        private float _initialSliderValue;
        private float _currentTime;
        private float _totalTime;
        private UpgradeEmployeeData _upgradeEmployeeData;
        private IWorldDataService _worldDataService;
        private Coroutine _timeCoroutine;
        private EmployeeDataService _employeeDataService;

        [Inject]
        private void Construct(IWorldDataService worldDataService, EmployeeDataService employeeDataService)
        {
            _employeeDataService = employeeDataService;
            _worldDataService = worldDataService;
        }

        private void OnDestroy() =>
            SaveLastUpgradeTime();

        public override void Open()
        {
            _checkOutButtons.ForEach(x =>x.Successful += SetAnimatedFinished);
            _canvasAnimator.FadeInCanvas();
        }

        public override void Close()
        {
            _canvasAnimator.FadeOutCanvas(base.Close);
            _checkOutButtons.ForEach(x =>x.Successful -= SetAnimatedFinished);
        }

        public void Init(UpgradeEmployeeData upgradeEmployeeData)
        {
            _upgradeEmployeeData = upgradeEmployeeData;
            _totalTime = upgradeEmployeeData.LastUpgradeTime;

            _remainingTimeSlider.value = InitialSliderValue;

            if (_timeCoroutine != null)
                StopCoroutine(StartDecreaseTimeCoroutine());

            _timeCoroutine = StartCoroutine(StartDecreaseTimeCoroutine());
        }

        private void SetAnimatedFinished()
        {
            _transformScaleAnims.ForEach(x => x.UnScale());
            StopCoroutine(StartDecreaseTimeCoroutine());
            _remainingTimeSlider.DOValue(_remainingTimeSlider.maxValue, 0.5f).OnComplete(SetCompleted);
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
            _transformScaleAnims.ForEach(x => x.UnScale());
            _completedItem.SetActive(true);
            _upgradeEmployeeData.SetCompleted(true);
            _upgradeEmployeeData.Completed = true;
            _remainingTimeText.text = "Completed";
            SaveLastUpgradeTime();
        }

        private void SaveLastUpgradeTime()
        {
            _upgradeEmployeeData
                .SetLastUpgradeTime(Mathf.Abs(_totalTime))
                .SetLastUpgradeWindowOpenedTime(_worldDataService.WorldData.WorldTimeData.CurrentTime);
            _employeeDataService.SaveUpgradeEmployeeData(_upgradeEmployeeData);
        }

        private IEnumerator StartDecreaseTimeCoroutine()
        {
            _upgradeEmployeeData.SetUpgradeStarted(true);

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