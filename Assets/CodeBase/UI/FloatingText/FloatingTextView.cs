using System;
using CodeBase.Services.GOPool;
using CodeBase.Services.TransformCameraFace;
using CodeBase.Services.UI;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CodeBase.UI.FloatingText
{
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(TransformCameraFacing))]
    public class FloatingTextView : MonoBehaviour
    {
        [field: SerializeField] public FloatingTextType FloatingTextType { get; private set; }
        [field: SerializeField] public TMP_Text Text { get; private set; }

        [field: SerializeField] public RectTransform RectTransform { get; private set; }

        [SerializeField] private float _duration = 0.5f;
        [SerializeField] private float _fadeInDuration = 0.1f;
        [SerializeField] private float _fadeOutDuration = 0.5f;
        [SerializeField] private float _minRandomAnchoredPosition;
        [SerializeField] private float _maxRandomAnchoredPosition;

        private Vector3 _initialPosition;
        private TransformCameraFacing _transformCameraFacing;
        private CanvasGroup _canvasGroup;
        private EnumObjectPool<FloatingTextView, Transform, FloatingTextType> _floatingTextPool;

        public CanvasGroup CanvasGroup => _canvasGroup;

        private void Awake()
        {
            _initialPosition = transform.position;
            _transformCameraFacing = GetComponent<TransformCameraFacing>();
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        private void OnDisable() => 
            _floatingTextPool?.Push(this, FloatingTextType);

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

        public void Init(Vector3 at, EnumObjectPool<FloatingTextView, Transform, FloatingTextType> floatingTextPool)
        {
            _floatingTextPool = floatingTextPool;
            transform.position = _initialPosition;

            FadeIn();

            float targetAnchoredPosition = Random.Range(_minRandomAnchoredPosition, _maxRandomAnchoredPosition);

            RectTransform.transform.position = at;

            RectTransform.DOAnchorPosY(targetAnchoredPosition, _duration)
                .SetUpdate(true)
                .OnComplete(FadeOut);
        }

        private void FadeIn()
        {
            gameObject.SetActive(true);
            _canvasGroup.DOFade(1, _fadeInDuration)
                .SetUpdate(true);
        }

        private void FadeOut() => _canvasGroup.DOFade(0, _fadeOutDuration)
            .OnComplete(() => gameObject.SetActive(false))
            .SetUpdate(true);
    }
}