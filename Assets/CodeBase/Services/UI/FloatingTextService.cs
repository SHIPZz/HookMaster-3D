using System;
using CodeBase.Animations;
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
        private const int TextCount = 10;
        private readonly ObjectPool<TMP_Text, string, Transform> _textObjectPool;
        private readonly EnumObjectPool<FloatingTextView, Transform, FloatingTextType> _enumTextPool;

        private Vector2 _initialTextAnchoredPosition;
        private TMP_Text _targetText;

        public FloatingTextService(UIFactory uiFactory)
        {
            _textObjectPool = new ObjectPool<TMP_Text, string, Transform>(uiFactory.CreateElement<TMP_Text>, TextCount);
            _enumTextPool = new EnumObjectPool<FloatingTextView, Transform, FloatingTextType>(
                uiFactory.CreateFloatingTextView, TextCount);
        }

        public void Fade(float fade, float duration)
        {
            if (_targetText == null)
                return;

            var rectTransformAnimator = _targetText.GetComponent<RectTransformAnimator>();
            rectTransformAnimator.FadeText(_targetText, fade, duration);
        }

        public void ShowFloatingText(FloatingTextType floatingTextType, Transform parent, Vector3 position)
        {
            FloatingTextView targetFloatingTextView = _enumTextPool.Pop(parent, floatingTextType);
            _targetText = targetFloatingTextView.Text;
            targetFloatingTextView.Init(position, _enumTextPool, this);
        }

        public void ShowFloatingText(FloatingTextType floatingTextType, Transform parent, Vector3 position, string text)
        {
            FloatingTextView targetFloatingTextView = _enumTextPool.Pop(parent, floatingTextType);
            targetFloatingTextView.Text.text = text;
            _targetText = targetFloatingTextView.Text;
            targetFloatingTextView.Init(position, _enumTextPool, this);
        }

        public void ShowFloatingText(FloatingTextType floatingTextType, Transform parent, Vector3 position, string text,
            float anchoredPos, float duration)
        {
            FloatingTextView targetFloatingTextView = _enumTextPool.Pop(parent, floatingTextType);
            targetFloatingTextView.Text.text = text;
            targetFloatingTextView.SetDuration(duration);
            targetFloatingTextView.SetAnchoredPos(anchoredPos);
            _targetText = targetFloatingTextView.Text;
            targetFloatingTextView.Init(position, _enumTextPool, this);
        }

        public void ShowFloatingText(FloatingTextView floatingTextView, float targetAnchoredPositionY,
            float duration,
            float fadeInDuration,
            float fadeOutDuration,
            Quaternion rotation, Vector3 position,
            Action onComplete = null)
        {
            _targetText = floatingTextView.Text;

            _targetText.transform.position = position;
            ConfigureText(rotation, _targetText.text);
            var rectTransformAnimator = _targetText.GetComponent<RectTransformAnimator>();
            ConfigureRectTransformAnimator(rectTransformAnimator, fadeInDuration);

            rectTransformAnimator.MoveAnchoredPositionY(targetAnchoredPositionY, duration,
                () => { HandleFadeOut(rectTransformAnimator, _targetText, fadeOutDuration, onComplete); });
        }

        public void ShowFloatingText(float targetAnchoredPositionY,
            float duration,
            float fadeInDuration,
            float fadeOutDuration,
            Quaternion rotation,
            string path, Transform parent, Vector3 position)
        {
            _targetText = _textObjectPool.Pop(path, parent);

            _targetText.transform.position = position;
            ConfigureText(rotation, _targetText.text);
            var rectTransformAnimator = _targetText.GetComponent<RectTransformAnimator>();
            ConfigureRectTransformAnimator(rectTransformAnimator, fadeInDuration);

            rectTransformAnimator.MoveAnchoredPositionY(targetAnchoredPositionY, duration,
                () => { HandleFadeOut(rectTransformAnimator, _targetText, fadeOutDuration); });
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
                () => HandleFadeOut(rectTransformAnimator, _targetText, fadeOutDuration));
        }

        private void ConfigureText(Quaternion rotation, string text)
        {
            _targetText.text = text;

            _targetText.gameObject.SetActive(true);

            if (rotation != Quaternion.identity)
                _targetText.rectTransform.rotation = rotation;
        }

        private void ConfigureRectTransformAnimator(RectTransformAnimator rectTransformAnimator, float fadeInDuration)
        {
            rectTransformAnimator.SetInitialPosition();
            rectTransformAnimator.SetRotation(rectTransformAnimator.transform.rotation);

            rectTransformAnimator.FadeText(_targetText, 1f, fadeInDuration);
        }

        private void HandleFadeOut(RectTransformAnimator rectTransformAnimator, TMP_Text text, float fadeOutDuration,
            Action onComplete = null)
        {
            rectTransformAnimator.FadeText(_targetText, 0f, fadeOutDuration, () =>
            {
                rectTransformAnimator.SetInitialPosition();
                onComplete?.Invoke();
            });
        }
    }
}