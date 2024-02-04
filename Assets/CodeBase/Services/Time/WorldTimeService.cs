using System;
using System.Collections;
using System.Globalization;
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
    public class WorldTimeService :  IDisposable
    {
        private const string ApiUrl = "http://worldtimeapi.org/api/ip";

        private readonly string[] BackupApiUrls =
            { "http://backup1.worldtimeapi.org/api/ip", "http://backup2.worldtimeapi.org/api/ip" };

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
            Application.focusChanged += OnFocusChanged;
        }

        public void Dispose()
        {
            Application.focusChanged -= OnFocusChanged;
        }

        private void OnFocusChanged(bool hasFocus)
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

            Debug.Log("update world time");

            _worldTimeCoroutine = _coroutineRunner.StartCoroutine(GetWorldTimeCoroutine());

            while (!TimeUpdated)
            {
                await UniTask.Yield();
            }

            TimeUpdated = false;
        }

        public void ResetLastSpawnedRandomItemTime()
        {
            _worldDataService.WorldData.RandomItemData.LastSpawnedTime = 0;
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
                worldTimeData.LastLazyDay = _worldDataService.WorldData.WorldTimeData.CurrentTime;

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

        public int GetMiningFarmLastCleanTime(string id)
        {
            MiningFarmData targetMiningFarmData = _worldDataService.WorldData.MiningFarmDatas[id];
            WorldTimeData worldTimeData = _worldDataService.WorldData.WorldTimeData;

            if (targetMiningFarmData.LastCleanTime == 0)
                targetMiningFarmData.LastCleanTime = worldTimeData.CurrentTime;

            TimeSpan timeDifference =
                worldTimeData.CurrentTime.ToDateTime() - targetMiningFarmData.LastCleanTime.ToDateTime();
            return (int)timeDifference.TotalMinutes;
        }

        public void SaveMiningFarmLastCleanTime(string id)
        {
            _worldDataService.WorldData.MiningFarmDatas[id].LastCleanTime =
                _worldDataService.WorldData.WorldTimeData.CurrentTime;
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
            _worldDataService.WorldData.WorldTimeData.CurrentTime = DateTime.Now.ToUnixTime();
            _worldDataService.Save();
            GotTime = true;
            TimeUpdated = true;
            yield return null;
            
            // using UnityWebRequest webRequest = UnityWebRequest.Get(ApiUrl);
            // webRequest.SetRequestHeader("cache-control", "no-cache");
            // yield return webRequest.SendWebRequest();
            //
            // if (webRequest.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError)
            // {
            //     Debug.LogError("Error: webrequest");
            //     yield return TryBackupServers();
            // }
            // else
            // {
            //     try
            //     {
            //         WorldTimeApiResponse response =
            //             JsonUtility.FromJson<WorldTimeApiResponse>(webRequest.downloadHandler.text);
            //         DateTime worldDateTime = DateTime.Parse(response.utc_datetime);
            //
            //         _worldDataService.WorldData.WorldTimeData.CurrentTime = worldDateTime.ToUnixTime();
            //         _worldDataService.Save();
            //         GotTime = true;
            //         TimeUpdated = true;
            //         Debug.Log("World Time: " + worldDateTime.ToUnixTime().ToDateTime());
            //     }
            //     catch (Exception e)
            //     {
            //         Debug.LogError("Error parsing world time response: " + e.Message);
            //     }
            // }
        }

        private IEnumerator TryBackupServers()
        {
            foreach (string backupUrl in BackupApiUrls)
            {
                using UnityWebRequest backupWebRequest = UnityWebRequest.Get(ApiUrl);
                backupWebRequest.SetRequestHeader("cache-control", "no-cache");
                yield return backupWebRequest.SendWebRequest();

                if (backupWebRequest.result != UnityWebRequest.Result.Success)
                    continue;

                try
                {
                    WorldTimeApiResponse response =
                        JsonUtility.FromJson<WorldTimeApiResponse>(backupWebRequest.downloadHandler.text);
                    DateTime worldDateTime = DateTime.Parse(response.utc_datetime);

                    _worldDataService.WorldData.WorldTimeData.CurrentTime = worldDateTime.ToUnixTime();
                    _worldDataService.Save();
                    GotTime = true;
                    TimeUpdated = true;
                    Debug.Log("World Time (Backup): " + worldDateTime.ToUnixTime().ToDateTime());
                }
                catch (Exception e)
                {
                    Debug.LogError("Error parsing backup world time response: " + e.Message);
                }

                yield break;
            }

            Debug.LogError("Failed to connect to any server.");
        }
    }

    [Serializable]
    public class WorldTimeApiResponse
    {
        public string utc_datetime;
    }
}