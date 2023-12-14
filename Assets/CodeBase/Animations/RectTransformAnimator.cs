using System;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;

public class RectTransformAnimator : MonoBehaviour
{
    [SerializeField] private RectTransform _targetRectTransform;
    
    private Tween _movePositionTween;
    private Vector2 _initialAnchoredPosition;

    private void Awake()
    {
        _initialAnchoredPosition = _targetRectTransform.anchoredPosition;
    }

    public void MoveAnchoredPositionY(float positionY, float duration, [CanBeNull] Action onCompleted = null)
    {
        _movePositionTween?.Kill(true);

        _targetRectTransform.DOAnchorPosY( _initialAnchoredPosition.y + positionY, duration)
            .OnComplete(() => onCompleted?.Invoke());
    }

    public void MoveRectTransform(Vector2 targetPosition, float duration)
    {
        _movePositionTween?.Kill(true);

        _movePositionTween = _targetRectTransform.DOAnchorPos(targetPosition, duration);
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