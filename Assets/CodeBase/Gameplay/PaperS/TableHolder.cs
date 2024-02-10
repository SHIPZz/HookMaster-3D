using System;
using System.Collections.Generic;
using System.Threading;
using CodeBase.Gameplay.PaperS;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace _Project_legacy.Scripts.Papers
{
    internal class TableHolder : MonoBehaviour, IHolder
    {
        [SerializeField] private Transform _holdablesRoot;
        [SerializeField] private float _timeToTake = 0.5f;
        [SerializeField] private Vector3 _offset = new(0, 0.06f, 0);

        private readonly Stack<IHoldable> _items = new();
        private IHoldable _lastHoldable;
        private CancellationTokenSource _cancellationToken;

        public event Action ItemPut;

        public int ItemsCount => _items.Count;

        private void OnEnable() => 
            _cancellationToken = new();

        private void OnDisable()
        {
            _cancellationToken?.Cancel();
            _cancellationToken?.Dispose();
        }

        public async UniTask<IHoldable> TakeAsync(Transform parent, CancellationToken cancellationToken)
        {
            if (ItemsCount == 0)
                throw new Exception("No items to take");

            IHoldable holdable = _items.Pop();
            
            if (_items.TryPeek(out IHoldable targetHoldable))
                _lastHoldable = targetHoldable;

            holdable.Transform.SetParent(parent, true);

            await holdable.Transform.DOLocalMove(Vector3.up, _timeToTake)
                .PlayAsync(_cancellationToken.Token);

            return holdable;
        }

        public async UniTask PutAsync(IHoldable holdable, CancellationToken cancellationToken)
        {
            holdable.Transform.SetParent(_holdablesRoot, true);
            holdable.Transform.localRotation = Quaternion.identity;

            if (_lastHoldable != null)
                await holdable.Transform.DOLocalMove(_lastHoldable.Transform.localPosition + _offset, _timeToTake)
                    .PlayAsync(cancellationToken);
            else
                await holdable.Transform.DOLocalMove(Vector3.zero, _timeToTake)
                    .PlayAsync(cancellationToken);

            ItemPut?.Invoke();
            _items.Push(holdable);
            _lastHoldable = holdable;
        }

        public void SetLastHoldableNull()
        {
            _lastHoldable = null;
            _cancellationToken?.Cancel();
            _cancellationToken?.Dispose();
            _cancellationToken = new();
        }
    }
}