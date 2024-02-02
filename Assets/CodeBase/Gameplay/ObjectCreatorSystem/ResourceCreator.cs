using System.Collections.Generic;
using CodeBase.Enums;
using CodeBase.Gameplay.PaperSystem;
using CodeBase.Gameplay.ResourceItem;
using CodeBase.Services.Factories.GameItem;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.ObjectCreatorSystem
{
    public class ResourceCreator : MonoBehaviour
    {
        [SerializeField] private List<Resource> _papers;
        [SerializeField] private float _spacingZ = 0.3f;
        [SerializeField] private float _spacingY = 0.1f;
        [SerializeField] private Vector3 _offset;
        [SerializeField] private Transform _parent;
        [SerializeField] private float _columnCount = 2;
        [SerializeField] private GameItemType _gameItemType;
        
        private GameItemFactory _gameItemFactory;
        private int _spawnedCount;
        private Vector3 _firstSpawnedPos;
        private Vector3 _lastSpawnedPos;

        [Inject]
        private void Construct(GameItemFactory gameItemFactory)
        {
            _gameItemFactory = gameItemFactory;
        }

        private void OnDisable()
        {
            _papers.ForEach(x =>
            {
                if (x != null)
                    x.Collected -= Reset;
            });
        }

        [Button]
        public Resource Create()
        {
            Resource resource = _gameItemFactory.CreateResourceItem(_gameItemType, _parent, _parent.transform.position);

            _spawnedCount++;
            _papers.Add(resource);
            _papers.RemoveAll(x => x == null);

            resource.transform.localPosition = _lastSpawnedPos + _offset;
            resource.Collected += Reset;
            _lastSpawnedPos = resource.transform.localPosition;

            if (_firstSpawnedPos == Vector3.zero)
                _firstSpawnedPos = resource.transform.localPosition;

            if (_spawnedCount != 1 && _spawnedCount % _columnCount == 1)
            {
                resource.transform.localPosition = _firstSpawnedPos;
                resource.transform.localPosition += new Vector3(0, _spacingY, 0);
                _lastSpawnedPos = resource.transform.localPosition;
                _firstSpawnedPos = resource.transform.localPosition;
            }

            return resource;
        }

        private void Reset(Resource resource)
        {
            _firstSpawnedPos = Vector3.zero;
            _spawnedCount = 0;
            _lastSpawnedPos = Vector3.zero;
        }
    }
}