using System;
using CodeBase.Services.TriggerObserve;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.ResourceItem
{
    public class ResourceCollector : MonoBehaviour, IResourceCollector
    {
        public event Action<IResourceCollector, Resource> ResourceDetected;

        [SerializeField] private Transform _controlPoint;
        [SerializeField] private TriggerObserver _triggerObserver;

        [Inject] private readonly IResourceCollectionSystem _resourceCollectionSystem;

        [field: SerializeField] public Transform Anchor { get; private set; }

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

        protected virtual void OnResourceEnter(Collider other)
        {
            ResourceDetected?.Invoke(this, other.GetComponent<Resource>());
        }
    }
}