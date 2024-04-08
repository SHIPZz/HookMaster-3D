using System.Globalization;
using CodeBase.Animations;
using CodeBase.Data;
using CodeBase.Gameplay.SoundPlayer;
using CodeBase.Services.Employees;
using CodeBase.Services.Window;
using CodeBase.UI.Hud;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using Zenject;

namespace CodeBase.UI.Upgrade
{
    public class UpgradeEmployeeCompletedWindow : WindowBase
    {
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _oldTextSpeed;
        [SerializeField] private TMP_Text _upgradedTextSpeed;

        [SerializeField] private CanvasAnimator _canvasAnimator;
        [SerializeField] private AudioSource _increaseSound;

        [SerializeField] private TransformScaleAnim _buttonScaleAnim;
        [SerializeField] private AppearanceEffect _appearanceEffect;
        [SerializeField] private SoundPlayerSystem _soundPlayerSystem;
        [SerializeField] private float _showGotItButtonDelay =1f;

        private EmployeeData _employeeData;

        private EmployeeService _employeeService;
        private NumberTextAnimService _numberTextAnimService;
        private WindowService _windowService;
        private float _oldProcessPaperSpeed;

        [Inject]
        private void Construct(EmployeeService employeeService, NumberTextAnimService numberTextAnimService,
            WindowService windowService)
        {
            _windowService = windowService;
            _numberTextAnimService = numberTextAnimService;
            _employeeService = employeeService;
        }

        public override void Open()
        {
            _appearanceEffect.PlayTargetEffects();
            _soundPlayerSystem.PlayActiveSound();
            _canvasAnimator.FadeInCanvas();

            _employeeService.Upgrade(_employeeData, OnEmployeeUpgradeComplete);
        }

        [Button]
        private async void OnEmployeeUpgradeComplete(EmployeeData newEmployeeData)
        {
            await _numberTextAnimService.AnimateNumber(0, newEmployeeData.PaperProcessTime, 1.5f, _upgradedTextSpeed);
            await UniTask.WaitForSeconds(_showGotItButtonDelay);
            _buttonScaleAnim.ToScale();
        }

        public override void Close()
        {
            _windowService.Open<HudWindow>();
            _canvasAnimator.FadeOutCanvas(base.Close);
        }

        public void Init(EmployeeData employeeData)
        {
            _oldProcessPaperSpeed = employeeData.PaperProcessTime;
            _employeeData = employeeData;
            _nameText.text = $"{_employeeData.Name}";
            _oldTextSpeed.text = _oldProcessPaperSpeed.ToString(CultureInfo.InvariantCulture);
        }
    }
}