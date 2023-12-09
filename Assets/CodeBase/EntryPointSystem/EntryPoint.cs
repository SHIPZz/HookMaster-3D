using System.Linq;
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
using CodeBase.Services.Profit;
using CodeBase.Services.Providers.Camera;
using CodeBase.Services.Providers.EmployeeProvider;
using CodeBase.Services.Providers.Location;
using CodeBase.Services.Providers.Player;
using CodeBase.Services.Providers.Tables;
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
        private readonly EmployeeProvider _employeeProvider;
        private readonly IEmployeeFactory _employeeFactory;
        private readonly IWorldDataService _worldDataService;
        private readonly TableService _tableService;
        private readonly WalletService _walletService;
        private readonly EmployeeSalaryService _employeeSalaryService;
        private readonly ProfitService _profitService;

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
            ProfitService profitService)
        {
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
            Player player = _playerFactory.Create(PlayerTypeId.Wolverine, _locationProvider.PlayerSpawnPoint);
            InitializeCamera(player);
            InitEmployees();
            InitTableService();
            _walletService.Init();
            _employeeSalaryService.Init();
            _profitService.Init();
            _playerProvider.Player = player;
        }

        private void InitTableService() =>
            _tableService.Init(_worldDataService.WorldData.TableData.BusyTableIds);

        private void InitEmployees()
        {
            PlayerData playerData = _worldDataService.WorldData.PlayerData;

            foreach (EmployeeData employeeData in playerData.PurchasedEmployees)
            {
                Table targetTable = _tableService.Tables.FirstOrDefault(x => x.Id == employeeData.TableId);
                Employee targetEmployee = _employeeFactory.Create(employeeData, targetTable.transform.position);
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