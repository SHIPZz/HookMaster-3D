using CodeBase.Services.Factories.UI;
using CodeBase.Services.GOPool;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

namespace CodeBase.Services.UI
{
    public class FloatingTextService
    {
        private Vector2 _initialTextAnchoredPosition;
        private TMP_Text _targetText;
        private readonly ObjectPool<TMP_Text, string, Transform> _textObjectPool;

        public FloatingTextService(UIFactory uiFactory)
        {
            _textObjectPool = new ObjectPool<TMP_Text, string, Transform>(uiFactory.CreateElement<TMP_Text>, 10);
        }

        public void ShowFloatingText(string text, float targetAnchoredPositionY,
            float duration,
            float fadeInDuration,
            float fadeOutDuration,
            Quaternion rotation,
            string path, Transform target)
        {
            _targetText = _textObjectPool.Pop(path, target);
            
            ConfigureText(rotation, text);
            var rectTransformAnimator = _targetText.GetComponent<RectTransformAnimator>();
            ConfigureRectTransformAnimator(rectTransformAnimator, fadeInDuration);

            rectTransformAnimator.MoveAnchoredPositionY(targetAnchoredPositionY, duration,
                () => HandleFadeOut(rectTransformAnimator, fadeOutDuration));
        }

        private void ConfigureText( Quaternion rotation, string text)
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

        private void HandleFadeOut(RectTransformAnimator rectTransformAnimator, float fadeOutDuration)
        {
            rectTransformAnimator.FadeText(_targetText, 0f, fadeOutDuration, () =>
            {
                rectTransformAnimator.SetInitialPosition();
                _textObjectPool.Push(_targetText);
                _targetText.gameObject.SetActive(false);
            });
        }
    }
}