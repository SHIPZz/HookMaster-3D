using System;
using System.Collections.Generic;
using CodeBase.Enums;
using CodeBase.Gameplay.ResourceItem;
using CodeBase.Services.Factories.GameItem;
using CodeBase.Services.Providers.Player;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.ObjectCreatorSystem
{
    public class ResourceCreator : MonoBehaviour
    {
        [SerializeField] private float _spacingY = 0.1f;
        [SerializeField] private Vector3 _offset;
        [SerializeField] private Transform _parent;
        [SerializeField] private float _columnCount = 2;
        [SerializeField] private GameItemType _gameItemType;
        [SerializeField] private Vector3 _rotation;
        [SerializeField] private Stacker _stacker;

        private List<Resource> _resources = new();
        private GameItemFactory _gameItemFactory;
        private int _spawnedCount;
        private Vector3 _firstSpawnedPos = Vector3.zero;
        private Vector3 _lastSpawnedPos;
        private PlayerProvider _playerProvider;

        [Inject]
        private void Construct(GameItemFactory gameItemFactory, PlayerProvider playerProvider)
        {
            _playerProvider = playerProvider;
            _gameItemFactory = gameItemFactory;
        }

        private void OnDisable() =>
            _resources.ForEach(x =>
            {
                if (x != null)
                    x.Collected -= Reset;
            });

        public Resource Create()
        {
            Resource resource = _gameItemFactory.CreateResourceItem(_gameItemType, _parent, _parent.transform.position);

            if (_rotation != Vector3.zero)
                resource.transform.localRotation = Quaternion.Euler(_rotation);

            _stacker.AddToStack(resource.gameObject);

            // _resources.Add(resource);
            // _resources.RemoveAll(x => x == null);
            _spawnedCount++;

            // SetResourcePosition(resource);

            resource.Collected += Reset;

            return resource;
        }

        private void SetResourcePosition(Resource resource)
        {
            if (_spawnedCount > 0)
                resource.transform.localPosition = _lastSpawnedPos + _offset;

            if (_firstSpawnedPos == Vector3.zero)
                _firstSpawnedPos = resource.transform.localPosition;

            _lastSpawnedPos = resource.transform.localPosition;

            if (_spawnedCount != 1 && _spawnedCount % _columnCount == 1)
            {
                resource.transform.localPosition = _firstSpawnedPos + new Vector3(0, _spacingY, 0);
                _lastSpawnedPos = resource.transform.localPosition;
                _firstSpawnedPos = resource.transform.localPosition;
            }
        }

        private void Reset(Resource resource)
        {
            _firstSpawnedPos = Vector3.zero;
            _spawnedCount = 0;
            _lastSpawnedPos = Vector3.zero;
            resource.Collected -= Reset;
        }

        public void SetCreatedCountZero() => 
            _stacker.SetCurrentIndexZero();
    }
}
