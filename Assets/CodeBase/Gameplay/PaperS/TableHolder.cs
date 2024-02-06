using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CodeBase.Gameplay.PaperS;
using DG.Tweening;
using UnityEngine;

namespace _Project_legacy.Scripts.Papers
{
    internal class TableHolder : MonoBehaviour, IHolder
    {
        [SerializeField] private Transform _holdablesRoot;
        [SerializeField] private float _timeToTake = 0.5f;
        [SerializeField] private Vector3 _offset = new(0, 0.06f, 0);
        [SerializeField] private bool _onlyHold = true;

        private readonly Stack<IHoldable> _items = new();
        private IHoldable _lastHoldable;

        public event Action ItemPut;
        
        public int ItemsCount => _items.Count;
        

        private void OnEnable()
        {
            IHoldable[] items = _holdablesRoot.GetComponentsInChildren<IHoldable>();

            foreach (var item in items)
            {
                _items.Push(item);
            }
        }

        public async Task<IHoldable> TakeAsync(Transform parent, CancellationToken cancellationToken)
        {
            if (ItemsCount == 0)
                throw new Exception("No items to take");

            IHoldable holdable = _items.Pop();

            holdable.Transform.SetParent(parent, true);
            await holdable.Transform.DOLocalMove(Vector3.up, _timeToTake).PlayAsync(cancellationToken);
            return holdable;
        }

        public async Task PutAsync(IHoldable holdable, CancellationToken cancellationToken)
        {
            holdable.Transform.SetParent(_holdablesRoot, true);
            holdable.Transform.localRotation = Quaternion.identity;
            print(holdable);
            
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
    }
}