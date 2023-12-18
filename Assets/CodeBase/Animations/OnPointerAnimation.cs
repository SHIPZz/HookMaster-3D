using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CodeBase.Animations
{
    [RequireComponent(typeof(RectTransform))]
    public class OnPointerAnimation : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private Vector2 _targetAnchoredScale;
        [SerializeField] private float _animationDuration = 0.2f;

        private RectTransform _targetRectTransform;
        private Tween _tween;
        private Vector2 _initialAnchoredScale;

        private void Awake()
        {
            _targetRectTransform = GetComponent<RectTransform>();
            _initialAnchoredScale = _targetRectTransform.sizeDelta;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _tween?.Kill(true);
            _tween = _targetRectTransform.DOSizeDelta(_targetAnchoredScale, _animationDuration);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _tween?.Kill(true);
            _tween = _targetRectTransform.DOSizeDelta(_initialAnchoredScale, _animationDuration);
        }
    }
}