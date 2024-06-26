﻿using CodeBase.Animations;
using CodeBase.EntryPointSystem;
using CodeBase.Gameplay.PlayerSystem;
using CodeBase.Gameplay.Tutorial;
using CodeBase.MaterialChanger;
using CodeBase.Services.BurnableObjects;
using CodeBase.Services.CameraServices;
using CodeBase.Services.CircleRouletteServices;
using CodeBase.Services.Clients;
using CodeBase.Services.DataService;
using CodeBase.Services.Employees;
using CodeBase.Services.Extinguisher;
using CodeBase.Services.Factories.Camera;
using CodeBase.Services.Factories.Clients;
using CodeBase.Services.Factories.Effect;
using CodeBase.Services.Factories.Employee;
using CodeBase.Services.Factories.GameItem;
using CodeBase.Services.Factories.Player;
using CodeBase.Services.Factories.UI;
using CodeBase.Services.Fire;
using CodeBase.Services.Focus;
using CodeBase.Services.GameItemServices;
using CodeBase.Services.GOPool;
using CodeBase.Services.GOPush;
using CodeBase.Services.Mining;
using CodeBase.Services.Player;
using CodeBase.Services.Profit;
using CodeBase.Services.Providers.Asset;
using CodeBase.Services.Providers.Couchs;
using CodeBase.Services.Providers.Extinguisher;
using CodeBase.Services.Providers.Fire;
using CodeBase.Services.Providers.Location;
using CodeBase.Services.Providers.Player;
using CodeBase.Services.Providers.PurchaseableItemProviders;
using CodeBase.Services.Providers.Tables;
using CodeBase.Services.PurchaseableItemServices;
using CodeBase.Services.RandomItems;
using CodeBase.Services.Reward;
using CodeBase.Services.ShopItemDataServices;
using CodeBase.Services.Sound;
using CodeBase.Services.Wallet;
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
        [SerializeField] private PurchaseableItemProvider _purchaseableItemProvider;
        [SerializeField] private ClientProvider _clientProvider;
        [SerializeField] private CouchService _couchService;
        [SerializeField] private CameraFocus _cameraFocus;
        [SerializeField] private CameraController _cameraController;

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
            BindProfitService();
            BindEmployeeDataService();
            BindSoundService();
            BindPlayerAnimationService();
            BindPools();
            BindRewardService();
            BindShopItemFactory();
            BindShopItemService();
            BindMiningFarmService();
            BindRandomItemService();
            BindExtinguisherService();
            BindFireService();
            BindFireSpawnerProvider();
            BindBurnableObjectService();
            BindRendererMaterialChangerService();
            BindPlayerIKService();
            BindCircleRouletteService();
            BindShopItemDataService();
            BindPurchaseableItemService();
            GameObjectPushService();
            BindClientServices();
            BindCouchService();
            BindTutorials();
            BindNumberTextAnimService();
            BindMaterialFadeAnimService();
            BindPlayerInputService();
            BindFocusService();
            BindEmployeeService();
            BindCameraFocus();
            BindCameraController();
        }

        private void BindCameraController() => 
            Container.BindInstance(_cameraController);

        private void BindCameraFocus() => 
            Container.BindInstance(_cameraFocus);

        private void BindEmployeeService() => 
            Container.Bind<EmployeeService>().AsSingle();

        private void BindFocusService()
        {
            Container.BindInterfacesAndSelfTo<FocusService>().AsSingle();
        }

        private void BindPlayerInputService() => 
            Container.BindInterfacesAndSelfTo<PlayerInputService>().AsSingle();

        private void BindMaterialFadeAnimService()
        {
            Container.Bind<MaterialFadeAnimService>().AsSingle();
        }

        private void BindNumberTextAnimService()
        {
            Container.Bind<NumberTextAnimService>().AsTransient();
        }

        private void BindTutorials()
        {
            Container.BindInterfacesAndSelfTo<TutorialRunner>().AsSingle();
        }

        private void BindCouchService()
        {
            Container.BindInstance(_couchService);
        }

        private void BindClientServices()
        {
            Container.Bind<ClientObjectService>().AsSingle();
            Container.Bind<ClientServeService>().AsSingle();
            Container.Bind<ClientFactory>().AsSingle();
            Container.BindInstance(_clientProvider);
        }

        private void GameObjectPushService() => 
            Container.Bind<GameObjectPushService>().AsTransient();

        private void BindPurchaseableItemService() => 
            Container.Bind<PurchaseableItemService>().AsSingle();

        private void BindShopItemDataService() => 
            Container.Bind<ShopItemDataService>().AsSingle();

        private void BindCircleRouletteService() => 
            Container.Bind<CircleRouletteService>().AsSingle();

        private void BindPlayerIKService() => 
            Container.Bind<PlayerIKService>().AsSingle();

        private void BindRendererMaterialChangerService()
        {
            Container.Bind<RendererMaterialChangerService>().AsTransient();
            Container.Bind<ChildRendererMaterialChangerService>().AsTransient();
        }

        private void BindBurnableObjectService() => 
            Container.Bind<BurnableObjectService>().AsSingle();

        private void BindFireSpawnerProvider() =>
            Container.BindInstance(_fireProvider);

        private void BindFireService() =>
            Container.BindInterfacesAndSelfTo<FireService>().AsSingle();

        private void BindExtinguisherService() =>
            Container.Bind<ExtinguisherService>().AsSingle();

        private void BindRandomItemService() =>
            Container.Bind<RandomItemService>().AsSingle();

        private void BindMiningFarmService() =>
            Container.Bind<MiningFarmService>().AsSingle();

        private void BindShopItemService() =>
            Container.Bind<GameItemService>().AsSingle();

        private void BindShopItemFactory() =>
            Container.Bind<GameItemFactory>().AsSingle();

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
                .Bind<EmployeeProfitService>()
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
            Container.Bind<PlayerProvider>().AsSingle();
            Container.BindInstance(_extinguisherProvider);
            Container.BindInstance(_purchaseableItemProvider);
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
            Container.Bind<GameStaticDataService>().AsSingle();
            Container.Bind<MaterialStaticDataService>().AsSingle();
            Container.Bind<EmployeeStaticDataService>().AsSingle();
        }
    }
}