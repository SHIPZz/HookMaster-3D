using System.Collections.Generic;
using System.Linq;
using CodeBase.Constant;
using CodeBase.Gameplay.Clients;
using CodeBase.Services.Providers.Asset;
using CodeBase.SO.Clients;
using UnityEngine;
using Zenject;

namespace CodeBase.Services.Factories.Clients
{
    public class ClientFactory
    {
        private readonly IInstantiator _instantiator;
        private readonly IAssetProvider _assetProvider;
        private List<string> _createdClientsId = new();

        public ClientFactory(IInstantiator instantiator, IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
            _instantiator = instantiator;
        }

        public Client Create(Transform parent, Vector3 at)
        {
            List<Client> prefabs = _assetProvider.GetAll<Client>(AssetPath.Clients);
            Client targetPrefab = prefabs.FirstOrDefault(x => !_createdClientsId.Contains(x.Id));
            _createdClientsId.Add(targetPrefab.Id);
            return _instantiator.InstantiatePrefabForComponent<Client>(targetPrefab, at, Quaternion.identity, parent);
        }
    }
}