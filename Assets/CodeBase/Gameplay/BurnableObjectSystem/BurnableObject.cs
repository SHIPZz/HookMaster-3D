using System;
using System.Collections.Generic;
using CodeBase.Data;
using CodeBase.Extensions;
using CodeBase.MaterialChanger;
using CodeBase.Services.BurnableObjects;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.BurnableObjectSystem
{
    public class BurnableObject : SerializedMonoBehaviour
    {
        [field: SerializeField] public string Id { get; private set; }
        [field: SerializeField] public bool IsBurned { get; private set; }
        [OdinSerialize] private List<IMaterialChanger> _childMaterialChangers;
        [SerializeField] private bool _needChangeChild;

        private IMaterialChanger _meshMaterialChanger;
        private BurnableObjectService _burnableObjectService;

        [Inject]
        private void Construct(BurnableObjectService burnableObjectService)
        {
            _burnableObjectService = burnableObjectService;
        }

        private void Awake() =>
            _meshMaterialChanger = GetComponent<IMaterialChanger>();
        
        private void Start()
        {
            _burnableObjectService.TryAdd(this.ToData());
            BurnableItemData data  = _burnableObjectService.GetTargetData(this.ToData());
            
            print($"${data.Position.X} + {transform.position.x}");

            IsBurned = data.IsBurned;
            
            if(IsBurned)
                Burn();
        }

        public void Recover()
        {
            if (_needChangeChild)
                _childMaterialChangers.ForEach(x => x.SetInitialMaterial());

            _meshMaterialChanger.SetInitialMaterial();
            IsBurned = false;
            _burnableObjectService.SetIsBurned(this.ToData());
        }

        public void Burn()
        {
            if (_needChangeChild)
                _childMaterialChangers.ForEach(x => x.Change());

            _meshMaterialChanger.Change();
            IsBurned = true;
            _burnableObjectService.SetIsBurned(this.ToData());
        }
    }
}