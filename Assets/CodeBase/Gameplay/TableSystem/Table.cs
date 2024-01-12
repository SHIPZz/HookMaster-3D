using System;
using System.Collections.Generic;
using CodeBase.Data;
using CodeBase.Gameplay.BurnableObjectSystem;
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

        public bool IsFree;
        public Transform Chair;
        public string Id;

        private ChildRendererMaterialChangerService _rendererMaterialChangerService;
        private BurnableObjectService _burnableObjectService;
        [SerializeField] private MeshRenderer _meshRenderer;
        private bool _wasFree;

        public event Action<bool, string> Busy;

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
            _wasFree = IsFree;
            IsFree = false;
            IsBurned = true;
            Busy?.Invoke(false, Id);
            _rendererMaterialChangerService.Change();
        }

        [Button]
        public void Recover()
        {
            IsBurned = false;
            Busy?.Invoke(_wasFree, Id);
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