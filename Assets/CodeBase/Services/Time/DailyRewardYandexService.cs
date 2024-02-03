/*using System;
using System.Globalization;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

namespace ExampleYGDateTime
{
    public class DailyRewardYandexService : IInitializable
    {
        private bool _isNetworkError;
        private bool _isHttpError;
        private bool _isLoaded;
        private bool _isCompleteLoaded;

        private bool isLocalDataFounded;

        public  void Initialize()
        {
            
        }

        protected async UniTask SendRequest()
        {
            try
            {
                using UnityWebRequest webRequest = UnityWebRequest.Head(Application.absoluteURL);
                webRequest.SetRequestHeader("cache-control", "no-cache");
                await webRequest.SendWebRequest();

                switch (webRequest.result)
                {
                    case UnityWebRequest.Result.ConnectionError:
                        _isNetworkError = true;
                        _isLoaded = true;
                        Debug.Log($"[DailyRewardYandexService] => Network Error! -> {webRequest.error}");
                        break;
                    case UnityWebRequest.Result.DataProcessingError:
                        _isHttpError = true;
                        _isLoaded = true;
                        Debug.Log($"[DailyRewardYandexService] => Data Processing Error! -> {webRequest.error}");
                        break;
                    case UnityWebRequest.Result.ProtocolError:
                        _isHttpError = true;
                        _isLoaded = true;
                        Debug.Log($"[DailyRewardYandexService] => Protocol Error! -> {webRequest.error}");
                        break;
                    case UnityWebRequest.Result.Success:
                        string dateString = webRequest.GetResponseHeader("date");
                        Debug.Log($"[DailyRewardYandexService] => Yandex server time -> {dateString}");
                        DateTimeOffset date = DateTimeOffset.ParseExact(dateString, "ddd, dd MMM yyyy HH:mm:ss 'GMT'", 
                            CultureInfo.InvariantCulture, 
                            DateTimeStyles.AssumeUniversal);
                        Debug.Log($"[DailyRewardYandexService] => Server time in date -> {date}");
                        serverTime = (int)date.ToUnixTimeSeconds();
                        Debug.Log($"[DailyRewardYandexService] => Server time in second -> {serverTime}");

                        _isCompleteLoaded = true;
                        _isLoaded = true;
                        break;
                }
            }
            catch (Exception)
            {
                _isNetworkError = true;
                _isLoaded = true;
                throw;
            }
        }
    }
}*/