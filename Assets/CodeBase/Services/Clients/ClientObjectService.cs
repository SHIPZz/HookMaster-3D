using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Gameplay.Clients;
using CodeBase.Gameplay.CouchSystem;
using CodeBase.Services.Providers.Couchs;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CodeBase.Services.Clients
{
    public class ClientObjectService
    {
        private readonly CouchService _couchService;
        private readonly ClientProvider _clientProvider;
        private List<Client> _createdClients = new();
        private ClientSpawner _clientSpawner;
        private Client _lastClient;
        private int _disabledClients;
        private Transform _servePoint;
        private bool _isFirstClientWentToServePoint;

        public ClientObjectService(ClientProvider clientProvider, CouchService couchService)
        {
            _couchService = couchService;
            _clientProvider = clientProvider;
        }

        public void Init()
        {
            _clientProvider.ClientSpawners.ForEach(x => _createdClients.Add(x.Spawn()));

            foreach (Client client in _createdClients)
            {
                Transform sitPlace = _couchService.GetSitPlace();

                if (sitPlace == null)
                    return;

                client.SetSitIdle(true, sitPlace);
            }
        }

        public void SetServePoint(Transform servePoint)
        {
            _servePoint = servePoint;
        }

        public void ActivateNextClient()
        {
            Client targetClient = _createdClients.FirstOrDefault(x => x.IsServed == false);

            if (targetClient == null)
            {
                _createdClients.Clear();
                CreateNewClients();
                SetMoveDirectionToClients();
                return;
            }

            targetClient.SetSitIdle(false, _servePoint);
            targetClient.SetTarget(_servePoint);
        }

        public void SetServed(string id)
        {
            _lastClient = _createdClients.FirstOrDefault(x => x.Id == id);
            _lastClient.IsServed = true;
            _lastClient.MoveBack();
        }

        private void CreateNewClients()
        {
            _clientProvider.ClientSpawners.ForEach(x => _createdClients.Add(x.Spawn()));
        }

        private void SetMoveDirectionToClients()
        {
            _isFirstClientWentToServePoint = false;
            
            foreach (Client client in _createdClients)
            {
                if (!_isFirstClientWentToServePoint)
                {
                    client.SetTarget(_servePoint);
                    _isFirstClientWentToServePoint = true;
                    continue;
                }

                Transform sitPlace = _couchService.GetSitPlace();

                client.SetSitIdleByMoving(sitPlace);
            }
        }
    }
}