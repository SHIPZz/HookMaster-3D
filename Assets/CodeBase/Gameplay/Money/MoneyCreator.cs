using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Gameplay.Camera;
using CodeBase.Services.Factories.ShopItems;
using CodeBase.Services.Providers.Player;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace CodeBase.Gameplay.Money
{
    public class MoneyCreator : MonoBehaviour
    {
        [SerializeField] private List<Money> _money;
        [SerializeField] private float _stepMovement = 0.3f;
        [SerializeField] private float _moveYDuration = 0.3f;
        [SerializeField] private float _spacingZ = 0.3f;
        [SerializeField] private float _spacingY = 0.1f;
        [SerializeField] private Transform _parent;

        public IReadOnlyList<Money> Money => _money;

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
            Money money = _gameItemFactory.Create<Money>(_parent, _parent.transform.position);
            _spawnedCount++;
            _money.Add(money);

            money.transform.localPosition = _lastSpawnedPos + new Vector3(0, 0, _spacingZ);
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
        
        public async UniTask MoveCreatedMoneyToPlayer(Action onComplete = null)
        {
            _money.RemoveAll(x => !x.gameObject.activeSelf);

            foreach (Money money in _money)
            {
                await UniTask.WaitForSeconds(_stepMovement);
                money.Move();
            }
            
            onComplete?.Invoke();
            _lastSpawnedPos = Vector3.zero;
            _firstSpawnedPos = Vector3.zero;
        }
    }
}