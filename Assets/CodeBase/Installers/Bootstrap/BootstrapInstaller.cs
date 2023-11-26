using CodeBase.Services.Coroutine;
using CodeBase.Services.Input;
using Zenject;

namespace CodeBase.Installers.Bootstrap
{
    public class BootstrapInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindInputService();
            BindCoroutineRunner();
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