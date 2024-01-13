using System;
using CodeBase.Services.GOPool;
using CodeBase.Services.Providers.Camera;
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
        
        private Vector3 _initialPosition;
        private CameraProvider _cameraProvider;

        [Inject]
        private void Construct(CameraProvider cameraProvider)
        {
            _cameraProvider = cameraProvider;
        }
        
        private void Awake() => 
            _initialPosition = transform.position; 

        public void Init(Vector3 at, EnumObjectPool<FloatingTextView, Transform, FloatingTextType> floatingTextPool, FloatingTextService floatingTextService)
        {
            transform.position = _initialPosition;

            float targetAnchoredPosition = 0;

            targetAnchoredPosition = _minRandomAnchoredPosition != 0 ? Random.Range(_minRandomAnchoredPosition, _maxRandomAnchoredPosition) : _anchoredPosition;
            
            floatingTextService.ShowFloatingText(this, targetAnchoredPosition, _duration, _fadeInDuration,
                _fadeOutDuration, Quaternion.LookRotation(_cameraProvider.Camera.transform.forward),  at, () => floatingTextPool.Push(this, FloatingTextType));
        }
        
        public void Init(Vector3 at, EnumObjectPool<FloatingTextView, Transform, FloatingTextType> floatingTextPool, 
            FloatingTextService floatingTextService,float targetPositionY, float duration)
        {
            transform.position = _initialPosition;

            float targetAnchoredPosition = 0;

            targetAnchoredPosition = _minRandomAnchoredPosition != 0 ? Random.Range(_minRandomAnchoredPosition, _maxRandomAnchoredPosition) : _anchoredPosition;
            
            floatingTextService.ShowFloatingText(this, targetPositionY, duration, _fadeInDuration,
                _fadeOutDuration, Quaternion.LookRotation(_cameraProvider.Camera.transform.forward),  at, () => floatingTextPool.Push(this, FloatingTextType));
        }
    }
}