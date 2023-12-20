using DG.Tweening;
using UnityEngine;

namespace CodeBase.UI.Toggle
{
    [RequireComponent(typeof(UnityEngine.UI.Toggle))]
    public class ToggleSwitcher : MonoBehaviour
    {
        [SerializeField] private Vector2 _offPosition;
        [SerializeField] private Vector2 _initialPosition;
        [SerializeField] private RectTransformAnimator _rectTransformAnimator;
        [SerializeField] private float _onDuration = 0.5f;
        [SerializeField] private float _offDuration = 0.3f;

        private UnityEngine.UI.Toggle _toggle;
        private Tween _tween;

        private void Awake() => 
            _toggle = GetComponent<UnityEngine.UI.Toggle>();

        private void OnEnable() => 
            _toggle.onValueChanged.AddListener(OnValueChanged);

        private void OnDisable() => 
            _toggle.onValueChanged.RemoveListener(OnValueChanged);

        private void OnValueChanged(bool isOn)
        {
            _tween?.Kill(true);

            if (isOn)
            {
                _rectTransformAnimator.MoveRectTransform(_initialPosition, _onDuration);
                return;
            }

            _rectTransformAnimator.MoveRectTransform(_offPosition, _offDuration);
        }
    }
}