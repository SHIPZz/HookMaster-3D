using System;
using DG.Tweening;
using UnityEngine;

namespace CodeBase.Animations
{
    [RequireComponent(typeof(RectTransform))]
    public class RectTransformLoopAnim : MonoBehaviour
    {
        [SerializeField] private Vector2 _targetAnchoredPosition;
        [SerializeField] private Ease _ease;
        [SerializeField] private float _targetTime = 1f;
        [SerializeField] private float _startTime = 0.5f;
        [SerializeField] private Ease _returnEase = Ease.OutQuad;
        
        private RectTransform _rectTransform;
        private Vector2 _initialAnchoredPosition;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _initialAnchoredPosition = _rectTransform.anchoredPosition;

            _rectTransform.DOAnchorPos(_targetAnchoredPosition, _targetTime)
                .OnComplete(() => _rectTransform.DOAnchorPos(_initialAnchoredPosition, _startTime)
                    .SetEase(_returnEase))
                .SetEase(_ease)
                .SetLoops(-1, LoopType.Yoyo);
        }
    }
}