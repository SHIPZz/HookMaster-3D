using System;
using CodeBase.Services.GOPool;
using CodeBase.Services.Providers.Camera;
using CodeBase.Services.TransformCameraFace;
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

        [field: SerializeField] public RectTransform RectTransform { get; private set; }

        [SerializeField] private float _duration = 0.5f;
        [SerializeField] private float _anchoredPosition = 20;
        [SerializeField] private float _fadeInDuration = 0.1f;
        [SerializeField] private float _fadeOutDuration = 0.5f;
        [SerializeField] private float _minRandomAnchoredPosition;
        [SerializeField] private float _maxRandomAnchoredPosition;

        private Vector3 _initialPosition;
        private TransformCameraFacing _transformCameraFacing;

        private void Awake()
        {
            _initialPosition = transform.position;
            _transformCameraFacing = GetComponent<TransformCameraFacing>();
        }

        public void SetDuration(float duration) => _duration = duration;

        public void SetAnchoredPos(float pos) => _anchoredPosition = pos;

        public void SetMinAnchoredPos(float pos) => _minRandomAnchoredPosition = pos;

        public void SetRotationToCamera(bool cameraFacing)
        {
            if (cameraFacing)
            {
                _transformCameraFacing.enabled = true;
                return;
            }

            _transformCameraFacing.enabled = false;
            transform.rotation = Quaternion.identity;
        }

        public void Init(Vector3 at, EnumObjectPool<FloatingTextView, Transform, FloatingTextType> floatingTextPool,
            FloatingTextService floatingTextService)
        {
            transform.position = _initialPosition;

            float targetAnchoredPosition = _minRandomAnchoredPosition != 0
                ? Random.Range(_minRandomAnchoredPosition, _maxRandomAnchoredPosition)
                : _anchoredPosition;

            floatingTextService.ShowFloatingText(this, RectTransform.anchoredPosition.y + targetAnchoredPosition,
                _duration, _fadeInDuration,
                _fadeOutDuration, Quaternion.identity, at, () => floatingTextPool.Push(this, FloatingTextType));
        }
    }
}