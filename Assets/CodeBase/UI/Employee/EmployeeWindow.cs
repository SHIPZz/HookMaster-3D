using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Data;
using CodeBase.Gameplay.TableSystem;
using CodeBase.Services.Factories.Employee;
using CodeBase.Services.Factories.UI;
using CodeBase.Services.Providers.Tables;
using CodeBase.Services.SaveSystem;
using TMPro;
using UnityEngine;
using Zenject;

namespace CodeBase.UI
{
    public class EmployeeWindow : WindowBase
    {
        [SerializeField] private Transform _parent;
        [SerializeField] private TMP_Text _noAvailableEmployeesText;

        private TableService _tableService;
        private UIFactory _uiFactory;
        private ISaveSystem _saveSystem;
        public event Action Destroyed;

        [Inject]
        private void Construct(TableService tableService, UIFactory uiFactory, ISaveSystem saveSystem)
        {
            _saveSystem = saveSystem;
            _uiFactory = uiFactory;
            _tableService = tableService;
            _tableService.AllTablesBusy += OnAllTablesBusy;
        }

        private async void OnDestroy()
        {
            WorldData worldData = await _saveSystem.Load();
            worldData.LastPotentialEmployeeData = null; 
            _saveSystem.Save(worldData);
            Destroyed?.Invoke();
        }

        private void OnAllTablesBusy()
        {
            _noAvailableEmployeesText.gameObject.SetActive(true);
            _parent.gameObject.SetActive(false);
        }

        public override async void Open()
        {
            if (!_tableService.Tables.Any(x => x.IsFree))
            {
                _noAvailableEmployeesText.gameObject.SetActive(true);
                return;
            }

            foreach (Table table in _tableService.Tables.Where(table => table.IsFree))
            {
                await _uiFactory.CreateEmployeeView(_parent);
            }

            _parent.gameObject.SetActive(true);
            _noAvailableEmployeesText.gameObject.SetActive(false);
        }

        public override void Close() { }
    }
}