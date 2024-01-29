using CodeBase.Data;
using CodeBase.Gameplay.Employees;
using CodeBase.Gameplay.Wallet;
using CodeBase.Services.Employees;
using CodeBase.Services.UI;
using CodeBase.UI.FloatingText;
using CodeBase.UI.SkipProgress;
using CodeBase.UI.Upgrade;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using Zenject;

namespace CodeBase.UI.Buttons
{
    public class UpgradeEmployeeButton : ButtonOpenerBase
    {
        [SerializeField] private TMP_Text _costText;

        private EmployeeData _employeeData;
        private EmployeeService _employeeService;
        private EmployeeDataService _employeeDataService;
        private WalletService _walletService;
        private int _price;
        private FloatingTextService _floatingTextService;

        [Inject]
        private void Construct(EmployeeDataService employeeDataService, EmployeeService employeeService,
            WalletService walletService, FloatingTextService floatingTextService)
        {
            _floatingTextService = floatingTextService;
            _walletService = walletService;
            _employeeDataService = employeeDataService;
            _employeeService = employeeService;
        }

        public void SetEmployeeData(EmployeeData employeeData) =>
            _employeeData = employeeData;

        public void SetUpgradeCost(int price)
        {
            _price = price;
            _costText.text = $"{price}$";
        }

        protected override void Open()
        {
            if (!_walletService.HasEnough(ItemTypeId.Money, _price))
            {
                _floatingTextService.ShowFloatingText(FloatingTextType.NotEnoughMoney, transform, transform.position, false);
                return;
            }

            UpgradeEmployeeData upgradeEmployeeData = _employeeDataService.GetUpgradeEmployeeData(_employeeData.Id);
            _employeeService.SetUpgrade(_employeeData.Id, true);
            _employeeDataService.RecountUpgradePriceEmployee(upgradeEmployeeData);
            _walletService.Set(ItemTypeId.Money, -_price);

            Gameplay.Employees.Employee targetEmployee = _employeeService.Get(_employeeData.Id);
            var skipEmployeeProgressUIHandler = targetEmployee.GetComponentInChildren<SkipEmployeeProgressUIHandler>();
            skipEmployeeProgressUIHandler.ActivateWindow(upgradeEmployeeData);
            WindowService.Close<UpgradeEmployeeWindow>();
        }
    }
}