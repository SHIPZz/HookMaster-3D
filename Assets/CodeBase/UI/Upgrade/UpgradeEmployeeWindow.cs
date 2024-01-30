using Abu;
using CodeBase.Animations;
using CodeBase.Data;
using CodeBase.Extensions;
using CodeBase.Services.Employees;
using CodeBase.UI.Buttons;
using TMPro;
using UnityEngine;
using Zenject;

namespace CodeBase.UI.Upgrade
{
    public class UpgradeEmployeeWindow : WindowBase
    {
        public Transform TutorialHandParent;
        public TutorialFadeImage TutorialFadeImage;

        [SerializeField] private TMP_Text _qualificationTypeText;
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _profitText;
        [SerializeField] private TMP_Text _salaryText;
        [SerializeField] private UpgradeEmployeeButton _upgradeEmployeeButton;
        [SerializeField] private CanvasAnimator _canvasAnimator;

        private Gameplay.Employees.Employee _employee;
        private EmployeeDataService _employeeDataService;

        public UpgradeEmployeeButton UpgradeEmployeeButton => _upgradeEmployeeButton;

        [Inject]
        private void Construct(EmployeeDataService employeeDataService)
        {
            _employeeDataService = employeeDataService;
        }

        public override void Open()
        {
            gameObject.SetActive(true);
            _canvasAnimator.FadeInCanvas();
        }

        public override void Close() =>
            _canvasAnimator.FadeOutCanvas(base.Close);

        public void Init(Gameplay.Employees.Employee employee)
        {
            _employee = employee;

            UpgradeEmployeeData targetUpgradeEmployee = _employeeDataService.GetUpgradeEmployeeData(employee.Id);
            InitUpgradeButton(targetUpgradeEmployee);

            _profitText.text = $"Profit: {_employee.Profit}";
            _qualificationTypeText.text = $"Qualification Level: {_employee.QualificationType}";
            _nameText.text = $"Name: {_employee.Name}";
            _salaryText.text = $"Salary: {_employee.Salary}";
        }

        private void InitUpgradeButton(UpgradeEmployeeData targetUpgradeEmployee)
        {
            _upgradeEmployeeButton.SetEmployeeData(_employee.ToEmployeeData());
            _upgradeEmployeeButton.SetUpgradeCost(targetUpgradeEmployee.UpgradeCost);
        }
    }
}