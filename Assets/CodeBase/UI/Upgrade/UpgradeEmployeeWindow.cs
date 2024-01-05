using CodeBase.Data;
using CodeBase.Extensions;
using CodeBase.Gameplay.Wallet;
using CodeBase.Services.Employee;
using CodeBase.UI.Buttons;
using TMPro;
using UnityEngine;
using Zenject;

namespace CodeBase.UI.UpgradeEmployee
{
    public class UpgradeEmployeeWindow : WindowBase
    {
        [SerializeField] private TMP_Text _qualificationTypeText;
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _profitText;
        [SerializeField] private TMP_Text _salaryText;
        [SerializeField] private UpgradeEmployeeButton _upgradeEmployeeButton;
        [SerializeField] private CanvasAnimator _canvasAnimator;

        private Gameplay.EmployeeSystem.Employee _employee;
        private EmployeeDataService _employeeDataService;
        private WalletService _walletService;

        [Inject]
        private void Construct(EmployeeDataService employeeDataService, WalletService walletService)
        {
            _walletService = walletService;
            _employeeDataService = employeeDataService;
        }

        public override void Open()
        {
            gameObject.SetActive(true);
            _canvasAnimator.FadeInCanvas();
        }

        public override void Close() =>
            _canvasAnimator.FadeOutCanvas(base.Close);

        public void Init(Gameplay.EmployeeSystem.Employee employee)
        {
            _employee = employee;

            UpgradeEmployeeData targetUpgradeEmployee = _employeeDataService.GetUpgradeEmployeeData(employee.Id);
            InitUpgradeButton(targetUpgradeEmployee);

            _profitText.text = $"Profit: {_employee.Profit}";
            _qualificationTypeText.text = $"Qualification type: {_employee.QualificationType}";
            _nameText.text = $"Name: {_employee.Name}";
            _salaryText.text = $"Salary: {_employee.Salary}";
        }

        private void InitUpgradeButton(UpgradeEmployeeData targetUpgradeEmployee)
        {
            if (!_walletService.HasEnough(ItemTypeId.Money, targetUpgradeEmployee.UpgradeCost))
                _upgradeEmployeeButton.interactable = false;
            
            _upgradeEmployeeButton.SetEmployeeData(_employee.ToEmployeeData());
            _upgradeEmployeeButton.SetUpgradeCost(targetUpgradeEmployee.UpgradeCost);

        }
    }
}