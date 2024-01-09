using System;
using System.Globalization;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace CodeBase.Animations
{
    [RequireComponent(typeof(TMP_Text))]
    public class TextAnimView : MonoBehaviour
    {
        [SerializeField] private float _targetScale = 2f;
        [SerializeField] private float _increaseScaleDuration = 0.5f;
        [SerializeField] private float _decreaseScaleDuration = 0.1f;
        [SerializeField] private float _defaultScale = 1f;
        [SerializeField] private float _colorFadeOutDuration = 1f;
        [SerializeField] private float _colorFadeDuration = 0.5f;
        
        private Tween _tween;
        private Tween _scaleTween;
        private TMP_Text _text;

        private void Awake() => 
            _text = GetComponent<TMP_Text>();

        public void SetText(int value) => 
            _text.text = value.ToString(CultureInfo.InvariantCulture);

        public void SetText(string text) => 
            _text.text = text;
        
        public void SetColor(Color color) => 
            _text.color = color;

        public void DoFadeInColor(Color color, Action onComplete = null)
        {
            _tween?.Kill(true);
            _tween = _text.DOColor(color, _colorFadeDuration).OnComplete(() => onComplete?.Invoke());
        }
        
        public void DoFadeOutColor(Color color,Action onComplete = null)
        {
            _tween?.Kill(true);
            _tween = _text.DOColor(color, _colorFadeOutDuration).OnComplete(() => onComplete?.Invoke());
        }

        public void DoScale(Action onComplete = null)
        {
            _scaleTween?.Kill(true);
            _scaleTween = _text.rectTransform.DOScale(_targetScale, _increaseScaleDuration).OnComplete(() => onComplete?.Invoke());
        }

        public void ResetScale(Action onComplete = null)
        {
            _scaleTween?.Kill(true);
            _scaleTween = _text.rectTransform.DOScale(_defaultScale, _decreaseScaleDuration).OnComplete(() => onComplete?.Invoke());
        }
        
    }
}