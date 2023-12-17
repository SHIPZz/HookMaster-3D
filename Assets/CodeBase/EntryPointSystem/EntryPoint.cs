using System.Linq;
using CodeBase.Constant;
using CodeBase.Data;
using CodeBase.Enums;
using CodeBase.Extensions;
using CodeBase.Gameplay.Camera;
using CodeBase.Gameplay.EmployeeSystem;
using CodeBase.Gameplay.PlayerSystem;
using CodeBase.Gameplay.TableSystem;
using CodeBase.Gameplay.Wallet;
using CodeBase.Services.EmployeeSalary;
using CodeBase.Services.Factories.Camera;
using CodeBase.Services.Factories.Employee;
using CodeBase.Services.Factories.Player;
using CodeBase.Services.Factories.UI;
using CodeBase.Services.Profit;
using CodeBase.Services.Providers.Camera;
using CodeBase.Services.Providers.EmployeeProvider;
using CodeBase.Services.Providers.Location;
using CodeBase.Services.Providers.Player;
using CodeBase.Services.Providers.Tables;
using CodeBase.Services.UI;
using CodeBase.Services.Window;
using CodeBase.Services.WorldData;
using CodeBase.UI.Hud;
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
        private readonly EmployeeProvider _employeeProvider;
        private readonly IEmployeeFactory _employeeFactory;
        private readonly IWorldDataService _worldDataService;
        private readonly TableService _tableService;
        private readonly WalletService _walletService;
        private readonly EmployeeSalaryService _employeeSalaryService;
        private readonly ProfitService _profitService;
        private readonly UIService _uiService;
        private int _targetFrameRate;

        public EntryPoint(LocationProvider locationProvider,
            IPlayerFactory playerFactory,
            ICameraFactory cameraFactory,
            CameraProvider cameraProvider,
            PlayerProvider playerProvider,
            EmployeeProvider employeeProvider,
            IEmployeeFactory employeeFactory,
            IWorldDataService worldDataService,
            TableService tableService,
            WalletService walletService,
            EmployeeSalaryService employeeSalaryService,
            ProfitService profitService,
            UIService uiService)
        {
            _uiService = uiService;
            _profitService = profitService;
            _employeeSalaryService = employeeSalaryService;
            _walletService = walletService;
            _tableService = tableService;
            _worldDataService = worldDataService;
            _employeeFactory = employeeFactory;
            _employeeProvider = employeeProvider;
            _playerProvider = playerProvider;
            _cameraProvider = cameraProvider;
            _locationProvider = locationProvider;
            _playerFactory = playerFactory;
            _cameraFactory = cameraFactory;
        }

        public void Initialize()
        {
            Player player = _playerFactory.Create(CharacterTypeId.Boss, _locationProvider.PlayerSpawnPoint);
            InitializeCamera(player);
            InitEmployees();
            InitTableService();
            SetRefreshRate();
            InitWalletService();
            InitEmployeeSalaryService();
            InitProfitService();
            InitUIService();
            InitPlayerProvider(player);
        }

        private void InitPlayerProvider(Player player)
        {
            _playerProvider.Player = player;
        }

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

                Employee targetEmployee = _employeeFactory.Create(employeeData, targetTable);
                _employeeProvider.Employees.Add(targetEmployee);
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