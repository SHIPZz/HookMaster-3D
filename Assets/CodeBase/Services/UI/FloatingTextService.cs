using CodeBase.Services.Factories.UI;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace CodeBase.Services.UI
{
    public class FloatingTextService
    {
        private Vector2 _initialTextAnchoredPosition;
        private readonly UIFactory _uiFactory;
        private TMP_Text _targetText;

        public FloatingTextService(UIFactory uiFactory)
        {
            _uiFactory = uiFactory;
        }

        public void ShowFloatingText(string text, float additionalAnchoredPositionY,
            float duration,
            float fadeInDuration,
            float fadeOutDuration,
            Quaternion rotation,
            string path, Transform target)
        {
            if (_targetText == null)
                _targetText = _uiFactory.CreateElement<TMP_Text>(path, target);

            ConfigureText(rotation, text);
            var rectTransformAnimator = _targetText.GetComponent<RectTransformAnimator>();
            ConfigureRectTransformAnimator(rectTransformAnimator, fadeInDuration);

            rectTransformAnimator.MoveAnchoredPositionY(additionalAnchoredPositionY, duration,
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
                _targetText.gameObject.SetActive(false);
            });
        }
    }
}