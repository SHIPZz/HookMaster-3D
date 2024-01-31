using System;
using System.Collections.Generic;
using CodeBase.Gameplay.ResourceItem;
using CodeBase.Services.Factories.GameItem;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.Money
{
    public class MoneyCreator : MonoBehaviour
    {
        [SerializeField] private List<Resource> _money;
        [SerializeField] private float _spacingZ = 0.3f;
        [SerializeField] private float _spacingY = 0.1f;
        [SerializeField] private Transform _parent;

        private GameItemFactory _gameItemFactory;
        private Vector3 _lastSpawnedPos = Vector3.zero;
        private int _spawnedCount;
        private Vector3 _firstSpawnedPos;

        [Inject]
        private void Construct(GameItemFactory gameItemFactory)
        {
            _gameItemFactory = gameItemFactory;
        }

        [Button]
        public void Create()
        {
            var money = _gameItemFactory.Create<Resource>(_parent, _parent.transform.position);
            _spawnedCount++;
            _money.Add(money);
            _money.RemoveAll(x => x == null);

            money.transform.localPosition = _lastSpawnedPos + new Vector3(0, 0, _spacingZ);
            money.Collected += Reset;
            _lastSpawnedPos = money.transform.localPosition;

            if (_firstSpawnedPos == Vector3.zero)
                _firstSpawnedPos = money.transform.localPosition;

            if (_spawnedCount != 1 && _spawnedCount % 4 == 1)
            {
                money.transform.localPosition = _firstSpawnedPos;
                money.transform.localPosition += new Vector3(0, _spacingY, 0);
                _lastSpawnedPos = money.transform.localPosition;
                _firstSpawnedPos = money.transform.localPosition;
            }
        }

        private void OnDisable()
        {
            _money.ForEach(x =>
            {
                if (x != null)
                    x.Collected -= Reset;
            });
        }

        private void Reset(Resource resource)
        {
            _firstSpawnedPos = Vector3.zero;
            _spawnedCount = 0;
            _lastSpawnedPos = Vector3.zero;
            resource.Collected -= Reset;
        }
    }
}