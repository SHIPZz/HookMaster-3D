using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace CodeBase.Gameplay.ResourceItem
{
    internal class ResourceCollectionSystem : IResourceCollectionSystem, IDisposable
    {
        private readonly ResourceCollectionSettings _settings;
        private readonly List<IResourceCollector> _collectors = new();
        private readonly CancellationTokenSource _lifetimeToken = new();

        public ResourceCollectionSystem(ResourceCollectionSettings settings)
        {
            _settings = settings;
        }

        public void Dispose()
        {
            foreach (var collector in _collectors)
            {
                collector.ResourceDetected -= OnResourceDetected;
            }

            _collectors.Clear();

            _lifetimeToken?.Cancel();
            _lifetimeToken?.Dispose();
        }

        public void Register(IResourceCollector resourceCollector)
        {
            _collectors.Add(resourceCollector);
            resourceCollector.ResourceDetected += OnResourceDetected;
        }

        public void Remove(IResourceCollector resourceCollector)
        {
            _collectors.Remove(resourceCollector);
            resourceCollector.ResourceDetected -= OnResourceDetected;
        }

        private void OnResourceDetected(IResourceCollector detector, IResource detectedResource)
        {
            CollectResource(detector, detectedResource);
        }

        private async void CollectResource(IResourceCollector collector, IResource resource)
        {
            try
            {
                resource.MarkAsDetected(); 
                var task = new ResourceCollectionTask(resource.Anchor, collector.Anchor, collector.ControlPoint, _settings);
                await task.ExecuteAsync(_lifetimeToken.Token);
                resource.Collect();
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception exception)
            {
                Debug.LogError("Resource collection failed: " + exception.Message);
            }
        }
    }
}