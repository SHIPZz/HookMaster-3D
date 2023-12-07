using System;
using CodeBase.Gameplay.PlayerSystem;
using CodeBase.Services.TriggerObserve;
using CodeBase.Services.Window;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.EmployeeSystem
{
    public class EmployeeTriggerInvokeWorkZone : MonoBehaviour
    {
        [SerializeField] private TriggerObserver _triggerObserver;
        [SerializeField] private Employee _employee;

        private WindowService _windowService;

        [Inject]
        private void Construct(WindowService windowService)
        {
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
            if (!obj.gameObject.TryGetComponent(out Player player))
                return;
            
            if(_employee.IsWorking)
                return;

            _windowService.Close<EmployeeWorkWindow>();
        }

        private void OnPlayerEntered(Collider obj)
        {
            if (!obj.gameObject.TryGetComponent(out Player player))
                return;
            
            if(_employee.IsWorking)
                return;

            var window = _windowService.GetAndOpen<EmployeeWorkWindow>();
            window.SetLastTargetEmployee(_employee);
        }
    }
}