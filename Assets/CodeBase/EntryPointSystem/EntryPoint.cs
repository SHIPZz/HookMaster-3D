﻿using CodeBase.Data;
using CodeBase.Enums;
using CodeBase.Gameplay.PlayerSystem;
using CodeBase.Gameplay.Tutorial;
using CodeBase.Services.Clients;
using CodeBase.Services.Employees;
using CodeBase.Services.Extinguisher;
using CodeBase.Services.Factories.Camera;
using CodeBase.Services.Factories.Employee;
using CodeBase.Services.Factories.Player;
using CodeBase.Services.Fire;
using CodeBase.Services.GameItemServices;
using CodeBase.Services.Profit;
using CodeBase.Services.Providers.Location;
using CodeBase.Services.Providers.Player;
using CodeBase.Services.Providers.Tables;
using CodeBase.Services.PurchaseableItemServices;
using CodeBase.Services.Sound;
using CodeBase.Services.UI;
using CodeBase.Services.Wallet;
using CodeBase.Services.WorldData;
using UnityEngine;
using Zenject;

namespace CodeBase.EntryPointSystem
{
    public class EntryPoint : IInitializable
    {
        private readonly LocationProvider _locationProvider;
        private readonly IPlayerFactory _playerFactory;
        private readonly ICameraFactory _cameraFactory;
        private readonly PlayerProvider _playerProvider;
        private readonly EmployeeService _employeeService;
        private readonly IEmployeeFactory _employeeFactory;
        private readonly IWorldDataService _worldDataService;
        private readonly TableService _tableService;
        private readonly WalletService _walletService;
        private readonly EmployeeProfitService _employeeProfitService;
        private readonly UIService _uiService;
        private readonly GameItemService _gameItemService;
        private readonly FireService _fireService;
        private readonly SettingsService _settingsService;
        private readonly PurchaseableItemService _purchaseableItemService;
        private readonly ClientObjectService _clientObjectService;
        private readonly TutorialRunner _tutorialRunner;
        private ExtinguisherService _extinguisherService;

        public EntryPoint(LocationProvider locationProvider,
            IPlayerFactory playerFactory,
            ICameraFactory cameraFactory,
            PlayerProvider playerProvider,
            EmployeeService employeeService,
            IEmployeeFactory employeeFactory,
            IWorldDataService worldDataService,
            TableService tableService,
            WalletService walletService,
            EmployeeProfitService employeeProfitService,
            UIService uiService,
            GameItemService gameItemService,
            FireService fireService,
            SettingsService settingsService,
            PurchaseableItemService purchaseableItemService,
            ClientObjectService clientObjectService,
            TutorialRunner tutorialRunner,
            ExtinguisherService extinguisherService)
        {
            _extinguisherService = extinguisherService;
            _tutorialRunner = tutorialRunner;
            _clientObjectService = clientObjectService;
            _purchaseableItemService = purchaseableItemService;
            _settingsService = settingsService;
            _fireService = fireService;
            _locationProvider = locationProvider;
            _playerFactory = playerFactory;
            _cameraFactory = cameraFactory;
            _playerProvider = playerProvider;
            _employeeService = employeeService;
            _employeeFactory = employeeFactory;
            _worldDataService = worldDataService;
            _tableService = tableService;
            _walletService = walletService;
            _employeeProfitService = employeeProfitService;
            _uiService = uiService;
            _gameItemService = gameItemService;
        }

        public void Initialize()
        {
            Player player = _playerFactory.Create(CharacterTypeId.Boss, _locationProvider.PlayerSpawnPoint);
            InitPlayerProvider(player);
            InitTableService();
            InitEmployees();
            SetRefreshRate();
            InitWalletService();
            InitProfitService();
            InitPurchaseableItemService();
            InitShopItemService();
            InitTutorialRunner();
            InitExtinguisherService();
            InitFireService();
            InitUIService();
            InitSettingsService();
            InitClientObjectService();
        }

        private void InitExtinguisherService() => 
            _extinguisherService.Initialize();

        private void InitTutorialRunner() =>
            _tutorialRunner.Init();

        private void InitClientObjectService() =>
            _clientObjectService.Init();

        private void InitPurchaseableItemService() =>
            _purchaseableItemService.Init();

        private void InitSettingsService() =>
            _settingsService.Init();

        private void InitFireService() =>
            _fireService.Init().Forget();

        private void InitShopItemService() =>
            _gameItemService.Init();

        private void InitPlayerProvider(Player player) =>
            _playerProvider.Player = player;

        private void InitWalletService() =>
            _walletService.Init();

        private void InitProfitService() =>
            _employeeProfitService.Init();

        private void InitUIService()
        {
            _uiService.Init();
        }

        private void SetRefreshRate()
        {
            RefreshRate refreshRate = Screen.currentResolution.refreshRateRatio;
            Application.targetFrameRate = Mathf.RoundToInt((float)refreshRate.value);
        }

        private void InitTableService() =>
            _tableService.Init(_worldDataService.WorldData.TableDatas);

        private void InitEmployees()
        {
            PlayerData playerData = _worldDataService.WorldData.PlayerData;

            _employeeService.Init(playerData.PurchasedEmployees);
        }
    }
}