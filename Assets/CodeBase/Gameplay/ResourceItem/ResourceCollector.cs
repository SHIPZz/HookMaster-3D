using System;
using CodeBase.Services.TriggerObserve;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.ResourceItem
{
    internal class ResourceCollector : MonoBehaviour, IResourceCollector
    {
        public event Action<IResourceCollector, IResource> ResourceDetected;

        [SerializeField] private Transform _controlPoint;
        [SerializeField] private TriggerObserver _triggerObserver;

        [Inject] private readonly IResourceCollectionSystem _resourceCollectionSystem;

        public Transform Anchor => transform;
        public Transform ControlPoint => _controlPoint;

        private void OnEnable()
        {
            _resourceCollectionSystem.Register(this);
            _triggerObserver.TriggerEntered += OnResourceEnter;
        }

        private void OnDisable()
        {
            _resourceCollectionSystem.Remove(this);
            _triggerObserver.TriggerEntered -= OnResourceEnter;
        }

        private void OnResourceEnter(Collider other)
        {
            if (other.TryGetComponent(out IResource resource))
            {
                ResourceDetected?.Invoke(this, resource);
            }
        }
    }
}