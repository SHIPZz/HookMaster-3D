using CodeBase.Gameplay.PlayerSystem;
using UnityEngine;
using Zenject;

namespace CodeBase.Installers.GameObjects
{
    [RequireComponent(typeof(GameObjectContext))]
    public class PlayerInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<Animator>().FromInstance(GetComponent<Animator>());
            Container.Bind<Rigidbody>().FromInstance(GetComponent<Rigidbody>());
            Container.Bind<PlayerAnimator>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerInput>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerMovementMediator>().AsSingle();
            Container.Bind<AnimOnRunning>().FromInstance(GetComponent<AnimOnRunning>());
            Container.Bind<PlayerMovement>().FromInstance(GetComponent<PlayerMovement>());
        }
    }
}