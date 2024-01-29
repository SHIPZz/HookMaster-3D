using CodeBase.Cheats;
using CodeBase.InfraStructure;
using CodeBase.Services.Coroutine;
using CodeBase.Services.Input;
using CodeBase.Services.Profit;
using CodeBase.Services.Saves;
using CodeBase.Services.SaveSystem;
using CodeBase.Services.Time;
using CodeBase.Services.WorldData;
using UnityEngine;
using Zenject;

namespace CodeBase.Installers.Bootstrap
{
    public class BootstrapInstaller : MonoInstaller, IInitializable
    {
        [SerializeField] private LoadingCurtain _loadingCurtain;

        public override void InstallBindings()
        {
            BindInputService();
            BindCoroutineRunner();
            BindSaveSystem();
            BindLoadingCurtain();
            BindGameStateMachine();
            BindStateFactory();
            BindWorldDataService();
            BindWorldTimeService();
            BindCheats();
            BindSaveEvents();
            Container.BindInterfacesTo<BootstrapInstaller>()
                .FromInstance(this);
        }

        private void BindSaveEvents()
        {
            Container.Bind<SaveOnEveryMinute>().AsSingle();
            Container.Bind<SaveFacade>().AsSingle();
        }

        public void Initialize()
        {
            var gameStateMachine = Container.Resolve<IGameStateMachine>();
            gameStateMachine.ChangeState<BootstrapState>();
        }

        private void BindCheats() =>
            Container.BindInterfacesAndSelfTo<Cheat>().AsSingle();

        private void BindWorldTimeService() =>
            Container.BindInterfacesAndSelfTo<WorldTimeService>()
                .AsSingle();

        private void BindWorldDataService() =>
            Container
                .Bind<IWorldDataService>()
                .To<WorldDataService>()
                .AsSingle();

        private void BindStateFactory() =>
            Container
                .Bind<IStateFactory>()
                .To<StateFactory>()
                .AsSingle();

        private void BindGameStateMachine() =>
            Container.Bind<IGameStateMachine>()
                .To<GameStateMachine>()
                .AsSingle();

        private void BindLoadingCurtain() =>
            Container.Bind<ILoadingCurtain>()
                .FromInstance(_loadingCurtain);

        private void BindSaveSystem() =>
            Container.Bind<ISaveSystem>().To<PlayerPrefsSaveSystem>()
                .AsSingle();

        private void BindCoroutineRunner() =>
            Container.Bind<ICoroutineRunner>()
                .To<CoroutineRunner>()
                .FromInstance(GetComponent<CoroutineRunner>());

        private void BindInputService() =>
            Container.Bind<IInputService>()
                .To<InputService>().AsSingle();
    }
}