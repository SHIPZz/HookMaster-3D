using CodeBase.Cheats;
using CodeBase.Services.Coroutine;
using CodeBase.Services.Input;
using CodeBase.Services.SaveSystem;
using Zenject;

namespace CodeBase.Installers.Bootstrap
{
    public class BootstrapInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindInputService();
            BindCoroutineRunner();
            BindSaveSystem();
            Container.BindInterfacesAndSelfTo<Cheat>().AsSingle();
        }

        private void BindSaveSystem()
        {
            Container.Bind<ISaveSystem>().To<PlayerPrefsSaveSystem>()
                .AsSingle();
        }

        private void BindCoroutineRunner()
        {
            Container.Bind<ICoroutineRunner>()
                .To<CoroutineRunner>()
                .FromInstance(GetComponent<CoroutineRunner>());
        }

        private void BindInputService()
        {
            Container.Bind<IInputService>()
                .To<InputService>().AsSingle();
        }
    }
}