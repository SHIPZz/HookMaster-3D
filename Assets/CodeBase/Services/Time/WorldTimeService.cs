﻿using System;
using System.Collections;
using CodeBase.Data;
using CodeBase.Extensions;
using CodeBase.Services.Coroutine;
using CodeBase.Services.WorldData;
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
        public bool TimeUpdated { get; private set; }

        public WorldTimeService(IWorldDataService worldDataService, ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
            _worldDataService = worldDataService;
        }

        public void Initialize()
        {
          _worldTimeCoroutine =  _coroutineRunner.StartCoroutine(GetWorldTimeCoroutine());
            Application.focusChanged += OnFocusCnanged;
        }

        public void Dispose()
        {
            Application.focusChanged -= OnFocusCnanged;
        }

        private void OnFocusCnanged(bool hasFocus)
        {
            if(_worldTimeCoroutine != null)
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

        public void SaveLastSalaryPaymentTime()
        {
            _worldDataService.WorldData.WorldTimeData.LastSalaryPaymentTime =  _worldDataService.WorldData.WorldTimeData.CurrentTime;
            _worldDataService.Save();
        }

        public double GetTimeDifferenceByMinutes()
        {
            WorldTimeData worldTimeData = _worldDataService.WorldData.WorldTimeData;

            TimeSpan timeDifference = worldTimeData.CurrentTime.ToDateTime() - worldTimeData.LastVisitedTime.ToDateTime();
            
            Debug.Log(worldTimeData.CurrentTime.ToDateTime() + " CurrentTime");
            Debug.Log(worldTimeData.LastVisitedTime.ToDateTime() + " LastVisitedTime");

            return timeDifference.TotalMinutes;
        }

        public void SaveLastProfitEarnedTime()
        {
            _worldDataService.WorldData.WorldTimeData.LastEarnedProfitTime = _worldDataService.WorldData.WorldTimeData.CurrentTime;
            _worldDataService.Save();
        }

        public int GetTimeDifferenceByDay()
        {
            WorldTimeData worldTimeData = _worldDataService.WorldData.WorldTimeData;

            TimeSpan timeDifference = worldTimeData.CurrentTime.ToDateTime() - worldTimeData.LastVisitedTime.ToDateTime();

            return timeDifference.Days;
        }

        public int GetTimeDifferenceByDaysBetweenProfitAndCurrentTime()
        {
            WorldTimeData worldTimeData = _worldDataService.WorldData.WorldTimeData;

            if (worldTimeData.LastEarnedProfitTime == 0)
                worldTimeData.LastEarnedProfitTime = _worldDataService.WorldData.WorldTimeData.CurrentTime;

            TimeSpan timeDifference = worldTimeData.CurrentTime.ToDateTime() - worldTimeData.LastEarnedProfitTime.ToDateTime();

            Debug.Log(worldTimeData.LastEarnedProfitTime.ToDateTime() + "last profit payment");
            return timeDifference.Days;
        }

        public int GetTimeDifferenceByDaysBetweenSalaryPaymentAndCurrentTime()
        {
            WorldTimeData worldTimeData = _worldDataService.WorldData.WorldTimeData;

            if (worldTimeData.LastSalaryPaymentTime == 0)
                worldTimeData.LastSalaryPaymentTime = _worldDataService.WorldData.WorldTimeData.CurrentTime;

            TimeSpan timeDifference = worldTimeData.CurrentTime.ToDateTime() - worldTimeData.LastSalaryPaymentTime.ToDateTime();
            Debug.Log(worldTimeData.LastSalaryPaymentTime.ToDateTime() + "last salary payment");
            return timeDifference.Days;
        }
        
        private void SaveLastVisitedTime()
        {
            if (!GotTime)
                return;

            _worldDataService.WorldData.WorldTimeData.LastVisitedTime = _worldDataService.WorldData.WorldTimeData.CurrentTime;
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
                    WorldTimeApiResponse response = JsonUtility.FromJson<WorldTimeApiResponse>(webRequest.downloadHandler.text);
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