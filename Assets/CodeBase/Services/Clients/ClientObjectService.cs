using System.Collections.Generic;
using System.Linq;
using CodeBase.Gameplay.Clients;
using UnityEngine;

namespace CodeBase.Services.Clients
{
    public class ClientObjectService
    {
        private List<Client> _createdClients = new();
        private ClientSpawner _clientSpawner;
        private ClientProvider _clientProvider;
        private Client _lastClient;

        public ClientObjectService(ClientProvider clientProvider)
        {
            _clientProvider = clientProvider;
        }

        public void Init()
        {
            _clientProvider.ClientSpawners.ForEach(x => _createdClients.Add(x.Spawn()));
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