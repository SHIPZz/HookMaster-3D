using CodeBase.Data;
using CodeBase.Services.Employee;
using CodeBase.Services.Providers.Camera;
using CodeBase.Services.TriggerObserve;
using CodeBase.Services.Window;
using CodeBase.UI;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.EmployeeSystem
{
    public class SkipEmployeeProgressUIHandler : MonoBehaviour
    {
        [SerializeField] private TriggerObserver _triggerObserver;
        [SerializeField] private Employee _employee;
        [SerializeField] private Vector3 _offset;
        
        private WindowService _windowService;
        private SkipProgressSliderWindow _skipProgressWindow;
        private CameraProvider _cameraProvider;
        private EmployeeDataService _employeeDataService;

        [Inject]
        private void Construct(WindowService windowService,
            CameraProvider cameraProvider, EmployeeDataService employeeDataService)
        {
            _employeeDataService = employeeDataService;
            _cameraProvider = cameraProvider;
            _windowService = windowService;
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
        }

        private void OnPlayerExited(Collider obj)
        {
            if (!_employee.IsUpgrading)
                return;

            if (_skipProgressWindow != null)
                _skipProgressWindow.Close();
        }

        private void OnPlayerEntered(Collider obj)
        {
            if (!_employee.IsUpgrading)
                return;

            _skipProgressWindow = _windowService.Get<SkipProgressSliderWindow>();

            Quaternion targetRotation = Quaternion.LookRotation(_cameraProvider.Camera.transform.forward);

            UpgradeEmployeeData targetUpgradeEmployeeData = _employeeDataService.GetUpgradeEmployeeData(_employee.Id);

            _skipProgressWindow.transform.SetParent(_employee.transform);
            _skipProgressWindow.Init(targetUpgradeEmployeeData, 
                targetUpgradeEmployeeData.LastUpgradeTime, 
                targetUpgradeEmployeeData.LastUpgradeWindowOpenedTime,targetRotation);

            _skipProgressWindow.transform.position = transform.position + _offset;
            
            _skipProgressWindow.Open();
        }
    }
}