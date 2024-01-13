using System;
using System.Collections;
using CodeBase.Constant;
using CodeBase.Data;
using CodeBase.Extensions;
using CodeBase.Services.Coroutine;
using CodeBase.Services.WorldData;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

namespace CodeBase.Services.Time
{
    public class WorldTimeService : IInitializable, IDisposable
    {
        private const string ApiUrl = "http://worldtimeapi.org/api/ip";

        private readonly ICoroutineRunner _coroutineRunner;
        private readonly IWorldDataService _worldDataService;
        private UnityEngine.Coroutine _worldTimeCoroutine;

        public bool GotTime { get; private set; }
        public long CurrentTime { get; private set; }

        public bool TimeUpdated;

        public WorldTimeService(IWorldDataService worldDataService, ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
            _worldDataService = worldDataService;
        }

        public void Initialize()
        {
            _worldTimeCoroutine = _coroutineRunner.StartCoroutine(GetWorldTimeCoroutine());
            Application.focusChanged += OnFocusCnanged;
        }

        public void Dispose()
        {
            Application.focusChanged -= OnFocusCnanged;
        }

        private void OnFocusCnanged(bool hasFocus)
        {
            if (_worldTimeCoroutine != null)
                _coroutineRunner.StopCoroutine(GetWorldTimeCoroutine());

            switch (hasFocus)
            {
                case true:
                    _coroutineRunner.StartCoroutine(GetWorldTimeCoroutine());
                    break;
                case false:
                    Debug.Log("save");
                    SaveLastVisitedTime();
                    break;
            }
        }

        public async UniTask UpdateWorldTime()
        {
            if (_worldTimeCoroutine != null)
                _coroutineRunner.StopCoroutine(GetWorldTimeCoroutine());

            _worldTimeCoroutine = _coroutineRunner.StartCoroutine(GetWorldTimeCoroutine());

            while (!TimeUpdated)
            {
                await UniTask.Yield();
            }

            TimeUpdated = false;
        }

        public void ResetLastMiningTime()
        {
            // _worldDataService.WorldData.MiningFarmDatas.LastWorkingTime = 0;
        }

        public void ResetLastSpawnedRandomItemTime()
        {
            _worldDataService.WorldData.RandomItemData.LastSpawnedTime = 0;
        }

        public void SetLastSpawnedTime()
        {
            _worldDataService.WorldData.RandomItemData.LastSpawnedTime =
                _worldDataService.WorldData.WorldTimeData.CurrentTime;
        }

        public int GetTimeDifferenceByLastSpawnedRandomItemInMinutes()
        {
            if (_worldDataService.WorldData.RandomItemData.LastSpawnedTime == 0)
            {
                _worldDataService.WorldData.RandomItemData.LastSpawnedTime =
                    _worldDataService.WorldData.WorldTimeData.CurrentTime;
            }

            TimeSpan timeDifference = _worldDataService.WorldData.WorldTimeData.CurrentTime.ToDateTime() -
                                      _worldDataService.WorldData.RandomItemData.LastSpawnedTime.ToDateTime();

            return (int)timeDifference.TotalMinutes;
        }

        public void SaveLastSalaryPaymentTime()
        {
            _worldDataService.WorldData.WorldTimeData.LastSalaryPaymentTime =
                _worldDataService.WorldData.WorldTimeData.CurrentTime;
            _worldDataService.Save();
        }

        public void SaveLastProfitEarnedTime()
        {
            _worldDataService.WorldData.WorldTimeData.LastEarnedProfitTime =
                _worldDataService.WorldData.WorldTimeData.CurrentTime;
            _worldDataService.Save();
        }

        public void SaveLastCircleRoulettePlayedTime(string id)
        {
            _worldDataService.WorldData.CircleRouletteItemDatas[id].LastPlayedTime =
                _worldDataService.WorldData.WorldTimeData.CurrentTime;
            _worldDataService.Save();
        }


        public int GetTimeDifferenceByLastCircleRoulettePlayTimeDays(string id)
        {
            TimeSpan timeSpan = _worldDataService.WorldData.WorldTimeData.CurrentTime.ToDateTime() -
                                _worldDataService.WorldData.CircleRouletteItemDatas[id].LastPlayedTime.ToDateTime();

            return (int)timeSpan.TotalDays;
        }

