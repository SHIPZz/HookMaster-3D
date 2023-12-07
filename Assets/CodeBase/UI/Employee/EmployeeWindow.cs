using System.Linq;
using CodeBase.Data;
using CodeBase.Gameplay.TableSystem;
using CodeBase.Services.Employee;
using CodeBase.Services.Factories.UI;
using CodeBase.Services.Providers.Tables;
using CodeBase.Services.WorldData;
using TMPro;
using UnityEngine;
using Zenject;

namespace CodeBase.UI.Employee
{
    public class EmployeeWindow : WindowBase
    {
        [SerializeField] private Transform _parent;
        [SerializeField] private TMP_Text _noAvailableEmployeesText;
        [SerializeField] private TMP_Text _tableCountText;

        private TableService _tableService;
        private UIFactory _uiFactory;
        private EmployeeHirerService _employeeHirerService;
        private IWorldDataService _worldDataService;

        [Inject]
        private void Construct(TableService tableService,
            UIFactory uiFactory,
            EmployeeHirerService employeeHirerService,
            IWorldDataService worldDataService)
        {
            _worldDataService = worldDataService;
            _employeeHirerService = employeeHirerService;
            _uiFactory = uiFactory;
            _tableService = tableService;
            _tableService.AllTablesBusy += OnAllTablesBusy;
            _tableService.TableConditionChanged += OnTableConditionChanged;
        }

        private void OnDisable()
        {
            _tableService.AllTablesBusy -= OnAllTablesBusy;
            _tableService.TableConditionChanged -= OnTableConditionChanged;
        }

        public override void Open()
        {
            if (!HasFreeTables())
            {
                _noAvailableEmployeesText.gameObject.SetActive(true);
                return;
            }

            SetEmployeeViews();

            _parent.gameObject.SetActive(true);
            _tableCountText.text = $"{_tableService.AvailableTableCount}/{_tableService.AllTableCount}";
            _noAvailableEmployeesText.gameObject.SetActive(false);
        }

        public override void Close()
        {
            _employeeHirerService.ActivateCreatedEmployees();
            Destroy(gameObject);
        }

        private void OnTableConditionChanged()
        {
            _tableCountText.text = $"{_tableService.AvailableTableCount}/{_tableService.AllTableCount}";
        }

        private void OnAllTablesBusy()
        {
            _noAvailableEmployeesText.gameObject.SetActive(true);
            _tableCountText.text = $"{_tableService.AvailableTableCount}/{_tableService.AllTableCount}";
            _parent.gameObject.SetActive(false);
        }

        private void SetEmployeeViews()
        {
            if (_worldDataService.WorldData.PotentialEmployeeList.Count == 0)
            {
                foreach (Table table in _tableService.Tables.Where(table => table.IsFree))
                {
                    _uiFactory.CreateEmployeeView(_parent);
                }

                return;
            }

            foreach (EmployeeData potentialEmployeeData in _worldDataService.WorldData.PotentialEmployeeList)
            {
                EmployeeView employeeView = _uiFactory.CreateDefaultEmployeeView(_parent);
                employeeView.SetInfo(potentialEmployeeData);
            }
        }

        private bool HasFreeTables() => 
            _tableService.Tables.Any(x => x.IsFree);
    }
}