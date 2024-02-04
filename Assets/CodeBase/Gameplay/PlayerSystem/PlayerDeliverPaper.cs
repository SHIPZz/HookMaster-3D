using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CodeBase.Gameplay.PaperSystem;
using CodeBase.Gameplay.TableSystem;
using CodeBase.Services.Employees;
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
        private EmployeeService _employeeService;

        [Inject]
        private void Construct(PlayerIKService playerIKService, EmployeeService employeeService)
        {
            _employeeService = employeeService;
            _playerIKService = playerIKService;
        }

        private void OnEnable()
        {
            _triggerObserver.TriggerEntered += OnPlayerApproachedToTable;
            _triggerObserver.TriggerExited += Exited;
        }

        private void OnDisable()
        {
            _triggerObserver.TriggerEntered -= OnPlayerApproachedToTable;
            _triggerObserver.TriggerExited -= Exited;
        }

        private async void OnPlayerApproachedToTable(Collider collider)
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

            Reboot();
        }

        private async UniTask Deliver(Table table)
        {
            IReadOnlyCollection<Paper> papersToDeliver = _playerPaperContainer.Papers;
            _employeeService.CancelProcessingPaper(table);

            foreach (Paper paper in papersToDeliver)
            {
                paper.SetOnEmployeeTable(true);
                paper.transform.SetParent(table.PaperPosition);
                paper.transform.localRotation = Quaternion.identity;
                _lastPaper = table.LastPaper;

                if (_lastPaper != null)
                {
                    await paper.transform
                        .DOLocalJump(_lastPaper.transform.localPosition + table.Offset, 1f, 1, 0.5f)
                        .AsyncWaitForCompletion().AsUniTask();
                }
                else
                {
                    await paper.transform
                        .DOLocalJump(Vector3.zero, 1f, 1, 0.5f)
                        .AsyncWaitForCompletion().AsUniTask();
                }

                table.Add(paper);
                _lastPaper = paper;
            }

            Reboot();
        }

        private void Exited(Collider obj)
        {
            if (!obj.gameObject.TryGetComponent(out Table table))
                return;

            _cancellationToken?.Cancel();
        }

        private void Reboot()
        {
            _playerPaperContainer.Clear();
            _playerIKService.ClearIKHandTargets();
            _lastPaper = null;
        }
    }
}