using System.Collections.Generic;
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

        private List<Resource> _collectedResources = new();
        private Resource _lastResource;
        private PlayerIKService _playerIKService;

        [Inject]
        private void Construct(PlayerIKService playerIKService)
        {
            _playerIKService = playerIKService;
        }

        protected override async void OnResourceEnter(Collider other)
        {
            base.OnResourceEnter(other);
            var resource = other.GetComponent<Resource>();

            while (!resource.IsCollected)
            {
                await UniTask.Yield();
            }

            _playerIKService.SetIKHandTargets(_leftHandCarring, _rightHandCarring);
            _playerPaperContainer.Add(resource.GetComponent<Paper>());
            
            if (_lastResource != null)
                resource.transform.localPosition = _lastResource.transform.localPosition + _offset;

            _lastResource = resource;

            _collectedResources.Add(other.GetComponent<Resource>());
        }
    }
}