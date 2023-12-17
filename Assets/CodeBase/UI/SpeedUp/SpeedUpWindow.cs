using System;
using System.Collections;
using System.Linq;
using CodeBase.Constant;
using CodeBase.Data;
using CodeBase.Extensions;
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
        [SerializeField] private CanvasAnimator _canvasAnimator;
        [SerializeField] private TMP_Text _remainingTimeText;
        [SerializeField] private TMP_Text _skipAdText;
        [SerializeField] private TMP_Text _skipTicketText;
        [SerializeField] private TMP_Text _skipDiamondText;
        [SerializeField] private Slider _remainingTimeSlider;

        private float _totalTime = 3600f;
        private float _currentTime;
        private UpgradeEmployeeData _upgradeEmployeeData;
        private IWorldDataService _worldDataService;
        private Coroutine _timeCoroutine;
        private WorldTimeService _worldTimeService;

        [Inject]
        private void Construct(IWorldDataService worldDataService, WorldTimeService worldTimeService)
        {
            _worldTimeService = worldTimeService;
            _worldDataService = worldDataService;
        }

        private void OnDisable()
        {
            SaveLastUpgradeTime();
        }

        public async UniTaskVoid Init(UpgradeEmployeeData upgradeEmployeeData)
        {
            _upgradeEmployeeData = upgradeEmployeeData;

            _worldTimeService.UpdateWorldTime();

            while (!_worldTimeService.TimeUpdated)
            {
                await UniTask.Yield();
            }

            if (_upgradeEmployeeData.LastUpgradeWindowOpenedTime == 0)
                _upgradeEmployeeData.LastUpgradeWindowOpenedTime =
                    _worldDataService.WorldData.WorldTimeData.CurrentTime;

            TimeSpan timePassed = _worldDataService.WorldData.WorldTimeData.CurrentTime.ToDateTime() -
                                  _upgradeEmployeeData.LastUpgradeWindowOpenedTime.ToDateTime();

            print(_upgradeEmployeeData.LastUpgradeTime + " LAST UPGRADE TIME");
            

            _totalTime = _upgradeEmployeeData.LastUpgradeTime;
            var passedSeconds = (float)timePassed.TotalSeconds;
            _totalTime -= Mathf.Abs(passedSeconds);
            print(timePassed.TotalSeconds + " TotalSeconds");
            print(passedSeconds + " TotalSeconds FLOAT");
            // print(_totalTime + " _totalTime");
            // print(_upgradeEmployeeData.LastUpgradeWindowOpenedTime.ToDateTime() + " LastUpgradeWindowOpenedTime");
            // print(_worldDataService.WorldData.WorldTimeData.CurrentTime.ToDateTime() + " CurrentTime");

            if (_timeCoroutine != null)
                StopCoroutine(StartDecreaseTimeCoroutine());

            _timeCoroutine = StartCoroutine(StartDecreaseTimeCoroutine());
        }

        public override void Open()
        {
            _canvasAnimator.FadeInCanvas();
        }

        public override void Close()
        {
            _canvasAnimator.FadeOutCanvas(base.Close);
            print(_upgradeEmployeeData.EmployeeData.Name);
            SaveLastUpgradeTime();
        }

        private void SaveLastUpgradeTime()
        {
            print(_totalTime + " TOTAL TIME SAVED");
            _upgradeEmployeeData.LastUpgradeTime = _totalTime;
            _upgradeEmployeeData.LastUpgradeWindowOpenedTime = _worldDataService.WorldData.WorldTimeData.CurrentTime;
            _worldDataService.WorldData.UpgradeEmployeeDatas.RemoveAll(x =>
                x.EmployeeData.Guid == _upgradeEmployeeData.EmployeeData.Guid);
            _worldDataService.WorldData.UpgradeEmployeeDatas.Add(_upgradeEmployeeData);

            _worldDataService.Save();
        }

        private IEnumerator StartDecreaseTimeCoroutine()
        {
            while (_totalTime != 0)
            {
                _totalTime -= Time.deltaTime;

                float targetValue = Mathf.Abs(_totalTime) % TimeConstantValue.SecondsInHour / TimeConstantValue.SecondsInMinute;

                _remainingTimeSlider.value = Mathf.Lerp(_remainingTimeSlider.value, -targetValue, 15 * Time.deltaTime);

                int minutes = Mathf.FloorToInt(Mathf.Abs(_totalTime) % TimeConstantValue.SecondsInHour /
                                               TimeConstantValue.SecondsInMinute);
                int seconds = Mathf.FloorToInt(Mathf.Abs(_totalTime) % TimeConstantValue.SecondsInMinute);


                _remainingTimeText.text = $"{minutes}m {seconds}s";
                _skipDiamondText.text = $"{minutes}m";
                _skipTicketText.text = $"{minutes}m";
                yield return null;
            }

            _remainingTimeSlider.value = _remainingTimeSlider.maxValue;
        }
    }
}