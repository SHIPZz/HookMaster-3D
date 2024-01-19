using DG.Tweening;
using UnityEngine;

namespace CodeBase.Animations
{
    public class RectTransformWidthLoopAnim : MonoBehaviour
    {
        [SerializeField] private Vector2 _anchored;
        [SerializeField] private Ease _ease;
        [SerializeField] private float _targetTime = 1f;
        [SerializeField] private float _startTime = 0.5f;
        [SerializeField] private Ease _returnEase = Ease.OutQuad;
        
        private RectTransform _rectTransform;
        private Vector2 _initialAnchoredPosition;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _initialAnchoredPosition = new Vector2(_rectTransform.rect.width,_rectTransform.rect.height);

            _rectTransform.DOSizeDelta(_anchored, _targetTime)
                .OnComplete(() => _rectTransform.DOSizeDelta(_initialAnchoredPosition, _startTime)
                    .SetEase(_returnEase))
                .SetEase(_ease)
                .SetLoops(-1, LoopType.Yoyo);
        }
    }
}