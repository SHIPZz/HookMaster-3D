using System;
using DG.Tweening;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

namespace CodeBase.Animations
{
    public class RectTransformAnimator : MonoBehaviour
    {
        [SerializeField] private RectTransform _targetRectTransform;

        private Tween _movePositionTween;
        private Vector2 _initialAnchoredPosition;
        private Tween _fadeTween;

        private void Awake()
        {
            _initialAnchoredPosition = _targetRectTransform.anchoredPosition;
        }

        public Tween MoveAnchoredPositionY(float positionY, float duration, [CanBeNull] Action onCompleted = null)
        {
            _movePositionTween?.Kill(true);

            _movePositionTween = _targetRectTransform.DOAnchorPosY(_initialAnchoredPosition.y + positionY, duration)
                .OnComplete(() => onCompleted?.Invoke());

            return _movePositionTween;
        }

        public void MoveRectTransform(Vector2 targetPosition, float duration)
        {
            _movePositionTween?.Kill(true);

            _movePositionTween = _targetRectTransform.DOAnchorPos(targetPosition, duration);
        }

        public Tween FadeText(TMP_Text text, float targetAlpha, float duration, [CanBeNull] Action onCompleted = null)
        {
            _fadeTween?.Kill(true);
            _fadeTween = text.DOFade(targetAlpha, duration)
                .OnComplete(() => onCompleted?.Invoke());

            return _fadeTween;
        }

        public void SetInitialPosition()
        {
            _targetRectTransform.anchoredPosition = _initialAnchoredPosition;
        }

        public void SetRotation(Quaternion rotation)
        {
            _targetRectTransform.rotation = rotation;
        }
    }
}