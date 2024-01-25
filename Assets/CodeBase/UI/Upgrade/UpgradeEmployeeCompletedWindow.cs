using CodeBase.Animations;
using CodeBase.Data;
using CodeBase.Gameplay.SoundPlayer;
using CodeBase.Services.Employees;
using CodeBase.Services.Window;
using CodeBase.UI.Hud;
using CodeBase.UI.SkipProgress;
using TMPro;
using UnityEngine;
using Zenject;

namespace CodeBase.UI.Upgrade
{
    public class UpgradeEmployeeCompletedWindow : WindowBase
    {
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private CanvasAnimator _canvasAnimator;
        
        [Header("Old data")] 
        [SerializeField] private TMP_Text _oldProfitText;
        [SerializeField] private TMP_Text _oldSalaryText;
        [SerializeField] private TMP_Text _oldQualificationTypeText;

        [Header("Upgraded data")] 
        [SerializeField] private TMP_Text _newProfitText;
        [SerializeField] private TMP_Text _newSalaryText;
        [SerializeField] private TMP_Text _newQualificationTypeText;
        
        [SerializeField] private RectTransformScaleAnim _buttonScaleAnim;
        [SerializeField] private AudioSource _increaseValueSound;
        [SerializeField] private AppearanceEffect _appearanceEffect;
        [SerializeField] private SoundPlayerSystem _soundPlayerSystem;

        private EmployeeData _employeeData;

        private EmployeeService _employeeService;
        private NumberTextAnimService _numberTextAnimService;
        private WindowService _windowService;
        private EmployeeDataService _employeeDataService;

        [Inject]
        private void Construct(EmployeeService employeeService, NumberTextAnimService numberTextAnimService, WindowService windowService, EmployeeDataService employeeDataService)
        {
            _employeeDataService = employeeDataService;
            _windowService = windowService;
            _numberTextAnimService = numberTextAnimService;
            _employeeService = employeeService;
        }

        public override void Open()
        {
            _windowService.Close<HudWindow>();
            _appearanceEffect.PlayTargetEffects();
            _soundPlayerSystem.PlayActiveSound();
            _canvasAnimator.FadeInCanvas();

            _employeeService.Upgrade(_employeeData, OnComplete);
        }

        private async void OnComplete(EmployeeData newEmployeeData)
        {
            await _numberTextAnimService.AnimateNumber(0, newEmployeeData.QualificationType, 0.5f,
                _newQualificationTypeText, _increaseValueSound);
            await _numberTextAnimService.AnimateNumber(0, newEmployeeData.Salary, 1.5f, _newSalaryText, '$', _increaseValueSound);
            await _numberTextAnimService.AnimateNumber(0, newEmployeeData.Profit, 1.5f, _newProfitText, '$', _increaseValueSound);
            
            _buttonScaleAnim.ToScale();
        }

        public override void Close()
        {
            _windowService.Open<HudWindow>();
            _windowService.Close<SkipProgressSliderWindow>();
            _employeeDataService.UpgradeEmployeeData(_employeeData);
            _canvasAnimator.FadeOutCanvas(base.Close);
        }

        public void Init(EmployeeData employeeData)
        {
            _employeeData = employeeData;
            _oldProfitText.text = $"{_employeeData.Profit}$";
            _oldSalaryText.text = $"{_employeeData.Salary}$";
            _oldQualificationTypeText.text = $"{_employeeData.QualificationType}";
            _nameText.text = $"{_employeeData.Name}";
        }
    }
}