using CodeBase.Constant;
using CodeBase.Gameplay.EmployeeSystem;
using CodeBase.Services.Providers.Asset;
using UnityEngine;
using Zenject;

namespace CodeBase.Services.Factories.Employee
{
    public class EmployeeFactory : IEmployeeFactory
    {
        private readonly IAssetProvider _assetProvider;
        private readonly DiContainer _diContainer;

        public EmployeeFactory(IAssetProvider assetProvider, DiContainer diContainer)
        {
            _diContainer = diContainer;
            _assetProvider = assetProvider;
        }

        public Gameplay.EmployeeSystem.Employee Create(Vector3 point,string name, int qualificationType, int salary, Vector3 targetPosition)
        {
            var employeePrefab = _assetProvider.Get<Gameplay.EmployeeSystem.Employee>(AssetPath.Employee);

            var employee = _diContainer.InstantiatePrefabForComponent<Gameplay.EmployeeSystem.Employee>(employeePrefab, 
                    point, 
                    Quaternion.identity, 
                    null);

            employee.Name = name;
            employee.QualificationType = qualificationType;
            employee.Salary = salary;
            employee.GetComponent<EmployeeMovement>().SetTarget(targetPosition);
            return employee;
        }
    }

    public interface IEmployeeFactory
    {
        Gameplay.EmployeeSystem.Employee Create(Vector3 point,string name, int qualificationType, int salary, Vector3 targetPosition);
    }
}