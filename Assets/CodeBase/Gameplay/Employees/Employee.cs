using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CodeBase.Data;
using CodeBase.Enums;
using CodeBase.Extensions;
using CodeBase.Gameplay.BurnableObjectSystem;
using CodeBase.Gameplay.PaperSystem;
using CodeBase.Gameplay.TableSystem;
using CodeBase.MaterialChanger;
using CodeBase.Services.BurnableObjects;
using CodeBase.Services.Employees;
using CodeBase.Services.Providers.Tables;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.Employees
{
    public class Employee : MonoBehaviour, IBurnable
    {
        [field: SerializeField] public MaterialTypeId BurnMaterial { get; private set; }
        [field: SerializeField] public SkinnedMeshRenderer Renderer { get; private set; }
        [field: SerializeField] public bool IsBurned { get; set; }
        [SerializeField] private float _processPaperTime = 5f;

        public EmployeeTypeId EmployeeTypeId;
        public Guid Guid;
        public string Id;
        public int QualificationType;
        public int Salary;
        public int Profit;
        public string Name;
        public string TableId;
        public bool IsWorking;
        public bool IsUpgrading;

        private bool _wasWorking;
        private EmployeeDataService _employeeDataService;
        private BurnableObjectService _burnableObjectService;
        private RendererMaterialChangerService _rendererMaterialChangerService;
        private TableService _tableService;
        private CancellationTokenSource _cancellationToken = new();
        private Stack<Paper> _papers = new();
        private Table _table;

        public bool HasPapers => _papers.Count > 0;

        public event Action<Employee> UpgradeStarted;
        public event Action<Employee> Burned;
        public event Action<Employee> PaperAdded;
        public event Action<Employee> AllPaperProcessed;

        [Inject]
        private void Construct(EmployeeDataService employeeDataService,
            BurnableObjectService burnableObjectService,
            RendererMaterialChangerService rendererMaterialChangerService,
            TableService tableService)
        {
            _tableService = tableService;
            _rendererMaterialChangerService = rendererMaterialChangerService;
            _burnableObjectService = burnableObjectService;
            _employeeDataService = employeeDataService;
        }

        private void Start()
        {
            _burnableObjectService.Add(this);
            _rendererMaterialChangerService.Init(1.5f, 1f, BurnMaterial, Renderer);
            _table = _tableService.Get(TableId);
            
            if (IsBurned)
                Burn();
        }

        public void StartWorking()
        {
            IsWorking = true;
            _employeeDataService.OverwritePurchasedEmployeeData(this.ToEmployeeData());
        }

        public void SetUpgrading(bool isUpgrading)
        {
            IsUpgrading = isUpgrading;
            UpgradeStarted?.Invoke(this);
            _employeeDataService.OverwritePurchasedEmployeeData(this.ToEmployeeData());
        }

        public void StopWorking()
        {
            IsWorking = false;
            _employeeDataService.OverwritePurchasedEmployeeData(this.ToEmployeeData());
        }

        public void Burn()
        {
            _wasWorking = IsWorking;
            IsBurned = true;
            _rendererMaterialChangerService.Change();
            Burned?.Invoke(this);
            StopWorking();
        }

        [Button]
        public void Recover()
        {
            IsBurned = false;
            _rendererMaterialChangerService.SetInitialMaterial();
            _tableService.RecoverTableFromBurning(TableId);

            if (_wasWorking)
                StartWorking();
        }
    }
}