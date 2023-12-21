using CodeBase.Gameplay.PlayerSystem;
using CodeBase.Services.Player;
using CodeBase.Services.Providers.Player;
using UnityEngine;
using Zenject;

namespace CodeBase.Installers.GameObjects
{
    [RequireComponent(typeof(GameObjectContext))]
    public class PlayerInstaller : MonoInstaller, IInitializable
    {
        private PlayerProvider _playerProvider;
        private PlayerAnimationService _playerAnimationService;

        [Inject]
        private void Construct(PlayerProvider playerProvider, PlayerAnimationService playerAnimationService)
        {
            _playerAnimationService = playerAnimationService;
            _playerProvider = playerProvider;
        }
        
        public override void InstallBindings()
        {
            Container.Bind<Animator>().FromInstance(GetComponent<Animator>());
            Container.Bind<Rigidbody>().FromInstance(GetComponent<Rigidbody>());
            Container.Bind<PlayerAnimator>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerInput>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerMovementMediator>().AsSingle();
            Container.Bind<AnimOnMoving>().FromInstance(GetComponent<AnimOnMoving>());
            Container.Bind<PlayerMovement>().FromInstance(GetComponent<PlayerMovement>());
            Container.BindInterfacesTo<PlayerInstaller>().FromInstance(this);
        }

        public void Initialize()
        {
            _playerProvider.PlayerInput = Container.Resolve<PlayerInput>();
            _playerAnimationService.SetPlayerAnimator(Container.Resolve<PlayerAnimator>());
        }
    }
}