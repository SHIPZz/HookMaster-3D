using CodeBase.Animations;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Toggle
{
    public class ToggleAnimation : MonoBehaviour
    {
        [SerializeField] private RectTransformAnimator _rectTransformAnimator;
        [SerializeField] private Vector2 _offPosition;
        [SerializeField] private Vector2 _initialPosition;
        [SerializeField] private float _onDuration = 0.5f;
        [SerializeField] private float _offDuration = 0.3f;
        [SerializeField] private UnityEngine.UI.Toggle _toggle;
        [SerializeField] private Sprite _offImage;
        [SerializeField] private Sprite _onImage;

        public void Initialize(bool isOn)
        {
            MoveHandle(isOn ? _initialPosition : _offPosition, 0);
        }

        public void MoveHandleWithAnim(bool isOn)
        {
            MoveHandle(isOn ? _initialPosition : _offPosition, isOn ? _onDuration : _offDuration);
            _toggle.image.sprite = isOn ? _onImage : _offImage;
        }

        private void MoveHandle(Vector2 to, float duration)
        {
            _rectTransformAnimator.MoveRectTransform(to, duration);
            
        }
    }
}