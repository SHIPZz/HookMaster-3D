using System;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;

namespace CodeBase.Animations
{
    public class CanvasAnimator : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _fadeInDuration = 0.3f;
        [SerializeField] private float _fadeOutDuration = 0.25f;

        private Tween _fadeTween;
        private Canvas _canvas;

        private void Awake()
        {
            _canvasGroup.alpha = 0f;
            _canvas = _canvasGroup.GetComponent<Canvas>();
        }

        public void FadeInCanvas([CanBeNull] Action onCompleted = null)
        {
            _fadeTween?.Kill(true);
            _canvasGroup.interactable = true;
            _canvas.enabled = true;
            _fadeTween = _canvasGroup.DOFade(1f, _fadeInDuration).OnComplete(() => onCompleted?.Invoke()).SetUpdate(true);
        }

        public void FadeOutCanvas([CanBeNull] Action onCompleted = null)
        {
            _fadeTween?.Kill(true);

            _fadeTween = _canvasGroup.DOFade(0f, _fadeOutDuration)
                .OnComplete(() =>
                {
                    _canvasGroup.interactable = false;
                    _canvas.enabled = false;
                    onCompleted?.Invoke();
                }).SetUpdate(true);
        }
    }
}