using System;
using System.Collections.Generic;
using CodeBase.Data;
using CodeBase.Gameplay.BurnableObjectSystem;
using CodeBase.Gameplay.ObjectCreatorSystem;
using CodeBase.Gameplay.PaperSystem;
using CodeBase.MaterialChanger;
using CodeBase.Services.BurnableObjects;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.TableSystem
{
    public class Table : MonoBehaviour, IBurnable
    {
        [field: SerializeField] public MaterialTypeId BurnMaterial { get; private set; }
        [field: SerializeField] public bool IsBurned { get; set; }

        [field: SerializeField] public Transform PaperPosition { get; private set; }
        [field: SerializeField] public ResourceCreator ResourceCreator { get; private set; }
        [field: SerializeField] public Transform PaperFinishedPosition { get; private set; }
        [field: SerializeField] public Vector3 Offset { get; private set; } = new Vector3(0, 0.06f, 0);

        [SerializeField] private List<UnityEngine.Renderer> _childRenderers;
        [SerializeField] private MeshRenderer _meshRenderer;

        public bool IsFree;
        public Transform Chair;
        public string Id;
        public Stack<Paper> PapersOnTable = new();
        public Paper LastPaper { get; private set; }

        private ChildRendererMaterialChangerService _rendererMaterialChangerService;
        private BurnableObjectService _burnableObjectService;
        private bool _wasFree;

        public event Action<bool, string> Busy;
        public event Action<Table> PaperAdded;
        public event Action<Table> AllPaperProcessed;

        [Inject]
        private void Construct(ChildRendererMaterialChangerService childRendererMaterial,
            BurnableObjectService burnableObjectService)
        {
            _burnableObjectService = burnableObjectService;
            _rendererMaterialChangerService = childRendererMaterial;
        }

        public void Add(Paper paper)
        {
            PapersOnTable.Push(paper);
            LastPaper = PapersOnTable.Peek();
            PaperAdded?.Invoke(this);
        }

        public void ClearPapers()
        {
            LastPaper = null;
            PapersOnTable.Clear();
        }

        public Paper PopPaper()
        {
            Paper paper = PapersOnTable.Pop();
            
            if (PapersOnTable.Count == 0)
                AllPaperProcessed?.Invoke(this);

            LastPaper = PapersOnTable.Peek();
            return paper;
        }

        public void Init()
        {
            _rendererMaterialChangerService.Init(1.5f, 1f, BurnMaterial, _meshRenderer, _childRenderers);
            _burnableObjectService.Add(this);

            if (IsBurned)
                Burn();
        }

        public void SetIsBurned(bool isBurned)
        {
            IsBurned = isBurned;
        }

        public void SetIsFree(bool isFree)
        {
            IsFree = isFree;
            Busy?.Invoke(IsFree, Id);
        }

        [Button]
        public void Burn()
        {
            if (IsFree)
                return;

            IsBurned = true;
            _rendererMaterialChangerService.Change();
        }

        [Button]
        public void Recover()
        {
            IsBurned = false;
            _rendererMaterialChangerService.SetInitialMaterial();
        }

        [ContextMenu("CreateGuid")]
        public void CreateGuid()
        {
            var guid = Guid.NewGuid();
            Id = guid.ToString();
        }
    }
}