using System;
using CodeBase.Services.Fire;
using CodeBase.Services.Providers.Extinguisher;
using Zenject;

namespace CodeBase.Services.Extinguisher
{
    public class ExtinguisherService : IInitializable, IDisposable
    {
        private readonly ExtinguisherProvider _extinguisherProvider;
        private readonly FireService _fireService;

        public ExtinguisherService(ExtinguisherProvider extinguisherProvider, FireService fireService)
        {
            _fireService = fireService;
            _extinguisherProvider = extinguisherProvider;
        }
        public void Initialize()
        {
            _fireService.FireStarted += SpawnExtinguishers;
            _fireService.FirePutOut += DestroySpawnedExtinguishers;
        }

        public void Dispose()
        {
            _fireService.FireStarted -= SpawnExtinguishers;
            _fireService.FirePutOut -= DestroySpawnedExtinguishers;
        }

        private void DestroySpawnedExtinguishers()
        {
            _extinguisherProvider.ExtinguisherSpawners.ForEach(x => x.DestroyCreatedExtinguisher());
        }

        private void SpawnExtinguishers()
        {
            _extinguisherProvider.ExtinguisherSpawners.ForEach(x => x.Spawn());
        }
    }
}