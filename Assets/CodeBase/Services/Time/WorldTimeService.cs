using System;
using System.Collections;
using CodeBase.Data;
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

        public bool GotTime { get; private set; }

        public WorldTimeService(IWorldDataService worldDataService, ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
            _worldDataService = worldDataService;
        }

        public void Initialize()
        {
            _coroutineRunner.StartCoroutine(GetWorldTime());
            Application.quitting += SaveLastVisitedTime;
        }

        public void Dispose()
        {
            Application.quitting -= SaveLastVisitedTime;
        }

        public int GetTimeDifferenceByDay()
        {
            WorldTimeData worldTimeData = _worldDataService.WorldData.WorldTimeData;
            
            TimeSpan timeDifference = worldTimeData.CurrentTime - worldTimeData.LastVisitedTime;
            
            return timeDifference.Days;
        }

        private void SaveLastVisitedTime()
        {
            if (!GotTime)
                return;
            
            WorldTimeData worldTimeData = _worldDataService.WorldData.WorldTimeData;
            worldTimeData.LastVisitedTime = worldTimeData.CurrentTime;
            _worldDataService.Save();
        }

        private IEnumerator GetWorldTime()
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
                    _worldDataService.WorldData.WorldTimeData.CurrentTime = worldDateTime;
                    _worldDataService.Save();
                    GotTime = true;
                    Debug.Log("World Time: " + worldDateTime);
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