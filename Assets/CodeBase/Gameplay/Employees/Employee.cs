using System;
using System.Collections;
using CodeBase.Data;
using CodeBase.Enums;
using CodeBase.Extensions;
using CodeBase.Gameplay.BurnableObjectSystem;
using CodeBase.Gameplay.PaperS;
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
        [field: SerializeField] public float ProcessPaperTime { get; set; }

        public EmployeeTypeId EmployeeTypeId;

        public Guid Guid { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string TableId { get; set; }
        public bool IsWorking { get; set; }
        public bool IsUpgrading { get; set; }

        private bool _wasWorking;
        private EmployeeDataService _employeeDataService;
        private BurnableObjectService _burnableObjectService;
        private RendererMaterialChangerService _rendererMaterialChangerService;
        private TableService _tableService;
        private Table _table;
        private TableHolder _tableHolder;
        private bool _paperProcessed;

        public bool HasPapers { get; private set; }

        public event Action<Employee> UpgradeStarted;
        public event Action<IBurnable> Burned;
        public event Action<Employee> PaperAdded;

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
            _table.Burned += Burn;
            _tableHolder = _table.GetComponent<TableHolder>();
            _tableHolder.ItemPut += OnPaperAdded;

            if (IsBurned)
                Burn();

            StartCoroutine(ProcessPapers());
        }

        private void OnDestroy()
        {
            if (_paperProcessed)
                _employeeDataService.SetPaperProcessedOnce();

            _tableHolder.ItemPut -= OnPaperAdded;
            _table.Burned -= Burn;
        }

        private void OnPaperAdded()
        {
            PaperAdded?.Invoke(this);
            HasPapers = true;
        }

        private IEnumerator ProcessPapers()
        {
            var littleDelay = new WaitForSeconds(0.1f);

            while (true)
            {
                yield return littleDelay;

                if (_tableHolder.ItemsCount > 0)
                {
                    if (!_tableHolder.IsItemAdding)
                    {
                        HasPapers = true;
                        
                        yield return new WaitForSeconds(ProcessPaperTime);

                        yield return new WaitUntil(() => _tableHolder.CanTakePapers);
                        
                        Paper paper = _tableHolder.Take(transform);

                        yield return paper?.transform.DOLocalJump(Vector3.zero, 1, 1, 0.5f)
                            .AsyncWaitForCompletion().AsUniTask().ToCoroutine();

                        yield return paper?.transform.DOScale(Vector3.zero, 0.2f).AsyncWaitForCompletion().AsUniTask()
                            .ToCoroutine();

                        paper?.Destroy();
                        _table.ResourceCreator.Create();
                        _paperProcessed = true;
                    }
                }
                else
                {
                    HasPapers = false;
                }
            }
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

        [Button]
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

        private void Burn(IBurnable obj)
        {
            if (!IsBurned)
                Burn();
        }
    }
}