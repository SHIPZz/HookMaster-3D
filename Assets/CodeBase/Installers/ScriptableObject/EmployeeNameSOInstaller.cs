using CodeBase.SO.Employee;
using UnityEngine;
using Zenject;

namespace CodeBase.Installers.ScriptableObject
{
    [CreateAssetMenu(fileName = nameof(EmployeeNameSOInstaller), menuName = "Installers/EmployeeNameSOInstaller")]
    public class EmployeeNameSOInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private EmployeeNameSO _employeeNameSo;

        public override void InstallBindings()
        {
            Container.BindInstance(_employeeNameSo);
        }
    }
}