using CodeBase.Constant;
using CodeBase.Services.Providers.Camera;
using CodeBase.Services.TriggerObserve;
using CodeBase.Services.UI;
using CodeBase.Services.Window;
using CodeBase.UI.Upgrade;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.Gameplay.EmployeeSystem
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
        private CameraProvider _cameraProvider;
        private WindowService _windowService;
        private FloatingButtonService _floatingButtonService;

        [Inject]
        private void Construct(CameraProvider cameraProvider, WindowService windowService,
            FloatingButtonService floatingButtonService)
        {
            _floatingButtonService = floatingButtonService;
            _windowService = windowService;
            _cameraProvider = cameraProvider;
        }

        private void OnEnable()
        {
            _triggerObserver.TriggerEntered += OnPlayerEntered;
            _triggerObserver.TriggerExited += OnPlayerExited;
        }

        private void OnDisable()
        {
            if (_upgradeButton != null)
                _upgradeButton.onClick.RemoveListener(OnUpgradeButtonClicked);
            
            _triggerObserver.TriggerEntered -= OnPlayerEntered;
            _triggerObserver.TriggerExited -= OnPlayerExited;
        }

        private void OnUpgradeButtonClicked()
        {
            var upgradeWindow = _windowService.OpenAndGet<UpgradeEmployeeWindow>();
            upgradeWindow.Init(_employee);
            _upgradeButton.gameObject.SetActive(false);
        }

        private void OnPlayerExited(Collider obj)
        {
            if (!_employee.IsWorking || _employee.IsUpgrading)
                return;
            
            if(_upgradeButton == null)
                return;
            
            if(!_upgradeButton.gameObject.activeSelf)
                return;
            
            _floatingButtonService.ShowFloatingButton(-_downPositionY, _downPositionDuration, Quaternion.identity, 
                AssetPath.UpgradeEmployeeButton,
                _employee.transform,false, false);
        }

        private void OnPlayerEntered(Collider obj)
        {
            if (!_employee.IsWorking || _employee.IsUpgrading)
                return;

            _floatingButtonService.ShowFloatingButton(_upPositionY, _upPositionDuration, Quaternion.identity, 
                AssetPath.UpgradeEmployeeButton,
                _employee.transform, true, true);

            if (_upgradeButton != null)
                return;

            _upgradeButton = _floatingButtonService.Get();
            _upgradeButton.onClick.AddListener(OnUpgradeButtonClicked);
        }
    }
}