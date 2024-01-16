using CodeBase.Gameplay.Clients;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace CodeBase.Installers.GameObjects.Clients
{
    [RequireComponent(typeof(GameObjectContext))]
    public class ClientInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInstance(GetComponent<NavMeshAgent>());
            Container.BindInstance(GetComponent<Animator>());
            Container.BindInstance(GetComponent<Transform>());
            Container.Bind<ClientAnimator>().AsSingle();
            Container.BindInterfacesAndSelfTo<ClientAnimOnMoving>().AsSingle();
            Container.BindInterfacesAndSelfTo<ClientMovement>().AsSingle();
        }
    }
}