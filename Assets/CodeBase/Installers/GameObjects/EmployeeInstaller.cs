using CodeBase.Gameplay.EmployeeSystem;
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
            Container.BindInstance(GetComponent<Animator>());
            Container.Bind<EmployeeAnimator>().AsSingle();
            Container.BindInterfacesAndSelfTo<AnimOnMoving>().AsSingle();
        }
    }
}