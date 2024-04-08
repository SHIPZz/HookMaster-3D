using CodeBase.Gameplay.SoundPlayer;
using CodeBase.Services.UI;
using CodeBase.UI.Pause;
using UnityEngine;
using Zenject;

namespace CodeBase.Installers.Game
{
    public class GameUIInstaller : MonoInstaller
    {
        
        public override void InstallBindings()
        {
            BindFloatingServices();
            BindUIService();
            BindUIProvider();
            BindPauseWindowController();
        }

        private void BindPauseWindowController()
        {
            Container.BindInterfacesAndSelfTo<PauseWindowController>().AsSingle();
        }

        private void BindFloatingServices()
        {
            Container.Bind<FloatingTextService>().AsTransient();
            Container.Bind<FloatingButtonService>().AsTransient();
        }

        private void BindUIProvider()
        {
            Container.Bind<UIProvider>().AsSingle();
        }

        private void BindUIService()
        {
            Container.Bind<UIService>().AsSingle();
        }
    }
}