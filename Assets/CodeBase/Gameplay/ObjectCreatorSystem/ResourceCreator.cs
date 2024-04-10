using System.Collections.Generic;
using CodeBase.Enums;
using CodeBase.Gameplay.AnimMovement;
using CodeBase.Gameplay.ResourceItem;
using CodeBase.Services.Factories.GameItem;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.ObjectCreatorSystem
{
    public class ResourceCreator : MonoBehaviour
    {
        [SerializeField] private Transform _parent;
        [SerializeField] private GameItemType _gameItemType;
        [SerializeField] private Vector3 _rotation;
        [SerializeField] private ResourceTestStacker _resourceTestStacker;
        [SerializeField] private bool _localRotation;
        [SerializeField] private bool _clearOnResourceCollect;
        [SerializeField] private AfterResourceCreateMovementBehaviour _afterResourceCreateMovement;
        [SerializeField] private Transform _startAnimPosition;
        [SerializeField] private float _stackAnimSpeed = 0.65f;

        private List<Resource> _resources = new();
        private GameItemFactory _gameItemFactory;

        [Inject]
        private void Construct(GameItemFactory gameItemFactory)
        {
            _gameItemFactory = gameItemFactory;
        }

        private void OnDisable() =>
            _resources.ForEach(x =>
            {
                if (x != null)
                    x.Collected -= ResourceCollectedHandler;
            });

        public Resource Create()
        {
            var targetRotation = _rotation == Vector3.zero ? Quaternion.identity : Quaternion.Euler(_rotation);
            Resource resource = _gameItemFactory.CreateResourceItem(_gameItemType, _parent, 
                _parent.transform.position, targetRotation,_localRotation);
            
            _resourceTestStacker.CalculateTargetPosition(resource, out Vector3 targetPosition);
            _afterResourceCreateMovement.Move(resource,_startAnimPosition.position, ()=> targetPosition, _stackAnimSpeed);

            resource.Collected += ResourceCollectedHandler;

            return resource;
        }

        private void ResourceCollectedHandler(Resource resource)
        {
            if(_clearOnResourceCollect)
                Clear();
            
            resource.Collected -= ResourceCollectedHandler;
        }

        public void Clear()
        {
            _resourceTestStacker.Clear();
        }
    }
}