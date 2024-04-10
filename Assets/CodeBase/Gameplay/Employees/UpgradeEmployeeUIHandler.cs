using System;
using CodeBase.Constant;
using CodeBase.Gameplay.BurnableObjectSystem;
using CodeBase.Services.Employees;
using CodeBase.Services.Providers.Player;
using CodeBase.Services.TriggerObserve;
using CodeBase.Services.UI;
using CodeBase.Services.Window;
using CodeBase.UI.Upgrade;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.Gameplay.Employees
{
    public class UpgradeEmployeeUIHandler : MonoBehaviour
    {
        [SerializeField] private TriggerObserver _triggerObserver;
        [SerializeField] private float _upPositionY = 2f;
        [SerializeField] private float _downPositionY = 5f;
        [SerializeField] private float _upPositionDuration = 0.5f;
        [SerializeField] private float _downPositionDuration = 0.2f;
        [SerializeField] private Employee _employee;

        private Button _upgradeButton;
        private WindowService _windowService;
        private FloatingButtonService _floatingButtonService;
        private EmployeeService _employeeService;
        private PlayerProvider _playerProvider;
        private bool _playerEntered;

        [Inject]
        private void Construct(WindowService windowService, FloatingButtonService floatingButtonService,
            EmployeeService employeeService, PlayerProvider playerProvider)
        {
            _playerProvider = playerProvider;
            _employeeService = employeeService;
            _floatingButtonService = floatingButtonService;
            _windowService = windowService;
        }

        private void Start()
        {
            _employee.UpgradeStarted += DisableUpgradeButton;
            _employeeService.EmployeeUpdated += TryShowUpgradeButton;
            _employee.Burned += DisableUpgradeButton;
            _triggerObserver.TriggerEntered += OnPlayerEntered;
            _triggerObserver.TriggerExited += OnPlayerExited;
        }

        private void OnDestroy()
        {
            if (_upgradeButton != null)
                _upgradeButton.onClick.RemoveListener(OnUpgradeButtonClicked);

            _employee.Burned -= DisableUpgradeButton;
            _employee.UpgradeStarted -= DisableUpgradeButton;
            _employeeService.EmployeeUpdated -= TryShowUpgradeButton;
            _triggerObserver.TriggerEntered -= OnPlayerEntered;
            _triggerObserver.TriggerExited -= OnPlayerExited;
        }

        private void DisableUpgradeButton(IBurnable burnable) =>
            _upgradeButton?.gameObject.SetActive(false);

        private void OnPlayerExited(Collider obj)
        {
            _playerEntered = false;

            if (_upgradeButton != null && _upgradeButton.gameObject.activeSelf)
            {
                _floatingButtonService.ShowFloatingButton(-_downPositionY, _downPositionDuration, Quaternion.identity,
                    AssetPath.UpgradeEmployeeButton,
                    _employee.transform, false, false);
                return;
            }

            if(_employeeService.UpdateMaxReached(_employee.Id))
                return;
            
            if (!_employee.IsWorking || _employee.IsUpgrading || _employee.IsBurned)
                return;

            if (_playerProvider.PlayerPaperContainer.HasPapers)
                return;

            if (_upgradeButton == null)
                return;

            if (!_upgradeButton.gameObject.activeSelf)
                return;

            _floatingButtonService.ShowFloatingButton(-_downPositionY, _downPositionDuration, Quaternion.identity,
                AssetPath.UpgradeEmployeeButton,
                _employee.transform, false, false);
        }

        private void OnPlayerEntered(Collider obj)
        {
            _playerEntered = true;
            
            if (UnacceptableConditionsToShowButtonMatch())
                return;

            _floatingButtonService.ShowFloatingButton(_upPositionY, _upPositionDuration, Quaternion.identity,
                AssetPath.UpgradeEmployeeButton,
                _employee.transform, true, true);

            if (_upgradeButton != null)
                return;

            SetAndSubscribeUpgradeButton();
        }

        private bool UnacceptableConditionsToShowButtonMatch()
        {
            if (_employeeService.UpdateMaxReached(_employee.Id))
                return true;

            if (_upgradeButton != null)
                _upgradeButton.gameObject.SetActive(false);

            if (_playerProvider.PlayerPaperContainer.HasPapers)
                return true;

            if (!_employee.IsWorking || _employee.IsUpgrading)
                return true;
            
            return false;
        }

        private void TryShowUpgradeButton(Employee employee)
        {
            if (employee.Id != _employee.Id)
                return;

            _floatingButtonService.ShowFloatingButton(_upPositionY, _upPositionDuration, Quaternion.identity,
                AssetPath.UpgradeEmployeeButton,
                _employee.transform, true, true);

            SetAndSubscribeUpgradeButton();

            _upgradeButton.gameObject.SetActive(true);
        }

        private void SetAndSubscribeUpgradeButton()
        {
            _upgradeButton = _floatingButtonService.Get();
            _upgradeButton.onClick.AddListener(OnUpgradeButtonClicked);
        }

        private void OnUpgradeButtonClicked()
        {
            var upgradeWindow = _windowService.OpenAndGet<UpgradeEmployeeWindow>();
            upgradeWindow.Init(_employee);
        }
    }
}