using CodeBase.SO.Employee;
using UnityEngine;
using Zenject;

namespace CodeBase.Installers.ScriptableObject
{
    [CreateAssetMenu(fileName = nameof(EmployeeSOInstaller), menuName = "Installers/EmployeeSOInstaller")]
    public class EmployeeSOInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private EmployeeNameSO _employeeNameSo;

        public override void InstallBindings()
        {
            Container.BindInstance(_employeeNameSo);
        }
    }
}