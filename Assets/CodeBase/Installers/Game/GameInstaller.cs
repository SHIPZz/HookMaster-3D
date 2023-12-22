using CodeBase.EntryPointSystem;
using CodeBase.Gameplay.Wallet;
using CodeBase.Services.DataService;
using CodeBase.Services.Employee;
using CodeBase.Services.Factories.Bullet;
using CodeBase.Services.Factories.Camera;
using CodeBase.Services.Factories.Employee;
using CodeBase.Services.Factories.Player;
using CodeBase.Services.Factories.UI;
using CodeBase.Services.Factories.Weapon;
using CodeBase.Services.Player;
using CodeBase.Services.Profit;
using CodeBase.Services.Providers.Asset;
using CodeBase.Services.Providers.Camera;
using CodeBase.Services.Providers.EmployeeProvider;
using CodeBase.Services.Providers.Location;
using CodeBase.Services.Providers.Player;
using CodeBase.Services.Providers.Tables;
using CodeBase.Services.Sound;
using CodeBase.Services.Window;
using UnityEngine;
using Zenject;

namespace CodeBase.Installers.Game
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private LocationProvider _locationProvider;
        [SerializeField] private TableService tableService;
        
        public override void InstallBindings()
        {
            BindEntryPoint();
            BindStaticDataServices();
            BindFactories();
            BindProviders();
            BindEmployeeHirerService();
            BindWindowService();
            BindEmployeeLazinessService();
            BindWalletService();
            BindEmployeeSalaryService();
            BindProfitService();
            BindEmployeeDataService();
            BindSoundService();
            BindPlayerAnimationService();
        }

        private void BindPlayerAnimationService() =>
            Container
                .Bind<PlayerAnimationService>()
                .AsSingle();

        private void BindSoundService() => 
            Container.Bind<SettingsService>().AsSingle();

        private void BindEmployeeDataService() =>
            Container
                .Bind<EmployeeDataService>()
                .AsSingle();

        private void BindProfitService()
        {
            Container
                .BindInterfacesAndSelfTo<ProfitService>()
                .AsSingle();
        }

        private void BindEmployeeSalaryService() =>
            Container
                .Bind<EmployeeSalaryService>()
                .AsSingle();

        private void BindWalletService() =>
            Container
                .Bind<WalletService>()
                .AsSingle();

        private void BindEmployeeLazinessService() =>
            Container
                .BindInterfacesAndSelfTo<EmployeeLazinessService>()
                .AsSingle();

        private void BindWindowService() => 
            Container.Bind<WindowService>().AsSingle();

        private void BindEmployeeHirerService() => 
            Container.Bind<EmployeeHirerService>().AsSingle();

        private void BindEntryPoint() => 
            Container.BindInterfacesAndSelfTo<EntryPoint>().AsSingle();

        private void BindProviders()
        {
            Container.Bind<IAssetProvider>().To<AssetProvider>().AsSingle();
            Container.BindInstance(_locationProvider);
            Container.BindInstance(tableService);
            Container.Bind<CameraProvider>().AsSingle();
            Container.Bind<PlayerProvider>().AsSingle();
            Container.Bind<EmployeeService>().AsSingle();
        }

        private void BindFactories()
        {
            Container.Bind<IPlayerFactory>().To<PlayerFactory>().AsSingle();
            Container.Bind<ICameraFactory>().To<CameraFactory>().AsSingle();
            Container.Bind<IBulletFactory>().To<BulletFactory>().AsSingle();
            Container.Bind<IWeaponFactory>().To<WeaponFactory>().AsSingle();
            Container.Bind<IEmployeeFactory>().To<EmployeeFactory>().AsSingle();
            Container.Bind<UIFactory>().AsSingle();
        }

        private void BindStaticDataServices()
        {
            Container.Bind<PlayerStaticDataService>().AsSingle();
            Container.Bind<WeaponStaticDataService>().AsSingle();
            Container.Bind<OfficeStaticDataService>().AsSingle();
            Container.Bind<UIStaticDataService>().AsSingle();
        }
    }
}