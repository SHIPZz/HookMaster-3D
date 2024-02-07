using System.Threading;
using CodeBase.Gameplay.PaperS;
using CodeBase.Gameplay.PaperSystem;
using CodeBase.Gameplay.PlayerSystem;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace _Project_legacy.Scripts.Papers
{
    internal class PlayerHands : MonoBehaviour
    {
        [SerializeField] private PlayerPaperContainer _playerPaperContainer;

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
        }

        private void OnDisable()
        {
            _lifetimeTokenSource.Cancel();
            _lifetimeTokenSource.Dispose();
        }

        private void Update()
        {
            if (_holder == null)
            {
                return;
            }

            if (_isTaking)
            {
                Take();
            }
            else
            {
                Put();
            }
        }

        private void Take()
        {
            if (_holder.ItemsCount == 0)
            {
                return;
            }

            var isTaskCompleted = _actionTask.Status.IsCompleted();

            if (!isTaskCompleted)
            {
                return;
            }

            _actionTask = TakeAsync();
        }

        private async UniTask TakeAsync()
        {
            // Task<IHoldable> takeTask = _holder.TakeAsync(transform, _lifetimeTokenSource.Token);
            // IHoldable holdable = await takeTask;
            // _playerPaperContainer.Push(holdable as Paper);
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

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IHolder holder))
            {
                _holder = holder;

                _isTaking = _playerPaperContainer.Papers.Count == 0;
            }
        }

        private void OnTriggerExit(Collider other)
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