using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using CodeBase.Data;
using CodeBase.Enums;
using CodeBase.Extensions;
using CodeBase.Gameplay.BurnableObjectSystem;
using CodeBase.Gameplay.PaperS;
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
        private Table _table;
        private TableHolder _tableHolder;
        private bool _isProcessingPaper;
        private Coroutine _coroutine;

        public bool HasPapers { get; private set; }

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
            _tableHolder = _table.GetComponent<TableHolder>();
            _tableHolder.ItemPut += OnPaperAdded;

            if (IsBurned)
                Burn();
        }

        private void OnDestroy()
        {
            _tableHolder.ItemPut -= OnPaperAdded;
            _cancellationToken?.Cancel();
            _cancellationToken?.Dispose();
        }

        private void OnPaperAdded()
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _cancellationToken?.Cancel();
            }
            
            PaperAdded?.Invoke(this);
            HasPapers = true;
            _coroutine = StartCoroutine(StartProcessPapersCoroutine());
        }

        private IEnumerator StartProcessPapersCoroutine()
        {
            _cancellationToken?.Dispose();
            _cancellationToken = new();
            yield return ProcessPapers();
        }

        private async UniTask ProcessPapers()
        {
            _cancellationToken.Token.ThrowIfCancellationRequested();
            
            while (_tableHolder.ItemsCount > 0)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(_processPaperTime)).AttachExternalCancellation(_cancellationToken.Token);
                IHoldable paper = await _tableHolder.TakeAsync(transform, _cancellationToken.Token);
                paper.Transform.DOScale(Vector3.zero, 0.5f).OnComplete(() =>
                {
                    paper.Destroy();
                    _table.ResourceCreator.Create();
                });
            }

            _employeeDataService.SetPaperProcessedOnce();
            _tableHolder.SetLastHoldableNull();
            AllPaperProcessed?.Invoke(this);
            HasPapers = false;
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