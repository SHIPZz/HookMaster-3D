using System;
using CodeBase.Services.Pause;
using UnityEngine;

namespace CodeBase.Services.Ad
{
    public class AdService
    {
        private readonly IPauseService _pauseService;

        private Action _successCallback;

        public AdService(IPauseService pauseService)
        {
            _pauseService = pauseService;
        }

        public void ShowVideo(Action onSuccessCallback)
        {
            _successCallback = onSuccessCallback;
#if UNITY_WEBGL && !UNITY_EDITOR
            VideoAd.Show(OnOpenCallback, null, OnCloseCallback, OnErrorCallback);
#endif
        }

        public void ShowInterstitial(Action onSuccessCallback)
        {
            _successCallback = onSuccessCallback;
#if UNITY_WEBGL && !UNITY_EDITOR
            InterstitialAd.Show(OnInterstitialOpenCallback, OnInterstitialCloseCallback, OnInterstitialErrorCallback,
                OnInterstitialOfflineCallback);
#endif
        }

        private void OnInterstitialOfflineCallback() =>
            RunAll();

        private void OnInterstitialErrorCallback(string obj) =>
            RunAll();

        private void OnInterstitialCloseCallback(bool isClosed)
        {
            if (!isClosed)
                return;
            
            RunAll();
            _successCallback?.Invoke();
        }

        private void OnInterstitialOpenCallback() =>
            StopAll();

        private void OnErrorCallback(string error) =>
            RunAll();

        private void OnCloseCallback()
        {
            RunAll();
            _successCallback?.Invoke();
        }

        private void OnOpenCallback() =>
            StopAll();

        private void StopAll()
        {
            _pauseService.Stop();
            AudioListener.volume = 0f;
            AudioListener.pause = true;
        }

        private void RunAll()
        {
            _pauseService.Run();
            AudioListener.volume = 1f;
            AudioListener.pause = false;
        }
    }
}