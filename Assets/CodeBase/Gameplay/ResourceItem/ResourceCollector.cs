using System;
using CodeBase.Enums;
using CodeBase.Services.TriggerObserve;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.ResourceItem
{
    public class ResourceCollector : MonoBehaviour, IResourceCollector
    {
        [SerializeField] private Transform _controlPoint;
        [SerializeField] private TriggerObserver _triggerObserver;
        [SerializeField] private GameItemType _resourceType;

        [Inject] private readonly IResourceCollectionSystem _resourceCollectionSystem;

        [field: SerializeField] public Transform Anchor { get; private set; }

        public Transform ControlPoint => _controlPoint;
        public event Action<IResourceCollector, Resource> ResourceDetected;

        protected virtual void OnEnable()
        {
            _resourceCollectionSystem.Register(this);
            _triggerObserver.TriggerEntered += OnResourceEnter;
        }

        protected virtual void OnDisable()
        {
            _resourceCollectionSystem.Remove(this);
            _triggerObserver.TriggerEntered -= OnResourceEnter;
        }

        protected virtual void OnResourceEnter(Collider other)
        {
            if(!other.gameObject.TryGetComponent(out Resource resource))
                return;
            
            if(resource.GameItemType != _resourceType)
                return;
            
            ResourceDetected?.Invoke(this, resource);
        }
    }
}