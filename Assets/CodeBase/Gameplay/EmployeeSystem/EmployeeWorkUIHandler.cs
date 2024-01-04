using CodeBase.Constant;
using CodeBase.Services.Player;
using CodeBase.Services.Providers.Camera;
using CodeBase.Services.TriggerObserve;
using CodeBase.Services.UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.Gameplay.EmployeeSystem
{
    public class EmployeeWorkUIHandler : MonoBehaviour
    {
        [SerializeField] private TriggerObserver _triggerObserver;
        [SerializeField] private Employee _employee;
        [SerializeField] private float _downPositionY = 2f;
        [SerializeField] private float _upPositionY = 3f;
        [SerializeField] private float _downDuration = 0.5f;
        [SerializeField] private float _upDuration = 0.25f;
        [SerializeField] private EmployeeMovement _employeeMovement;

        private FloatingButtonService _floatingButtonService;
        private Button _invokeWorkButton;
        private PlayerAnimationService _playerAnimationService;

        [Inject]
        private void Construct(FloatingButtonService floatingButtonService, PlayerAnimationService playerAnimationService)
        {
            _playerAnimationService = playerAnimationService;
            _floatingButtonService = floatingButtonService;
        }

        private void OnEnable()
        {
            _triggerObserver.TriggerEntered += OnPlayerEntered;
            _triggerObserver.TriggerExited += OnPlayerExited;
        }

        private void OnDisable()
        {
            _triggerObserver.TriggerEntered -= OnPlayerEntered;
            _triggerObserver.TriggerExited -= OnPlayerExited;

            if (_invokeWorkButton != null)
                _invokeWorkButton.onClick.RemoveListener(OnInvokeClicked);
        }

        private void OnPlayerExited(Collider obj)
        {
            if (_employee.IsWorking || _employeeMovement.IsMovingToTable)
                return;

            _floatingButtonService.ShowFloatingButton(-_downPositionY, _downDuration, Quaternion.identity, 
                AssetPath.InvokeEmployeeWorkButton, _employee.transform, false, false);
        }

        private void OnPlayerEntered(Collider obj)
        {
            if (_employee.IsWorking || _employeeMovement.IsMovingToTable)
                return;

            _floatingButtonService.ShowFloatingButton(_upPositionY, _upDuration, Quaternion.identity,
                AssetPath.InvokeEmployeeWorkButton, _employee.transform, true, true);

            if (_invokeWorkButton == null)
                _invokeWorkButton = _floatingButtonService.Get();

            _invokeWorkButton.onClick.AddListener(OnInvokeClicked);
        }

        private void OnInvokeClicked()
        {
            _employee.StartWorking();
            _playerAnimationService.PlayPunchAnimation();
            _floatingButtonService.ShowFloatingButton(-_downPositionY, _downDuration, Quaternion.identity, 
                AssetPath.InvokeEmployeeWorkButton, _employee.transform, false, false);
        }
    }
}