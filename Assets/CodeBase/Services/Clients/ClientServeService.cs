using System;
using System.Threading;
using CodeBase.Data;
using CodeBase.Gameplay.Clients;
using CodeBase.Gameplay.Wallet;
using CodeBase.Services.Wallet;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CodeBase.Services.Clients
{
    public class ClientServeService
    {
        private const float ServeTime = 2f;
        private const int ServeReward = 50;
        private readonly ClientObjectService _clientObjectService;
        private bool _isPlayerAroundTable;
        private bool _isClientApproached;
        private Client _currentClient;
        private CancellationTokenSource _serveTokenSource = new CancellationTokenSource();
        private readonly WalletService _walletService;
        private float _serveDelay;

        public event Action<float> Started;
        public event Action<int> Finished;

        public ClientServeService(ClientObjectService clientObjectService, WalletService walletService)
        {
            _walletService = walletService;
            _clientObjectService = clientObjectService;
        }

        public void SetServeDelay(float delay)
        {
            _serveDelay = delay;
        }

        public async void SetPlayerApproached(bool isApproached)
        {
            _isPlayerAroundTable = isApproached;

            try
            {
                await TryStartServing();
            }
            catch (Exception e)
            {
                // ignored
            }
        }

        public async void SetClientApproached(bool isApproached, Client client)
        {
            _currentClient = client;
            _isClientApproached = isApproached;

            try
            {
                await TryStartServing();
            }
            catch (Exception e)
            {
                // ignored
            }
        }

        public void SetTargetServePoint(Transform servePoint)
        {
            _clientObjectService.SetServePoint(servePoint);
        }

        public void Stop()
        {
            _serveTokenSource?.Cancel();
        }

        private async UniTask TryStartServing()
        {
            if (_currentClient == null)
                _clientObjectService.ActivateNextClient();

            if (!_isClientApproached || !_isPlayerAroundTable)
                return;

            
            _serveTokenSource?.Dispose();
            _serveTokenSource = new CancellationTokenSource();
            await UniTask.WaitForSeconds(_serveDelay);
            Started?.Invoke(ServeTime);
            await UniTask.WaitForSeconds(ServeTime).AttachExternalCancellation(_serveTokenSource.Token);
            OnServeFinished();
        }

        private void OnServeFinished()
        {
            _isClientApproached = false;
            // _walletService.Set(ItemTypeId.Money, ServeReward);
            Finished?.Invoke(ServeReward);
            _clientObjectService.SetServed(_currentClient.Id);
            _clientObjectService.ActivateNextClient();
        }
    }
}