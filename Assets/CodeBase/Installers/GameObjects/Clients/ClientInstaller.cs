using CodeBase.Gameplay.Clients;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace CodeBase.Installers.GameObjects.Clients
{
    [RequireComponent(typeof(GameObjectContext))]
    public class ClientInstaller : MonoInstaller
    {
        [SerializeField] private NavMeshAgent _navMeshAgent;
        
        public override void InstallBindings()
        {
            Container.BindInstance(GetComponent<Animator>());
            Container.BindInstance(_navMeshAgent);
            Container.Bind<ClientAnimator>().AsSingle();
            Container.BindInterfacesAndSelfTo<ClientAnimOnMoving>().AsSingle();
        }
    }
}