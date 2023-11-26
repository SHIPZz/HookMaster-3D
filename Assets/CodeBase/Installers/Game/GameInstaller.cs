using CodeBase.EntryPointSystem;
using CodeBase.Services.Data;
using CodeBase.Services.Factories.Bullet;
using CodeBase.Services.Factories.Camera;
using CodeBase.Services.Factories.Player;
using CodeBase.Services.Factories.Weapon;
using CodeBase.Services.Providers.Asset;
using CodeBase.Services.Providers.Camera;
using CodeBase.Services.Providers.Location;
using CodeBase.Services.Providers.Player;
using UnityEngine;
using Zenject;

namespace CodeBase.Installers.Game
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private LocationProvider _locationProvider;
        
        public override void InstallBindings()
        {
            BindEntryPoint();
            BindStaticDataServices();
            BindFactories();
            BindProviders();
        }

        private void BindEntryPoint()
        {
            Container.BindInterfacesAndSelfTo<EntryPoint>().AsSingle();
        }

        private void BindProviders()
        {
            Container.Bind<IAssetProvider>().To<AssetProvider>().AsSingle();
            Container.BindInstance(_locationProvider);
            Container.Bind<CameraProvider>().AsSingle();
            Container.Bind<PlayerProvider>().AsSingle();
        }

        private void BindFactories()
        {
            Container.Bind<IPlayerFactory>().To<PlayerFactory>().AsSingle();
            Container.Bind<ICameraFactory>().To<CameraFactory>().AsSingle();
            Container.Bind<IBulletFactory>().To<BulletFactory>().AsSingle();
            Container.Bind<IWeaponFactory>().To<WeaponFactory>().AsSingle();
        }

        private void BindStaticDataServices()
        {
            Container.Bind<PlayerStaticDataService>().AsSingle();
            Container.Bind<WeaponStaticDataService>().AsSingle();
        }
    }
}