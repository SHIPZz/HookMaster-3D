using System.Linq;
using AmazingAssets.AdvancedDissolve;
using CodeBase.Data;
using CodeBase.Enums;
using CodeBase.Gameplay.Camera;
using CodeBase.Gameplay.Employees;
using CodeBase.Gameplay.PlayerSystem;
using CodeBase.Gameplay.TableSystem;
using CodeBase.Gameplay.Tutorial;
using CodeBase.Gameplay.Wallet;
using CodeBase.Services.Camera;
using CodeBase.Services.Clients;
using CodeBase.Services.Employees;
using CodeBase.Services.Extinguisher;
using CodeBase.Services.Factories.Camera;
using CodeBase.Services.Factories.Employee;
using CodeBase.Services.Factories.Player;
using CodeBase.Services.Fire;
using CodeBase.Services.Mining;
using CodeBase.Services.Profit;
using CodeBase.Services.Providers.Camera;
using CodeBase.Services.Providers.Location;
using CodeBase.Services.Providers.Player;
using CodeBase.Services.Providers.Tables;
using CodeBase.Services.PurchaseableItemServices;
using CodeBase.Services.RandomItems;
using CodeBase.Services.ShopItemData;
using CodeBase.Services.Sound;
using CodeBase.Services.UI;
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
        private readonly CameraProvider _cameraProvider;
        private readonly PlayerProvider _playerProvider;
        private readonly EmployeeService _employeeService;
        private readonly IEmployeeFactory _employeeFactory;
        private readonly IWorldDataService _worldDataService;
        private readonly TableService _tableService;
        private readonly WalletService _walletService;
        private readonly EmployeeSalaryService _employeeSalaryService;
        private readonly ProfitService _profitService;
        private readonly UIService _uiService;
        private readonly GameItemService _gameItemService;
        private readonly ExtinguisherService _extinguisherService;
        private readonly FireService _fireService;
        private readonly SettingsService _settingsService;
        private readonly PurchaseableItemService _purchaseableItemService;
        private readonly ClientObjectService _clientObjectService;
        private readonly TutorialRunner _tutorialRunner;
        private readonly CameraService _cameraService;

        public EntryPoint(LocationProvider locationProvider,
            IPlayerFactory playerFactory,
            ICameraFactory cameraFactory,
            CameraProvider cameraProvider,
            PlayerProvider playerProvider,
            EmployeeService employeeService,
            IEmployeeFactory employeeFactory,
            IWorldDataService worldDataService, 
            TableService tableService, 
            WalletService walletService, 
            EmployeeSalaryService employeeSalaryService,
            ProfitService profitService, 
            UIService uiService, 
            GameItemService gameItemService, 
            ExtinguisherService extinguisherService,
            FireService fireService,
            SettingsService settingsService,
            PurchaseableItemService purchaseableItemService,
            ClientObjectService clientObjectService, 
            TutorialRunner tutorialRunner,
            CameraService cameraService)
        {
            _cameraService = cameraService;
            _tutorialRunner = tutorialRunner;
            _clientObjectService = clientObjectService;
            _purchaseableItemService = purchaseableItemService;
            _settingsService = settingsService;
            _fireService = fireService;
            _extinguisherService = extinguisherService;
            _locationProvider = locationProvider;
            _playerFactory = playerFactory;
            _cameraFactory = cameraFactory;
            _cameraProvider = cameraProvider;
            _playerProvider = playerProvider;
            _employeeService = employeeService;
            _employeeFactory = employeeFactory;
            _worldDataService = worldDataService;
            _tableService = tableService;
            _walletService = walletService;
            _employeeSalaryService = employeeSalaryService;
            _profitService = profitService;
            _uiService = uiService;
            _gameItemService = gameItemService;
        }

        public void Initialize()
        {
            InitTutorialRunner();
            Player player = _playerFactory.Create(CharacterTypeId.Boss, _locationProvider.PlayerSpawnPoint);
            InitializeCamera(player);
            InitCameraService();
            InitTableService();
            InitEmployees();
            SetRefreshRate();
            InitWalletService();
            InitEmployeeSalaryService();
            InitProfitService();
            InitUIService();
            InitPlayerProvider(player);
            InitShopItemService();
            InitExtinguisherService();
            InitFireService();
            InitSettingsService();
            InitPurchaseableItemService();
            InitClientObjectService();
        }

        private void InitCameraService() => 
            _cameraService.Init();

        private void InitTutorialRunner() => 
            _tutorialRunner.Init();

        private void InitClientObjectService() => 
            _clientObjectService.Init();

        private void InitPurchaseableItemService() => 
            _purchaseableItemService.Init();

        private void InitSettingsService() => 
            _settingsService.Init();

        private void InitFireService() => 
            _fireService.Init();

        private void InitExtinguisherService() => 
            _extinguisherService.Init();

        private void InitShopItemService() => 
            _gameItemService.Init();

        private void InitPlayerProvider(Player player) => 
            _playerProvider.Player = player;

        private void InitWalletService() =>
            _walletService.Init();

        private void InitEmployeeSalaryService() =>
            _employeeSalaryService.Init();

        private void InitProfitService() =>
            _profitService.Init();

        private void InitUIService() =>
            _uiService.Init(_cameraProvider.Camera);

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

            foreach (EmployeeData employeeData in playerData.PurchasedEmployees)
            {
                Table targetTable = _tableService.Tables.FirstOrDefault(x => x.Id == employeeData.TableId);

                if (targetTable == null)
                    continue;

                Employee targetEmployee = _employeeFactory.Create(employeeData, targetTable, true);
                _employeeService.Employees.Add(targetEmployee);
            }

            _employeeService.Initialized = true;
        }

        private void InitializeCamera(Player player)
        {
            CameraFollower cameraFollower = _cameraFactory.Create();
            cameraFollower.SetTarget(player.transform);
            _cameraProvider.CameraFollower = cameraFollower;
            _cameraProvider.Camera = cameraFollower.GetComponent<Camera>();
        }
    }
}