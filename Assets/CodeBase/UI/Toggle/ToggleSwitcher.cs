using CodeBase.Enums;
using CodeBase.Services.Sound;
using DG.Tweening;
using UnityEngine;
using Zenject;

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
        [SerializeField] private ToggleTypeId _toggleTypeId;

        private UnityEngine.UI.Toggle _toggle;
        private Tween _tween;
        private SettingsService _settingsService;

        [Inject]
        private void Construct(SettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        private void Awake() => 
            _toggle = GetComponent<UnityEngine.UI.Toggle>();

        private void Start()
        {
            _toggle.isOn = _settingsService.GetTargetToggleValue(_toggleTypeId);
            MoveHandleWithAnim(_toggle.isOn, false);
        }

        private void OnEnable() =>
            _toggle.onValueChanged.AddListener(OnValueChanged);

        private void OnDisable() =>
            _toggle.onValueChanged.RemoveListener(OnValueChanged);

        private void OnValueChanged(bool isOn)
        {
            _tween?.Kill(true);
            _settingsService.SetToggleSetting(_toggle.isOn, _toggleTypeId);

            MoveHandleWithAnim(isOn, true);
        }

        private void MoveHandleWithAnim(bool isOn, bool withAnim)
        {
            if (!withAnim)
            {
                MoveHandle(isOn, 0f);
                return;
            }

            float duration = isOn ? _onDuration : _offDuration;
            Vector3 targetPosition = isOn ? _initialPosition : _offPosition;

            _rectTransformAnimator.MoveRectTransform(targetPosition, duration);
        }

        private void MoveHandle(bool isOn, float duration)
        {
            Vector3 targetPosition = isOn ? _initialPosition : _offPosition;
            _rectTransformAnimator.MoveRectTransform(targetPosition, duration);
        }
    }
}