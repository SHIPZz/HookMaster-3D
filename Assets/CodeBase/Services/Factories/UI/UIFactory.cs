using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Constant;
using CodeBase.Data;
using CodeBase.Services.DataService;
using CodeBase.Services.Factories.Employee;
using CodeBase.Services.Providers.Asset;
using CodeBase.Services.Providers.Location;
using CodeBase.Services.WorldData;
using CodeBase.UI;
using CodeBase.UI.Employee;
using CodeBase.UI.Hud;
using TMPro;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

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

        public EmployeeView CreateEmployeeView(Transform parent)
        {
            var employeeFactory = _diContainer.Resolve<IEmployeeFactory>();
            var worldDataService = _diContainer.Resolve<IWorldDataService>();
            CodeBase.Data.WorldData worldData = worldDataService.WorldData;
            EmployeeData employeeData = employeeFactory.Create();

            var employeeViewPrefab = _assetProvider.Get<EmployeeView>(AssetPath.EmployeeView);
            var employeeView = _diContainer.InstantiatePrefabForComponent<EmployeeView>(employeeViewPrefab, parent);
            employeeView.SetInfo(employeeData);

            worldData.PotentialEmployeeList.Add(employeeData);
            worldDataService.Save();

            return employeeView;
        }

        public EmployeeView CreateDefaultEmployeeView(Transform parent)
        {
            var employeeViewPrefab = _assetProvider.Get<EmployeeView>(AssetPath.EmployeeView);
            return _diContainer.InstantiatePrefabForComponent<EmployeeView>(employeeViewPrefab, parent);
        }

        public T CreateWindow<T>() where T : WindowBase
        {
            WindowBase windowPrefab = _uiStaticDataService.Get<T>();
            var employeeWindow =
                _diContainer.InstantiatePrefabForComponent<T>(windowPrefab, _locationProvider.UIParent);
            return employeeWindow;
        }
        
        public TMP_Text CreateText(string path, Transform parent)
        {
            var targetText = _assetProvider.Get<TMP_Text>(path);
            return  _diContainer.InstantiatePrefabForComponent<TMP_Text>(targetText, parent);
        }

        public T CreateElement<T>(string path, Transform parent) where T : Object
        {
            var targetElement = _assetProvider.Get<T>(path);
            return  _diContainer.InstantiatePrefabForComponent<T>(targetElement, parent);
        }
    }
}