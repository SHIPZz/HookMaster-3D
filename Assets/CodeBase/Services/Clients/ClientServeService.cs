using System.Collections;
using CodeBase.Gameplay.Clients;
using CodeBase.Services.Coroutine;
using UnityEngine;

namespace CodeBase.Services.Clients
{
    public class ClientServeService
    {
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly ClientObjectService _clientObjectService;
        private bool _isPlayerAroundTable;
        private bool _isClientApproached;
        private Client _currentClient;
        private Vector3 _servePoint;

        public ClientServeService(ClientObjectService clientObjectService,  ICoroutineRunner coroutineRunner)
        {
            _clientObjectService = clientObjectService;
            _coroutineRunner = coroutineRunner;
        }

        public void SetPlayerApproached(bool isApproached)
        {
            _isPlayerAroundTable = isApproached;
            TryStartServing();
        }

        public void SetClientApproached(bool isApproached, Client client)
        {
            _currentClient = client;
            _isClientApproached = isApproached;
            TryStartServing();
        }

        public void SetTargetServePoint(Vector3 servePoint)
        {
            _servePoint = servePoint;
        }

        public void TryStartServing()
        {
            Debug.Log(_isPlayerAroundTable);
            Debug.Log(_isClientApproached);
            
            _clientObjectService.ActivateNextClient(_servePoint);
            if (_isClientApproached && _isPlayerAroundTable)
                _coroutineRunner.StartCoroutine(StartServeCoroutine());
        }

        private IEnumerator StartServeCoroutine()
        {
            yield return new WaitForSeconds(2f);
            OnServeFinished();
        }

        private void OnServeFinished()
        {
            _clientObjectService.SetServed(_currentClient.Id);
            _clientObjectService.ActivateNextClient(_servePoint);
        }
    }
}