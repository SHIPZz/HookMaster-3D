using System;
using DG.Tweening;
using UnityEngine;

namespace CodeBase.Animations
{
    public class RectTransformScaleAnim : MonoBehaviour
    {
        [SerializeField] private float _targetScale = 1f;
        [SerializeField] private float _unScale;
        [SerializeField] private float _scaleDuration = 0.2f;
        [SerializeField] private float _unScaleDuration = 0.1f;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private bool _scaleX;
        [SerializeField] private bool _scaleY;
        [SerializeField] private bool _scaleZ;

        private Tween _tween;

        public void ToScale(Action onComplete = null)
        {
            SetTween(Vector3.one * _targetScale, _scaleDuration, onComplete);
        }

        public void UnScale(Action onComplete = null)
        {
            SetTween(Vector3.one * _unScale, _unScaleDuration,onComplete);
        }

        private void SetTween(Vector3 scale, float duration, Action onComplete = null)
        {
            _tween?.Kill(true);

            switch (true)
            {
                case bool _ when _scaleX:
                    _tween = _rectTransform.DOScaleX(scale.x, duration).OnComplete(()=>onComplete?.Invoke());
                    break;
                case bool _ when _scaleY:
                    _tween = _rectTransform.DOScaleY(scale.y, duration).OnComplete(()=>onComplete?.Invoke());
                    break;
                case bool _ when _scaleZ:
                    _tween = _rectTransform.DOScaleZ(scale.z, duration).OnComplete(()=>onComplete?.Invoke());
                    break;
                default:
                    _tween = _rectTransform.DOScale(scale, duration).OnComplete(()=>onComplete?.Invoke());
                    break;
            }
        }
    }
}