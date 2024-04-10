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

        private Transform _lastResourcePosition;
        private PlayerIKService _playerIKService;

        [Inject]
        private void Construct(PlayerIKService playerIKService)
        {
            _playerIKService = playerIKService;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _playerPaperContainer.Cleared += Reset;
            _playerPaperContainer.Removed += SetNewLastResource;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _playerPaperContainer.Cleared -= Reset;
        }

        protected override async void OnResourceEnter(Collider other)
        {
            base.OnResourceEnter(other);

            if (!other.gameObject.TryGetComponent(out Resource resource))
                return;

            if (resource.GameItemType != GameItemType.Paper)
                return;

            await UniTask.WaitUntil(() => resource.IsCollected);
            
            _playerIKService.SetIKHandTargets(_leftHandCarring, _rightHandCarring);
            _playerPaperContainer.Push(resource.GetComponent<Paper>());

            if (_lastResourcePosition != null)
                resource.transform.localPosition = _lastResourcePosition.transform.localPosition + _offset;

            _lastResourcePosition = resource.transform;
        }

        private void Reset()
        {
            _lastResourcePosition = null;
        }

        private void SetNewLastResource()
        {
            var lastResource = _playerPaperContainer.Peek();
            
            if (lastResource != null)
                _lastResourcePosition = lastResource.transform;
        }
    }
}