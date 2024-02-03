using System.Collections.Generic;
using CodeBase.Enums;
using CodeBase.Gameplay.ResourceItem;
using CodeBase.Services.Factories.GameItem;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.ObjectCreatorSystem
{
    public class ResourceCreator : MonoBehaviour
    {
        [SerializeField] private float _spacingZ = 0.3f;
        [SerializeField] private float _spacingY = 0.1f;
        [SerializeField] private Vector3 _offset;
        [SerializeField] private Transform _parent;
        [SerializeField] private float _columnCount = 2;
        [SerializeField] private GameItemType _gameItemType;

        private List<Resource> _papers = new();
        private GameItemFactory _gameItemFactory;
        private int _spawnedCount;
        private Vector3 _firstSpawnedPos = Vector3.zero;
        private Vector3 _lastSpawnedPos;

        [Inject]
        private void Construct(GameItemFactory gameItemFactory)
        {
            _gameItemFactory = gameItemFactory;
        }

        private void OnDisable() =>
            _papers.ForEach(x =>
            {
                if (x != null)
                    x.Collected -= Reset;
            });

        [Button]
        public void Create(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Create();
            }
        }

        [Button]
        public Resource Create()
        {
            Resource resource = _gameItemFactory.CreateResourceItem(_gameItemType, _parent, _parent.transform.position);

            _papers.Add(resource);
            _papers.RemoveAll(x => x == null);
            _spawnedCount++;

            if (_spawnedCount > 0)
                resource.transform.localPosition = _lastSpawnedPos + _offset;

            if (_firstSpawnedPos == Vector3.zero)
                _firstSpawnedPos = resource.transform.localPosition;


            resource.Collected += Reset;
            _lastSpawnedPos = resource.transform.localPosition;

            if (_spawnedCount != 1 && _spawnedCount % _columnCount == 1)
            {
                resource.transform.localPosition = _firstSpawnedPos + new Vector3(0, _spacingY, 0);
                _lastSpawnedPos = resource.transform.localPosition;
                _firstSpawnedPos = resource.transform.localPosition;
            }

            return resource;
        }

        [Button]
        private void Reset(Resource resource)
        {
            _firstSpawnedPos = Vector3.zero;
            _spawnedCount = 0;
            _lastSpawnedPos = Vector3.zero;
            resource.Collected -= Reset;
        }
    }
}