using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Gameplay.Clients;
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

        public void ActivateNextClient(Transform servePoint)
        {
            Client targetClient = _createdClients.FirstOrDefault(x => x.IsServed == false);

            if (targetClient == null)
            {
                _createdClients.ForEach(x => x.IsServed = false);
            }

            targetClient = _createdClients.FirstOrDefault(x => x.IsServed == false);
            
            targetClient.SetSitIdle(false, servePoint);
            targetClient.SetTarget(servePoint);
        }

        public void SetServed(string id, Action onComplete = null)
        {
            _lastClient = _createdClients.FirstOrDefault(x => x.Id == id);
            _lastClient.IsServed = true;
            _lastClient.MoveBack(onComplete);
        }
    }
}