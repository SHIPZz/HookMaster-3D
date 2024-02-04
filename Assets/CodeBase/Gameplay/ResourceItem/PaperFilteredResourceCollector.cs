using System;
using System.Collections.Generic;
using CodeBase.Enums;
using CodeBase.Gameplay.PaperSystem;
using CodeBase.Gameplay.PlayerSystem;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.ResourceItem
{
    public class PaperFilteredResourceCollector : ResourceCollector
    {
        [SerializeField] private Vector3 _offset;
        [SerializeField] private PlayerPaperContainer _playerPaperContainer;
        [SerializeField] private Transform _leftHandCarring;
        [SerializeField] private Transform _rightHandCarring;

        private Resource _lastResource;
        private PlayerIKService _playerIKService;

        [Inject]
        private void Construct(PlayerIKService playerIKService)
        {
            _playerIKService = playerIKService;
        }

        private void Start()
        {
            _playerPaperContainer.Cleared += Reset;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _playerPaperContainer.Cleared -= Reset;
        }

        protected override async void OnResourceEnter(Collider other)
        {
            base.OnResourceEnter(other);
            
            if(!other.gameObject.TryGetComponent(out Resource resource))
                return;
            
            if(resource.GameItemType != GameItemType.Paper)
                return;
            
            while (!resource.IsCollected)
            {
                await UniTask.Yield();
            }
            
            _playerIKService.SetIKHandTargets(_leftHandCarring, _rightHandCarring);
            _playerPaperContainer.Push(resource.GetComponent<Paper>());
            
            if (_lastResource != null)
                resource.transform.localPosition = _lastResource.transform.localPosition + _offset;
            
            _lastResource = resource;
        }

        private void Reset()
        {
            _lastResource = null;
        }
    }
}