﻿using CodeBase.EntryPointSystem;
using CodeBase.Services.Data;
using CodeBase.Services.EmployeeHirer;
using CodeBase.Services.Factories.Bullet;
using CodeBase.Services.Factories.Camera;
using CodeBase.Services.Factories.Employee;
using CodeBase.Services.Factories.Player;
using CodeBase.Services.Factories.Weapon;
using CodeBase.Services.Providers.Asset;
using CodeBase.Services.Providers.Camera;
using CodeBase.Services.Providers.EmployeeProvider;
using CodeBase.Services.Providers.Location;
using CodeBase.Services.Providers.Player;
using CodeBase.Services.Providers.Tables;
using UnityEngine;
using Zenject;

namespace CodeBase.Installers.Game
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private LocationProvider _locationProvider;
        [SerializeField] private TableProvider _tableProvider;
        
        public override void InstallBindings()
        {
            BindEntryPoint();
            BindStaticDataServices();
            BindFactories();
            BindProviders();
            BindEmployeeHirerService();
        }

        private void BindEmployeeHirerService() => 
            Container.Bind<EmployeeHirerService>().AsSingle();

        private void BindEntryPoint() => 
            Container.BindInterfacesAndSelfTo<EntryPoint>().AsSingle();

        private void BindProviders()
        {
            Container.Bind<IAssetProvider>().To<AssetProvider>().AsSingle();
            Container.BindInstance(_locationProvider);
            Container.BindInstance(_tableProvider);
            Container.Bind<CameraProvider>().AsSingle();
            Container.Bind<PlayerProvider>().AsSingle();
            Container.Bind<EmployeeProvider>().AsSingle();
        }

        private void BindFactories()
        {
            Container.Bind<IPlayerFactory>().To<PlayerFactory>().AsSingle();
            Container.Bind<ICameraFactory>().To<CameraFactory>().AsSingle();
            Container.Bind<IBulletFactory>().To<BulletFactory>().AsSingle();
            Container.Bind<IWeaponFactory>().To<WeaponFactory>().AsSingle();
            Container.Bind<IEmployeeFactory>().To<EmployeeFactory>().AsSingle();
        }

        private void BindStaticDataServices()
        {
            Container.Bind<PlayerStaticDataService>().AsSingle();
            Container.Bind<WeaponStaticDataService>().AsSingle();
            Container.Bind<OfficeStaticDataService>().AsSingle();
        }
    }
}