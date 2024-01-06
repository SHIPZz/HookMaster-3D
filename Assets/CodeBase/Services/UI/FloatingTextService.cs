using System;
using CodeBase.Services.Factories.UI;
using CodeBase.Services.GOPool;
using CodeBase.UI.FloatingText;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace CodeBase.Services.UI
{
    public class FloatingTextService
    {
        private Vector2 _initialTextAnchoredPosition;
        private TMP_Text _targetText;
        private readonly ObjectPool<TMP_Text, string, Transform> _textObjectPool;
        private Tween _fadeTween;
        private bool _completed = true;
        private Tween _moveTween;

        public FloatingTextService(UIFactory uiFactory)
        {
            _textObjectPool = new ObjectPool<TMP_Text, string, Transform>(uiFactory.CreateElement<TMP_Text>, 10);
        }

        public void Fade(float fade, float duration)
        {
            if (_targetText == null)
                return;

            var rectTransformAnimator = _targetText.GetComponent<RectTransformAnimator>();
            rectTransformAnimator.FadeText(_targetText, fade, duration);
        }
        
        public void ShowFloatingText(FloatingTextView floatingTextView,  float targetAnchoredPositionY,
            float duration,
            float fadeInDuration,
            float fadeOutDuration,
            Quaternion rotation, Vector3 position,
            Action onComplete = null)
        {
            _completed = false;
            _targetText = floatingTextView.Text;
            
            _targetText.transform.position = position;
            ConfigureText(rotation, _targetText.text);
            var rectTransformAnimator = _targetText.GetComponent<RectTransformAnimator>();
            ConfigureRectTransformAnimator(rectTransformAnimator, fadeInDuration);

            _moveTween = rectTransformAnimator.MoveAnchoredPositionY(targetAnchoredPositionY, duration,
                () =>
                {
                    _moveTween = null;
                    HandleFadeOut(rectTransformAnimator, fadeOutDuration, onComplete);
                });
        }

        public async void ShowFloatingText(float targetAnchoredPositionY,
            float duration,
            float fadeInDuration,
            float fadeOutDuration,
            Quaternion rotation,
            string path, Transform parent, Vector3 position)
        {
            // while (!_completed)
            // {
            //     await UniTask.Yield();
            // }

            _completed = false;
            // if (_moveTween != null || _fadeTween != null)
            //     return;
            
            _targetText = _textObjectPool.Pop(path, parent);

            _targetText.transform.position = position;
            ConfigureText(rotation, _targetText.text);
            var rectTransformAnimator = _targetText.GetComponent<RectTransformAnimator>();
            ConfigureRectTransformAnimator(rectTransformAnimator, fadeInDuration);

            _moveTween = rectTransformAnimator.MoveAnchoredPositionY(targetAnchoredPositionY, duration,
                () =>
                {
                    _moveTween = null;
                    HandleFadeOut(rectTransformAnimator, fadeOutDuration);
                });
        }

        public void ShowFloatingText(string text, float targetAnchoredPositionY,
            float duration,
            float fadeInDuration,
            float fadeOutDuration,
            Quaternion rotation,
            string path, Transform parent)
        {
            _targetText = _textObjectPool.Pop(path, parent);

            ConfigureText(rotation, text);
            var rectTransformAnimator = _targetText.GetComponent<RectTransformAnimator>();
            ConfigureRectTransformAnimator(rectTransformAnimator, fadeInDuration);

            rectTransformAnimator.MoveAnchoredPositionY(targetAnchoredPositionY, duration,
                () => HandleFadeOut(rectTransformAnimator, fadeOutDuration));
        }

        private void ConfigureText(Quaternion rotation, string text)
        {
            _targetText.text = text;

            _targetText.gameObject.SetActive(true);
            _targetText.rectTransform.rotation = rotation;
        }

        private void ConfigureRectTransformAnimator(RectTransformAnimator rectTransformAnimator, float fadeInDuration)
        {
            rectTransformAnimator.SetInitialPosition();
            rectTransformAnimator.SetRotation(rectTransformAnimator.transform.rotation);

            rectTransformAnimator.FadeText(_targetText, 1f, fadeInDuration);
        }

        private void HandleFadeOut(RectTransformAnimator rectTransformAnimator, float fadeOutDuration,
            Action onComplete = null)
        {
            _fadeTween = rectTransformAnimator.FadeText(_targetText, 0f, fadeOutDuration, () =>
            {
                rectTransformAnimator.SetInitialPosition();
                _textObjectPool.Push(_targetText);
                _fadeTween = null;
                _completed = true;
                onComplete?.Invoke();
                _targetText.gameObject.SetActive(false);
            });
        }
    }
}