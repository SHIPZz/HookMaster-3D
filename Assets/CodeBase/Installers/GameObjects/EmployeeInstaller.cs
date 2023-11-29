using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace CodeBase.Installers.GameObjects
{
    [RequireComponent(typeof(GameObjectContext))]
    public class EmployeeInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInstance(GetComponent<NavMeshAgent>());
        }
    }
}