using System.Linq;
using CodeBase.Gameplay.EmployeeSystem;
using CodeBase.Services.Providers.EmployeeProvider;
using CodeBase.UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class EmployeeWorkWindow : WindowBase
{
    [SerializeField] private Button _invokeEmployeeWorkButton;
    private EmployeeProvider _employeeProvider;
    private Employee _lastTargetEmployee;
    
    public Button InvokeEmployeeWorkButton => _invokeEmployeeWorkButton;

    [Inject]
    private void Construct(EmployeeProvider employeeProvider) => 
        _employeeProvider = employeeProvider;

    private void OnEnable() => 
        _invokeEmployeeWorkButton.onClick.AddListener(OnClicked);

    private void OnDisable() => 
        _invokeEmployeeWorkButton.onClick.RemoveListener(OnClicked);

    private void OnClicked()
    {
        Employee employee = _employeeProvider.Employees.FirstOrDefault(x => x.Guid == _lastTargetEmployee.Guid);
        employee.StartWorking();
        Close();
    }

    public void SetLastTargetEmployee(Employee employee)
    {
        _lastTargetEmployee = employee;
    }

    public override void Open()
    {
        
    }

    public override void Close()
    {
        Destroy(gameObject);
    }
}