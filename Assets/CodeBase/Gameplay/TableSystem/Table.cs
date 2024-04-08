using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Data;
using CodeBase.Gameplay.BurnableObjectSystem;
using CodeBase.Gameplay.ObjectCreatorSystem;
using CodeBase.Gameplay.PaperSystem;
using CodeBase.MaterialChanger;
using CodeBase.Services.BurnableObjects;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.TableSystem
{
    public class Table : MonoBehaviour, IBurnable
    {
        [field: SerializeField] public MaterialTypeId BurnMaterial { get; private set; }
        [field: SerializeField] public bool IsBurned { get; set; }

        [SerializeField] private List<UnityEngine.Renderer> _childRenderers;
        [SerializeField] private MeshRenderer _meshRenderer;
        [field: SerializeField] public ResourceCreator ResourceCreator { get; private set; }

        public bool IsFree;
        public Transform Chair;
        public string Id;

        private ChildRendererMaterialChangerService _rendererMaterialChangerService;
        private BurnableObjectService _burnableObjectService;
        private bool _wasFree;

        public event Action<bool, string> Busy;
        public event Action<IBurnable> Burned;

        [Inject]
        private void Construct(ChildRendererMaterialChangerService childRendererMaterial,
            BurnableObjectService burnableObjectService)
        {
            _burnableObjectService = burnableObjectService;
            _rendererMaterialChangerService = childRendererMaterial;
        }

        public void Init()
        {
            _rendererMaterialChangerService.Init(1.5f, 1f, BurnMaterial, _meshRenderer, _childRenderers);
            _burnableObjectService.Add(this);

            if (IsBurned)
                Burn();
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
            Burned?.Invoke(this);
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