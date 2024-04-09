using Abu;
using CodeBase.Animations;
using CodeBase.Data;
using CodeBase.Extensions;
using CodeBase.Services.Employees;
using CodeBase.Services.Window;
using CodeBase.UI.Buttons;
using CodeBase.UI.Hud;
using TMPro;
using UnityEngine;
using Zenject;

namespace CodeBase.UI.Upgrade
{
    public class UpgradeEmployeeWindow : WindowBase
    {
        public Transform TutorialHandParent;
        public TutorialFadeImage TutorialFadeImage;

        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private UpgradeEmployeeButton _upgradeEmployeeButton;
        [SerializeField] private CanvasAnimator _canvasAnimator;

        private Gameplay.Employees.Employee _employee;
        private EmployeeDataService _employeeDataService;
        private WindowService _windowService;

        public UpgradeEmployeeButton UpgradeEmployeeButton => _upgradeEmployeeButton;

        [Inject]
        private void Construct(EmployeeDataService employeeDataService, WindowService windowService)
        {
            _windowService = windowService;
            _employeeDataService = employeeDataService;
        }

        public override void Open()
        {
            gameObject.SetActive(true);
            _windowService.Close<HudWindow>();
            _canvasAnimator.FadeInCanvas();
        }

        public override void Close()
        {
            _canvasAnimator.FadeOutCanvas(base.Close);
            _windowService.Open<HudWindow>();
        }

        public void Init(Gameplay.Employees.Employee employee)
        {
            _employee = employee;

            UpgradeEmployeeData targetUpgradeEmployee = _employeeDataService.GetUpgradeEmployeeData(employee.Id);
            InitUpgradeButton(targetUpgradeEmployee);

            _nameText.text = $"{_employee.Name}";
        }

        private void InitUpgradeButton(UpgradeEmployeeData targetUpgradeEmployee)
        {
            _upgradeEmployeeButton.SetEmployeeData(_employee.ToEmployeeData());
            _upgradeEmployeeButton.SetUpgradeCost(targetUpgradeEmployee.UpgradeCost);
        }
    }
}