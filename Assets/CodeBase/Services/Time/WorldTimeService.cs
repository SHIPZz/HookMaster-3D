using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using CodeBase.Constant;
using CodeBase.Data;
using CodeBase.Extensions;
using CodeBase.Services.Coroutine;
using CodeBase.Services.WorldData;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CodeBase.Services.Time
{
    public class WorldTimeService : IDisposable
    {
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly IWorldDataService _worldDataService;
        private UnityEngine.Coroutine _worldTimeCoroutine;

        public bool GotTime { get; private set; }

        public bool TimeUpdated;

        public WorldTimeService(IWorldDataService worldDataService, ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
            _worldDataService = worldDataService;
        }

        public async void Initialize()
        {
            // _worldTimeCoroutine = _coroutineRunner.StartCoroutine(GetWorldTimeCoroutine());
            await InitializeTime();
            Application.focusChanged += OnFocusChanged;
        }

        public void Dispose()
        {
            Application.focusChanged -= OnFocusChanged;
        }

        private async void OnFocusChanged(bool hasFocus)
        {
            if (_worldTimeCoroutine != null)
                _coroutineRunner.StopCoroutine(GetWorldTimeCoroutine());

            switch (hasFocus)
            {
                case true:
                    await InitializeTime();
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

            await InitializeTime();
            // _worldTimeCoroutine = _coroutineRunner.StartCoroutine(GetWorldTimeCoroutine());

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
            // GetCurrentTime();
            // var client = new TcpClient("time.nist.gov", 13);
            //
            // using var streamReader = new StreamReader(client.GetStream());
            //
            // var response = streamReader.ReadToEnd();
            // var utcDateTimeString = response.Substring(7, 17);
            // DateTime localDateTime = DateTime.ParseExact(utcDateTimeString, "yy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);

            Debug.Log(_worldDataService.WorldData.WorldTimeData.CurrentTime.ToDateTime().Minute);
            Debug.Log(_worldDataService.WorldData.WorldTimeData.CurrentTime.ToDateTime());

            yield return null;
        }

        private async UniTask InitializeTime()
        {
            DateTime currentTime = await GetCurrentTime();
            _worldDataService.WorldData.WorldTimeData.CurrentTime = currentTime.ToUnixTime();
            _worldDataService.Save();
            GotTime = true;
            TimeUpdated = true;
        }

        private async UniTask<DateTime> GetCurrentTime()
        {
            const String ntpServer = "pool.ntp.org";
            var ntpData = new Byte[48];
            ntpData[0] = 0x1B; //LeapIndicator = 0 (no warning), VersionNum = 3 (IPv4 only), Mode = 3 (Client Mode)

            try
            {
                var addresses = Dns.GetHostEntry(ntpServer).AddressList;
                var ipEndPoint = new IPEndPoint(addresses[0], 123);
                var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp)
                    { ReceiveTimeout = 5000, SendTimeout = 5000 };

                // milliseconds
                socket.Connect(ipEndPoint);
                socket.Send(ntpData);
                socket.Receive(ntpData);
                socket.Close();

                var intPart = (UInt64)ntpData[40] << 24 | (UInt64)ntpData[41] << 16 | (UInt64)ntpData[42] << 8 |
                              (UInt64)ntpData[43];
                var fractPart = (UInt64)ntpData[44] << 24 | (UInt64)ntpData[45] << 16 | (UInt64)ntpData[46] << 8 |
                                (UInt64)ntpData[47];

                var milliseconds = intPart * 1000 + (fractPart * 1000) / 0x100000000L;
                if (milliseconds == 0) //Corrupted packet received, try again later
                {
                    Debug.LogError("Received corrupted packet");
                    Thread.Sleep(1000);
                    await GetCurrentTime();
                    return DateTime.Now;
                }

                DateTime networkDateTime = new DateTime(1900, 1, 1).AddMilliseconds((Int64)milliseconds);
                Debug.Log("datetime network");
                return networkDateTime;
            }
            catch
            {
                //Process exception
            }

            Debug.Log("datetime now");
            return DateTime.Now;
        }

        /*private async UniTask<DateTime> GetCurrentTime()
        {
            var client = new TcpClient("time.nist.gov", 13);

            using var streamReader = new StreamReader(client.GetStream());
            var response = streamReader.ReadToEnd();

            while (string.IsNullOrEmpty(response))
                await UniTask.Yield();

            var utcDateTimeString = response.Substring(7, 17);
            DateTime localDateTime = DateTime.ParseExact(utcDateTimeString, "yy-MM-dd HH:mm:ss",
                CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
            Debug.Log(localDateTime);
            return localDateTime;
        }
        */

        // private async UniTask<DateTime> GetCurrentTime()
        // {
        //     using var client = new HttpClient();
        //     try
        //     {
        //         HttpResponseMessage result = client.GetAsync("https://google.com",
        //             HttpCompletionOption.ResponseHeadersRead).Result;
        //
        //         Debug.Log(result.StatusCode);
        //         
        //         
        //         while (result.StatusCode != HttpStatusCode.OK)
        //         {
        //             await UniTask.Yield();
        //         }
        //
        //         Debug.Log($"RETURN DATETIME FROM INTERNET {result.Headers.Date.Value.DateTime.Date}");
        //         NtpImpl();
        //         GetTime();
        //         return result.Headers.Date.Value.DateTime;
        //     }
        //     catch
        //     {
        //         Debug.Log("RETURN DATETIME.NOW");
        //         return DateTime.Now;
        //     }
    }
}


[Serializable]
public class WorldTimeApiResponse
{
    public string utc_datetime;
}