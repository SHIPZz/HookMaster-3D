using System;
using CodeBase.Services.EmployeeHirer;
using UnityEngine;
using Zenject;

namespace CodeBase.UI
{
    public class EmployeePresenter : MonoBehaviour
    {
        private EmployeeView _employeeView;
        private EmployeeHirerService _employeeHirerService;

        [Inject]
        private void Construct(EmployeeHirerService employeeHirerService) => 
            _employeeHirerService = employeeHirerService;

        private void Awake() => 
            _employeeView = GetComponent<EmployeeView>();

        private void OnEnable() => 
            _employeeView.Selected += _employeeHirerService.TryHire;

        private void OnDisable() => 
            _employeeView.Selected -= _employeeHirerService.TryHire;
    }
}