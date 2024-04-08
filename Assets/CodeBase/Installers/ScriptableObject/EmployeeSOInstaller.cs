using CodeBase.SO.Employee;
using UnityEngine;
using Zenject;

namespace CodeBase.Installers.ScriptableObject
{
    [CreateAssetMenu(fileName = nameof(EmployeeSOInstaller), menuName = "Installers/EmployeeSOInstaller")]
    public class EmployeeSOInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private EmployeeNameSO _employeeNameSo;
        [SerializeField] private EmployeeSkinSO _employeeSkinSo;
        [SerializeField] private EmployeeStatsSO _employeeStats;

        public override void InstallBindings()
        {
            Container.BindInstance(_employeeNameSo);
            Container.BindInstance(_employeeSkinSo);
            Container.BindInstance(_employeeStats);
        }
    }
}