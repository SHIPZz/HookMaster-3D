using System.Collections.Generic;
using System.Linq;
using CodeBase.Gameplay.Clients;
using CodeBase.Services.Providers.Couchs;
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
               
               if(sitPlace == null)
                   return;
               
               client.SetSitIdle(true, sitPlace);
            }
        }

        public void SetSit(Transform target)
        {
            
        }

        public void ActivateNextClient(Vector3 servePoint)
        {
            Client targetClient = _createdClients.FirstOrDefault(x => x.IsServed == false);
            targetClient.StartMovement(servePoint);
        }

        public void SetServed(string id)
        {
            _lastClient = _createdClients.FirstOrDefault(x => x.Id == id);
            _lastClient.IsServed = true;
            _lastClient.gameObject.SetActive(false);
        }
    }
}