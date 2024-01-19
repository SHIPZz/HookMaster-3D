using System.Collections.Generic;
using System.Linq;
using Abu;
using CodeBase.Animations;
using CodeBase.Data;
using CodeBase.Gameplay.TableSystem;
using CodeBase.Services.Employee;
using CodeBase.Services.Factories.UI;
using CodeBase.Services.Providers.Tables;
using CodeBase.Services.Window;
using CodeBase.Services.WorldData;
using CodeBase.UI.Hud;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using Zenject;

namespace CodeBase.UI.Employee
{
    public class EmployeeWindow : WindowBase
    {
        public Transform TutorialHandParent;
        public TutorialFadeImage TutorialFadeImage;

        [SerializeField] private Transform _parent;
        [SerializeField] private TMP_Text _noAvailableEmployeesText;
        [SerializeField] private TMP_Text _tableCountText;
        [SerializeField] private CanvasAnimator _canvasAnimator;

        private TableService _tableService;
        private UIFactory _uiFactory;
        private EmployeeHirerService _employeeHirerService;
        private IWorldDataService _worldDataService;
        private WindowService _windowService;
        private List<EmployeeView> _employeeViews = new();
        private bool _oneButtonHighlited;
        
        [Inject]
        private void Construct(TableService tableService,
            UIFactory uiFactory,
            EmployeeHirerService employeeHirerService,
            IWorldDataService worldDataService,
            WindowService windowService)
        {
            _windowService = windowService;
            _worldDataService = worldDataService;
            _employeeHirerService = employeeHirerService;
            _uiFactory = uiFactory;
            _tableService = tableService;
        }

        private void OnDisable()
        {
            _tableService.AllTablesBusy -= OnAllTablesBusy;
            _tableService.TableConditionChanged -= OnTableConditionChanged;
        }

        public override void Open()
        {
            _tableService.AllTablesBusy += OnAllTablesBusy;
            _tableService.TableConditionChanged += OnTableConditionChanged;
            _canvasAnimator.FadeInCanvas();
            _tableCountText.text = $"{_tableService.AvailableTableCount}/{_tableService.AllTableCount}";

            if (!HasFreeTables())
            {
                _noAvailableEmployeesText.gameObject.SetActive(true);
                return;
            }

            SetEmployeeViews();

            _parent.gameObject.SetActive(true);
            _noAvailableEmployeesText.gameObject.SetActive(false);
        }

        public override void Close()
        {
            _employeeHirerService.ActivateCreatedEmployees();
            _windowService.Open<HudWindow>();
            _canvasAnimator.FadeOutCanvas(base.Close);
        }

        public EmployeeView GetFirstEmployeeView()
        {
            return _employeeViews.FirstOrDefault();
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
                CreateNewEmployeeViews();

                return;
            }

            CreateByData();
        }

        private void CreateByData()
        {
            foreach (EmployeeData potentialEmployeeData in _worldDataService.WorldData.PotentialEmployeeList)
            {
                EmployeeView employeeView = _uiFactory.CreateDefaultEmployeeView(_parent);
                employeeView.SetInfo(potentialEmployeeData);
                SetTutorialHighlight(employeeView);
                _employeeViews.Add(employeeView);
            }
        }

        private void CreateNewEmployeeViews()
        {
            foreach (Table table in _tableService.Tables.Where(table => table.IsFree))
            {
                EmployeeView employeeView = _uiFactory.CreateEmployeeView(_parent);
                SetTutorialHighlight(employeeView);
                _employeeViews.Add(employeeView);
            }
        }

        private void SetTutorialHighlight(EmployeeView employeeView)
        {
            if(_oneButtonHighlited)
                return;
            
            employeeView.TutorialHighlight.enabled = false;
            employeeView.TutorialHighlight.tutorialFade = TutorialFadeImage;
            employeeView.TutorialHighlight.enabled = true;
            _oneButtonHighlited = true;
        }

        private bool HasFreeTables() =>
            _tableService.Tables.Any(x => x.IsFree);
    }
}