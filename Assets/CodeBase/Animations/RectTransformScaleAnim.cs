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

        public void ToScale()
        {
            SetTween(Vector3.one * _targetScale, _scaleDuration);
        }

        public void UnScale()
        {
            SetTween(Vector3.one * _unScale, _unScaleDuration);
        }

        private void SetTween(Vector3 scale, float duration)
        {
            _tween?.Kill(true);

            switch (true)
            {
                case bool _ when _scaleX:
                    _tween = _rectTransform.DOScaleX(scale.x, duration);
                    break;
                case bool _ when _scaleY:
                    _tween = _rectTransform.DOScaleY(scale.y, duration);
                    break;
                case bool _ when _scaleZ:
                    _tween = _rectTransform.DOScaleZ(scale.z, duration);
                    break;
                default:
                    _tween = _rectTransform.DOScale(scale, duration);
                    break;
            }
        }
    }
}