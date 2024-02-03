using System;
using System.Linq;
using System.Threading;
using CodeBase.Gameplay.PaperSystem;
using CodeBase.Gameplay.TableSystem;
using CodeBase.Services.TriggerObserve;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.PlayerSystem
{
    public class PlayerDeliverPaper : MonoBehaviour
    {
        [SerializeField] private TriggerObserver _triggerObserver;
        [SerializeField] private PlayerPaperContainer _playerPaperContainer;

        private PlayerIKService _playerIKService;
        private Paper _lastPaper;
        private CancellationTokenSource _cancellationToken = new();
        private bool _exited;

        [Inject]
        private void Construct(PlayerIKService playerIKService)
        {
            _playerIKService = playerIKService;
        }

        private void OnEnable()
        {
            // _triggerObserver.TriggerEntered += Deliver;
            // _triggerObserver.TriggerExited += Exited;
        }

        private void OnDisable()
        {
            // _triggerObserver.TriggerEntered -= Deliver;
            // _triggerObserver.TriggerExited -= Exited;
        }

        private async void Deliver(Collider collider)
        {
            if (!collider.gameObject.TryGetComponent(out Table table))
                return;

            _cancellationToken?.Dispose();
            _cancellationToken = new();

            try
            {
                await Deliver(table).AttachExternalCancellation(_cancellationToken.Token);
            }
            catch (Exception e)
            {
                return;
            }

            _playerPaperContainer.Clear();
            _playerIKService.ClearIKHandTargets();
            _lastPaper = null;
        }

        private async UniTask Deliver(Table table)
        {
            foreach (Paper paper in _playerPaperContainer.Papers.Where(x => !x.IsOnEmployeeTable))
            {
                paper.SetOnEmployeeTable(true);
                paper.transform.SetParent(table.PaperPosition);
                paper.transform.localRotation = Quaternion.identity;
                table.Add(paper);

                if (_lastPaper != null)
                {
                    await paper.transform
                        .DOLocalJump(_lastPaper.transform.localPosition + table.Offset, 1f, 1, 0.5f)
                        .AsyncWaitForCompletion().AsUniTask().AttachExternalCancellation(_cancellationToken.Token);
                }
                else
                {
                    await paper.transform
                        .DOLocalJump(Vector3.zero, 1f, 1, 0.5f)
                        .AsyncWaitForCompletion().AsUniTask().AttachExternalCancellation(_cancellationToken.Token);
                }

                _lastPaper = paper;
            }
        }

        private void Exited(Collider obj)
        {
            _cancellationToken?.Cancel();
        }
    }
}