using System;
using System.Threading;
using CodeBase.Data;
using CodeBase.Gameplay.Wallet;
using CodeBase.Services.TriggerObserve;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.Money
{
    public class MoneyMovementHandler : MonoBehaviour
    {
        [SerializeField] private TriggerObserver _triggerObserver;
        [SerializeField] private MoneyCreator _moneyCreator;
        private WalletService _walletService;
        private CancellationTokenSource _cancellationToken = new();

        [Inject]
        private void Construct(WalletService walletService)
        {
            _walletService = walletService;
        }

        private void OnEnable()
        {
            _triggerObserver.TriggerEntered += PlayerEntered;
            _triggerObserver.TriggerExited += Exited;
        }

        private void OnDisable()
        {
            _triggerObserver.TriggerEntered -= PlayerEntered;
            _triggerObserver.TriggerExited -= Exited;
        }

        private void Exited(Collider obj)
        {
            _cancellationToken?.Cancel();
        }

        private async void PlayerEntered(Collider player)
        {
            _cancellationToken?.Dispose();
            _cancellationToken = new();

            try
            {
                await _moneyCreator.MoveCreatedMoneyToPlayer(OnMoved)
                    .AttachExternalCancellation(_cancellationToken.Token);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void OnMoved()
        {
            foreach (Money money in _moneyCreator.Money)
            {
                _walletService.Set(ItemTypeId.Money, money.Value);
            }
        }
    }
}