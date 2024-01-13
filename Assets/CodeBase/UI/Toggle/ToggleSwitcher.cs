using CodeBase.Enums;
using CodeBase.Services.Sound;
using UnityEngine;
using Zenject;

namespace CodeBase.UI.Toggle
{
    [RequireComponent(typeof(UnityEngine.UI.Toggle),typeof(ToggleAnimation))]
    public class ToggleSwitcher : MonoBehaviour
    {
        [SerializeField] private ToggleTypeId _toggleTypeId;

        private UnityEngine.UI.Toggle _toggle;
        private SettingsService _settingsService;
        private ToggleAnimation _toggleAnimation;

        [Inject]
        private void Construct(SettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        private void Awake()
        {
            _toggle = GetComponent<UnityEngine.UI.Toggle>();
            _toggleAnimation = GetComponent<ToggleAnimation>();
        }

        private void Start()
        {
            _toggle.isOn = _settingsService.GetTargetToggleValue(_toggleTypeId);
            _toggleAnimation.Initialize(_toggle.isOn);
        }

        private void OnEnable() =>
            _toggle.onValueChanged.AddListener(OnValueChanged);

        private void OnDisable() =>
            _toggle.onValueChanged.RemoveListener(OnValueChanged);

        private void OnValueChanged(bool isOn)
        {
            _settingsService.SetToggleSetting(_toggle.isOn, _toggleTypeId);
            _toggleAnimation.MoveHandleWithAnim(isOn);
        }
    }
}
