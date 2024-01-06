using System;
using CodeBase.Constant;
using CodeBase.Services.GOPool;
using CodeBase.Services.UI;
using TMPro;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace CodeBase.UI.FloatingText
{
    public class FloatingTextView : MonoBehaviour
    {
        [field: SerializeField] public FloatingTextType FloatingTextType { get; private set; }
        [field: SerializeField] public TMP_Text Text { get; private set; }
        
        [SerializeField] private float _duration = 0.5f;
        [SerializeField] private float _anchoredPosition = 20;
        [SerializeField] private float _fadeInDuration = 0.1f;
        [SerializeField] private float _fadeOutDuration = 0.5f;
        [SerializeField] private float _minRandomAnchoredPosition;
        [SerializeField] private float _maxRandomAnchoredPosition;
        
        private FloatingTextService _floatingTextService;
        private EnumObjectPool<FloatingTextView, Transform, FloatingTextType> _floatingTextPool;
        private Vector3 _initialPosition;
        
        [Inject]
        private void Construct(FloatingTextService floatingTextService) => 
            _floatingTextService = floatingTextService;

        private void Awake() => 
            _initialPosition = transform.position;

        public void Init(Vector3 at, EnumObjectPool<FloatingTextView, Transform, FloatingTextType> floatingTextPool)
        {
            transform.position = _initialPosition;
            _floatingTextPool = floatingTextPool;

            float targetAnchoredPosition = 0;

            targetAnchoredPosition = _minRandomAnchoredPosition != 0 ? Random.Range(_minRandomAnchoredPosition, _maxRandomAnchoredPosition) : _anchoredPosition;
            
            _floatingTextService.ShowFloatingText(this, targetAnchoredPosition, _duration, _fadeInDuration,
                _fadeOutDuration, Quaternion.identity,  at, () => _floatingTextPool.Push(this, FloatingTextType));
        }
    }
}