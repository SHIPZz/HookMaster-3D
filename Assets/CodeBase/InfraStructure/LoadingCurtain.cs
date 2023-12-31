﻿using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.InfraStructure
{
    public class LoadingCurtain : MonoBehaviour, ILoadingCurtain
    {
        [SerializeField] private float _closeDuration =1f;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Slider _loadingSlider;
        private Canvas _canvas;

        public event Action Closed;

        private void Awake()
        {
            _canvas = _canvasGroup.GetComponent<Canvas>();
            _loadingSlider.value = 0;
            DontDestroyOnLoad(this);
        }

        private void OnDisable() =>
            _loadingSlider.value = 0;

        public void Show(float loadSliderDuration)
        {
            _loadingSlider.value = 0;
            _canvas.enabled = true;
            _loadingSlider.DOValue(_loadingSlider.maxValue, loadSliderDuration).SetUpdate(true);
            _canvasGroup.alpha = 1;
        }

        public async UniTaskVoid Hide()
        {
            while (Mathf.Approximately(_loadingSlider.value, _loadingSlider.maxValue) == false)
                await UniTask.Yield();

            _canvasGroup
                .DOFade(0, _closeDuration).SetUpdate(true)
                .OnComplete(() =>
                {
                    _canvas.enabled = false;
                    Closed?.Invoke();
                });
        }
    }
}