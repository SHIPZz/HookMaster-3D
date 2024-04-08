using CodeBase.Data;
using CodeBase.Gameplay.BurnableObjectSystem;
using CodeBase.Gameplay.PlayerSystem;
using CodeBase.Services.Employees;
using CodeBase.Services.Providers.Player;
using CodeBase.Services.TriggerObserve;
using CodeBase.Services.Window;
using CodeBase.UI.SkipProgress;
using RootMotion;
using UnityEngine;
using Zenject;
using CameraController = CodeBase.Services.CameraServices.CameraController;

namespace CodeBase.Gameplay.Employees
{
    public class SkipEmployeeProgressUIHandler : MonoBehaviour
    {
        [SerializeField] private TriggerObserver _triggerObserver;
        [SerializeField] private Employee _employee;
        [SerializeField] private Vector3 _offset;

        private WindowService _windowService;
        private SkipProgressSliderWindow _skipProgressWindow;
        private EmployeeDataService _employeeDataService;
        private PlayerPaperContainer _playerPaperContainer;
        private PlayerProvider _playerProvider;
        private CameraController _cameraController;

        [Inject]
        private void Construct(WindowService windowService,
            CameraController cameraController, EmployeeDataService employeeDataService,
            PlayerProvider playerProvider)
        {
            _cameraController = cameraController;
            _playerProvider = playerProvider;
            _employeeDataService = employeeDataService;
            _windowService = windowService;
        }

        private void Start()
        {
            _playerPaperContainer = _playerProvider.PlayerPaperContainer;
            _employeeDataService.EmployeeUpdated += TryCloseWindow;
        }

        private void OnEnable()
        {
            _triggerObserver.TriggerEntered += OnPlayerEntered;
            _triggerObserver.TriggerExited += OnPlayerExited;
            _employee.PaperAdded += HideWindow;
            _employee.Burned += HideWindow;
        }

        private void OnDisable()
        {
            _triggerObserver.TriggerEntered -= OnPlayerEntered;
            _triggerObserver.TriggerExited -= OnPlayerExited;
            _employee.PaperAdded -= HideWindow;
            _employee.Burned -= HideWindow;
            _employeeDataService.EmployeeUpdated -= TryCloseWindow;
        }

        public void ActivateWindow(UpgradeEmployeeData targetUpgradeEmployeeData)
        {
            if (_skipProgressWindow != null)
            {
                _skipProgressWindow.Show();
                return;
            }

            Quaternion targetRotation = Quaternion.LookRotation(_cameraController.Camera.transform.forward);
            _skipProgressWindow = _windowService.GetNew<SkipProgressSliderWindow>();
            _skipProgressWindow.transform.SetParent(_employee.transform);

            _skipProgressWindow.Init(targetUpgradeEmployeeData, targetRotation);

            _skipProgressWindow.transform.position = transform.position + _offset;

            _skipProgressWindow.Open();
        }

        private void HideWindow(IBurnable burnable)
        {
            if (_skipProgressWindow != null)
                _skipProgressWindow.Hide();
        }

        private void OnPlayerExited(Collider obj)
        {
            if (!_employee.IsUpgrading || !_employee.IsWorking || _employee.IsBurned)
                return;
            
            if (_employee.HasPapers || _playerPaperContainer.HasPapers)
                return;

            if (_skipProgressWindow != null)
                _skipProgressWindow.Hide();
        }

        private void OnPlayerEntered(Collider obj)
        {
            if (!_employee.IsUpgrading || !_employee.IsWorking || _employee.IsBurned)
                return;

            if (_employee.HasPapers || _playerPaperContainer.HasPapers)
                return;

            UpgradeEmployeeData targetUpgradeEmployeeData = _employeeDataService.GetUpgradeEmployeeData(_employee.Id);

            ActivateWindow(targetUpgradeEmployeeData);
        }

        private void TryCloseWindow(EmployeeData employeeData)
        {
            if (employeeData.Id != _employee.Id)
                return;
                
            if (_skipProgressWindow != null)
                _skipProgressWindow.Close();
        }
    }
}