using CodeBase.Gameplay.Clients;
using CodeBase.Services.Factories.Clients;
using UnityEngine;
using Zenject;

namespace CodeBase.Services.Clients
{
    public class ClientSpawner : MonoBehaviour
    {
        private ClientFactory _clientFactory;

        [Inject]
        private void Construct(ClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public Client Spawn() => 
            _clientFactory.Create(transform, transform.position);
    }
}