        public int GetTimeDifferenceByLastLazinessDays()
        {
            WorldTimeData worldTimeData = _worldDataService.WorldData.WorldTimeData;

            if (worldTimeData.LastLazyDay == 0)
                worldTimeData.LastEarnedProfitTime = _worldDataService.WorldData.WorldTimeData.CurrentTime;

            TimeSpan timeDifference = worldTimeData.CurrentTime.ToDateTime() - worldTimeData.LastLazyDay.ToDateTime();

            return timeDifference.Days;
        }

        public int GetTimeDifferenceLastFireTimeByMinutes()
        {
            WorldTimeData worldTimeData = _worldDataService.WorldData.WorldTimeData;

            if (worldTimeData.LastFireTime == 0)
                worldTimeData.LastFireTime = _worldDataService.WorldData.WorldTimeData.CurrentTime;


            TimeSpan timeDifference = worldTimeData.CurrentTime.ToDateTime() - worldTimeData.LastFireTime.ToDateTime();

            return (int)timeDifference.TotalMinutes;
        }

        public void SaveLastFireTime()
        {
            WorldTimeData worldTimeData = _worldDataService.WorldData.WorldTimeData;
            worldTimeData.LastFireTime = _worldDataService.WorldData.WorldTimeData.CurrentTime;
        }

        public void SaveLastLazyDay()
        {
            _worldDataService.WorldData.WorldTimeData.LastLazyDay =
                _worldDataService.WorldData.WorldTimeData.CurrentTime;
            _worldDataService.Save();
        }

        public int GetTimeDifferenceByMinutesBetweenProfitAndCurrentTime()
        {
            WorldTimeData worldTimeData = _worldDataService.WorldData.WorldTimeData;

            if (worldTimeData.LastEarnedProfitTime == 0)
                worldTimeData.LastEarnedProfitTime = _worldDataService.WorldData.WorldTimeData.CurrentTime;

            TimeSpan timeDifference =
                worldTimeData.CurrentTime.ToDateTime() - worldTimeData.LastEarnedProfitTime.ToDateTime();

            int passedMinutes = (int)timeDifference.TotalMinutes;

            passedMinutes = Mathf.Clamp(passedMinutes, 0, TimeConstantValue.MinutesInTwoHour);

            return passedMinutes;
        }

        public int GetTimeDifferenceByMinutesBetweenSalaryPaymentAndCurrentTime()
        {
            WorldTimeData worldTimeData = _worldDataService.WorldData.WorldTimeData;

            if (worldTimeData.LastSalaryPaymentTime == 0)
                worldTimeData.LastSalaryPaymentTime = _worldDataService.WorldData.WorldTimeData.CurrentTime;

            TimeSpan timeDifference = worldTimeData.CurrentTime.ToDateTime() -
                                      worldTimeData.LastSalaryPaymentTime.ToDateTime();
            return (int)timeDifference.TotalMinutes;
        }

        public int GetLastVisitedTimeByMinutes()
        {
            var timeDifference = _worldDataService.WorldData.WorldTimeData.CurrentTime.ToDateTime() -
                                 _worldDataService.WorldData.WorldTimeData.LastVisitedTime.ToDateTime();

            return (int)timeDifference.TotalMinutes;
        }

        private void SaveLastVisitedTime()
        {
            if (!GotTime)
                return;

            _worldDataService.WorldData.WorldTimeData.LastVisitedTime =
                _worldDataService.WorldData.WorldTimeData.CurrentTime;
            TimeUpdated = false;
            _worldDataService.Save();
        }

        private IEnumerator GetWorldTimeCoroutine()
        {
            using UnityWebRequest webRequest = UnityWebRequest.Get(ApiUrl);

            yield return webRequest.SendWebRequest();

            if (webRequest.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + webRequest.error);
            }
            else
            {
                try
                {
                    WorldTimeApiResponse response =
                        JsonUtility.FromJson<WorldTimeApiResponse>(webRequest.downloadHandler.text);
                    DateTime worldDateTime = DateTime.Parse(response.utc_datetime);

                    _worldDataService.WorldData.WorldTimeData.CurrentTime = worldDateTime.ToUnixTime();
                    _worldDataService.Save();
                    GotTime = true;
                    TimeUpdated = true;
                    Debug.Log("World Time: " + worldDateTime.ToUnixTime().ToDateTime());
                }
                catch (Exception e)
                {
                    Debug.LogError("Error parsing world time response: " + e.Message);
                }
            }
        }
    }

    [Serializable]
    public class WorldTimeApiResponse
    {
        public string utc_datetime;
    }
}