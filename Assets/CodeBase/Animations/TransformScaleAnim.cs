using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace CodeBase.Animations
{
    public class TransformScaleAnim : MonoBehaviour
    {
        [SerializeField] private float _targetScale = 1f;
        [SerializeField] private float _unScale;
        [SerializeField] private float _scaleDuration = 0.2f;
        [SerializeField] private float _unScaleDuration = 0.1f;
        [SerializeField] private Transform _transform;
        [SerializeField] private bool _scaleX;
        [SerializeField] private bool _scaleY;
        [SerializeField] private bool _scaleZ;
        [SerializeField] private bool _unscaleOnStart;
        [SerializeField] private float _scaleDelay = 1f;

        private Tween _tween;

        private void Awake()
        {
            if(_unscaleOnStart)
                UnScale();
        }

        public void ToScale(Action onComplete = null)
        {
            _transform.gameObject.SetActive(true);
            SetTween(Vector3.one * _targetScale, _scaleDuration, onComplete);
        }
        
        public async void ToScaleAsync(Action onComplete = null)
        {
            await UniTask.WaitForSeconds(_scaleDelay);
            
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
                    _tween = _transform.DOScaleX(scale.x, duration).OnComplete(()=>onComplete?.Invoke()).SetUpdate(true);
                    break;
                case bool _ when _scaleY:
                    _tween = _transform.DOScaleY(scale.y, duration).OnComplete(()=>onComplete?.Invoke()).SetUpdate(true);
                    break;
                case bool _ when _scaleZ:
                    _tween = _transform.DOScaleZ(scale.z, duration).OnComplete(()=>onComplete?.Invoke()).SetUpdate(true);
                    break;
                default:
                    _tween = _transform.DOScale(scale, duration).OnComplete(()=>onComplete?.Invoke()).SetUpdate(true);
                    break;
            }
        }
    }
}