using System.Threading;
using CodeBase.Gameplay.PaperSystem;
using CodeBase.Gameplay.PlayerSystem;
using CodeBase.Services.TriggerObserve;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.PaperS
{
    internal class PlayerHands : MonoBehaviour
    {
        [SerializeField] private PlayerPaperContainer _playerPaperContainer;
        [SerializeField] private TriggerObserver _triggerObserver;

        private IHolder _holder;

        private CancellationTokenSource _lifetimeTokenSource;
        private UniTask _actionTask;

        private bool _isTaking;
        private PlayerIKService _playerIKService;

        [Inject]
        private void Construct(PlayerIKService playerIKService)
        {
            _playerIKService = playerIKService;
        }

        private void OnEnable()
        {
            _lifetimeTokenSource = new CancellationTokenSource();
            _triggerObserver.TriggerEntered += OnTriggerEntered;
            _triggerObserver.TriggerExited += OnTriggerExited;
        }

        private void OnDisable()
        {
            _triggerObserver.TriggerEntered -= OnTriggerEntered;
            _triggerObserver.TriggerExited -= OnTriggerExited;
            _lifetimeTokenSource.Cancel();
            _lifetimeTokenSource.Dispose();
        }

        private void Update()
        {
            if (_holder == null)
            {
                return;
            }

            if (!_isTaking)
            {
                Put();
            }
        }

        private void Put()
        {
            if (_playerPaperContainer.Papers.Count == 0)
            {
                return;
            }

            var isTaskCompleted = _actionTask.Status.IsCompleted();

            if (!isTaskCompleted)
            {
                return;
            }

            _actionTask = PutAsync();
        }

        private UniTask PutAsync()
        {
            Paper holdable = _playerPaperContainer.Pop();

            if (holdable != null)
                holdable.IsAccessed = false;
            
            if (_playerPaperContainer.Papers.Count == 0)
            {
                _playerIKService.ClearIKHandTargets();
                _playerPaperContainer.Clear();
            }

            return _holder.PutAsync(holdable, _lifetimeTokenSource.Token);
        }

        private void OnTriggerEntered(Collider other)
        {
            if (other.TryGetComponent(out IHolder holder))
            {
                if(!holder.CanPut)
                    return;
                
                _holder = holder;

                _isTaking = _playerPaperContainer.Papers.Count == 0;
            }
        }

        private void OnTriggerExited(Collider other)
        {
            if (other.TryGetComponent(out IHolder holder))
            {
                if (_holder == holder)
                {
                    _holder = null;
                }
            }
        }
    }
}