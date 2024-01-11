using System.Linq;
using AmazingAssets.AdvancedDissolve;
using CodeBase.Data;
using CodeBase.Enums;
using CodeBase.Gameplay.Camera;
using CodeBase.Gameplay.EmployeeSystem;
using CodeBase.Gameplay.PlayerSystem;
using CodeBase.Gameplay.TableSystem;
using CodeBase.Gameplay.Wallet;
using CodeBase.Services.Employee;
using CodeBase.Services.Extinguisher;
using CodeBase.Services.Factories.Camera;
using CodeBase.Services.Factories.Employee;
using CodeBase.Services.Factories.Player;
using CodeBase.Services.Fire;
using CodeBase.Services.MiningFarm;
using CodeBase.Services.Profit;
using CodeBase.Services.Providers.Camera;
using CodeBase.Services.Providers.EmployeeProvider;
using CodeBase.Services.Providers.Location;
using CodeBase.Services.Providers.Player;
using CodeBase.Services.Providers.Tables;
using CodeBase.Services.RandomItems;
using CodeBase.Services.ShopItemData;
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
        private readonly ShopItemService _shopItemService;
        private readonly MiningFarmService _miningFarmService;
        private readonly RandomItemService _randomItemService;
        private readonly ExtinguisherService _extinguisherService;
        private readonly FireService _fireService;

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
            ShopItemService shopItemService, 
            MiningFarmService miningFarmService, 
            RandomItemService randomItemService,
            ExtinguisherService extinguisherService,
            FireService fireService)
        {
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
            _shopItemService = shopItemService;
            _miningFarmService = miningFarmService;
            _randomItemService = randomItemService;
        }

        public void Initialize()
        {
            Player player = _playerFactory.Create(CharacterTypeId.Boss, _locationProvider.PlayerSpawnPoint);
            InitializeCamera(player);
            InitTableService();
            InitEmployees();
            SetRefreshRate();
            InitWalletService();
            InitEmployeeSalaryService();
            InitProfitService();
            InitUIService();
            InitPlayerProvider(player);
            InitShopItemService();
            InitMiningFarmService();
            InitRandomItemService();
            InitExtinguisherService();
            InitFireService();
        }

        private void InitFireService() => 
            _fireService.Init();

        private void InitExtinguisherService() => 
            _extinguisherService.Init();

        private void InitRandomItemService() => 
            _randomItemService.Init();

        private void InitMiningFarmService() => 
            _miningFarmService.Init();

        private void InitShopItemService() => 
            _shopItemService.Init();

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
            _tableService.Init(_worldDataService.WorldData.TableData.BusyTableIds);

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