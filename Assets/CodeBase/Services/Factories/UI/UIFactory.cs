using System.Linq;
using CodeBase.Constant;
using CodeBase.Services.Data;
using CodeBase.Services.Factories.Employee;
using CodeBase.Services.Providers.Asset;
using CodeBase.Services.Providers.Location;
using CodeBase.Services.SaveSystem;
using CodeBase.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace CodeBase.Services.Factories.UI
{
    public class UIFactory
    {
        private readonly UIStaticDataService _uiStaticDataService;
        private readonly DiContainer _diContainer;
        private readonly LocationProvider _locationProvider;
        private readonly IAssetProvider _assetProvider;

        public UIFactory(UIStaticDataService uiStaticDataService,
            DiContainer diContainer,
            LocationProvider locationProvider,
            IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
            _locationProvider = locationProvider;
            _diContainer = diContainer;
            _uiStaticDataService = uiStaticDataService;
        }

        public async UniTask<EmployeeView> CreateEmployeeView(Transform parent)
        {
            var employeeFactory = _diContainer.Resolve<IEmployeeFactory>();
            var saveSystem = _diContainer.Resolve<ISaveSystem>();
            CodeBase.Data.WorldData worldData = await saveSystem.Load();
            PotentialEmployeeData potentialEmployeeData = worldData.PotentialEmployeeList
                .SkipWhile(potentialEmployee => potentialEmployee.Equals(worldData.LastPotentialEmployeeData))
                .FirstOrDefault();
            
            potentialEmployeeData ??= await employeeFactory.Create();

            if (!worldData.PotentialEmployeeList.Contains(potentialEmployeeData))
                worldData.PotentialEmployeeList.Add(potentialEmployeeData);
            
            worldData.LastPotentialEmployeeData = potentialEmployeeData;
            saveSystem.Save(worldData);

            var employeeViewPrefab = _assetProvider.Get<EmployeeView>(AssetPath.EmployeeView);
            var employeeView = _diContainer.InstantiatePrefabForComponent<EmployeeView>(employeeViewPrefab, parent);
            employeeView.SetInfo(potentialEmployeeData);
            return employeeView;
        }

        public EmployeeWindow CreateEmployeeWindow()
        {
            WindowBase windowPrefab = _uiStaticDataService.Get<EmployeeWindow>();
            var employeeWindow =
                _diContainer.InstantiatePrefabForComponent<EmployeeWindow>(windowPrefab, _locationProvider.UIParent);
            return employeeWindow;
        }
    }
}