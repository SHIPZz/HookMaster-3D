using System.Collections.Generic;
using CodeBase.Enums;
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
            
            _resourceTestStacker.StackItems(resource);
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