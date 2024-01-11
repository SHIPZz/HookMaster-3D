using CodeBase.EntryPointSystem;
using CodeBase.Gameplay.Wallet;
using CodeBase.Services.BurnableObjects;
using CodeBase.Services.DataService;
using CodeBase.Services.Employee;
using CodeBase.Services.Extinguisher;
using CodeBase.Services.Factories.Camera;
using CodeBase.Services.Factories.Effect;
using CodeBase.Services.Factories.Employee;
using CodeBase.Services.Factories.Player;
using CodeBase.Services.Factories.RandomItems;
using CodeBase.Services.Factories.ShopItems;
using CodeBase.Services.Factories.UI;
using CodeBase.Services.Fire;
using CodeBase.Services.GOPool;
using CodeBase.Services.MiningFarm;
using CodeBase.Services.Player;
using CodeBase.Services.Profit;
using CodeBase.Services.Providers.Asset;
using CodeBase.Services.Providers.Camera;
using CodeBase.Services.Providers.EmployeeProvider;
using CodeBase.Services.Providers.Extinguisher;
using CodeBase.Services.Providers.Fire;
using CodeBase.Services.Providers.Location;
using CodeBase.Services.Providers.Player;
using CodeBase.Services.Providers.Tables;
using CodeBase.Services.RandomItems;
using CodeBase.Services.Reward;
using CodeBase.Services.ShopItemData;
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
        [SerializeField] private ExtinguisherProvider _extinguisherProvider;
        [SerializeField] private FireProvider _fireProvider;

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
            BindPools();
            BindRewardService();
            BindPlayerRewardService();
            BindShopItemFactory();
            BindShopItemService();
            BindMiningFarmService();
            BindRandomItemFactory();
            BindRandomItemService();
            BindExtinguisherService();
            BindFireService();
            BindFireSpawnerProvider();
            BindBurnableObjectService();
        }

        private void BindBurnableObjectService() => 
            Container.Bind<BurnableObjectService>().AsSingle();

        private void BindFireSpawnerProvider() =>
            Container.BindInstance(_fireProvider);

        private void BindFireService() =>
            Container.Bind<FireService>().AsSingle();

        private void BindExtinguisherService() =>
            Container.Bind<ExtinguisherService>().AsSingle();

        private void BindRandomItemService() =>
            Container.Bind<RandomItemService>().AsSingle();

        private void BindRandomItemFactory() =>
            Container.Bind<RandomItemFactory>().AsSingle();

        private void BindMiningFarmService() =>
            Container.Bind<MiningFarmService>().AsSingle();

        private void BindShopItemService() =>
            Container.Bind<ShopItemService>().AsSingle();

        private void BindShopItemFactory() =>
            Container.Bind<GameItemFactory>().AsSingle();

        private void BindPlayerRewardService() =>
            Container.BindInterfacesAndSelfTo<PlayerRewardService>().AsSingle();

        private void BindRewardService() =>
            Container.Bind<RewardService>().AsSingle();

        private void BindPools() =>
            Container
                .Bind<EffectPool>()
                .AsTransient();

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

        private void BindProfitService() =>
            Container
                .BindInterfacesAndSelfTo<ProfitService>()
                .AsSingle();

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
            Container.BindInstance(_extinguisherProvider);
        }

        private void BindFactories()
        {
            Container.Bind<IPlayerFactory>().To<PlayerFactory>().AsSingle();
            Container.Bind<ICameraFactory>().To<CameraFactory>().AsSingle();
            Container.Bind<IEmployeeFactory>().To<EmployeeFactory>().AsSingle();
            Container.Bind<IEffectFactory>().To<EffectFactory>().AsSingle();
            Container.Bind<UIFactory>().AsSingle();
        }

        private void BindStaticDataServices()
        {
            Container.Bind<PlayerStaticDataService>().AsSingle();
            Container.Bind<OfficeStaticDataService>().AsSingle();
            Container.Bind<UIStaticDataService>().AsSingle();
            Container.Bind<EffectStaticDataService>().AsSingle();
            Container.Bind<GameItemStaticDataService>().AsSingle();
        }
    }
}