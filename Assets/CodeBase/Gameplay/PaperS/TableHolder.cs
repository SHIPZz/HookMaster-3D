using System;
using System.Collections;
using System.Collections.Generic;
using CodeBase.Gameplay.BurnableObjectSystem;
using CodeBase.Gameplay.TableSystem;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace CodeBase.Gameplay.PaperS
{
    internal class TableHolder : MonoBehaviour
    {
        [SerializeField] private Transform _holdablesRoot;
        [SerializeField] private float _moveHoldableDuration = 0.5f;
        [SerializeField] private Vector3 _offset = new(0, 0.06f, 0);
        [SerializeField] private Table _table;

        private readonly Stack<IHoldable> _items = new();
        private IHoldable _lastHoldable;

        public event Action ItemPut;

        public int ItemsCount => _items.Count;
        public bool IsItemAdding { get; set; }

        private void OnEnable()
        {
            _table.Burned += DestroyItems;
        }

        private void OnDisable()
        {
            _table.Burned -= DestroyItems;
        }

        private void DestroyItems(IBurnable burnable)
        {
            foreach (IHoldable holdable in _items)
            {
                Destroy(holdable.Transform.gameObject);
            }

            _items.Clear();
        }

        public IHoldable Take(Transform parent)
        {
            IHoldable holdable = _items.Pop();

            if (_items.TryPeek(out IHoldable targetHoldable))
                _lastHoldable = targetHoldable;

            holdable.Transform.SetParent(parent);
            return holdable;
        }

        public IEnumerator PutAsync(IHoldable holdable)
        {
            holdable.Transform.SetParent(_holdablesRoot, true);
            holdable.Transform.localRotation = Quaternion.identity;
            _items.Push(holdable);

            if (_lastHoldable != null)
                yield return holdable.Transform
                    .DOLocalMove(_lastHoldable.Transform.localPosition + _offset, _moveHoldableDuration)
                    .SetEase(Ease.InOutQuint)
                    .AsyncWaitForCompletion()
                    .AsUniTask().ToCoroutine();
            else
                yield return holdable.Transform.DOLocalMove(Vector3.zero, _moveHoldableDuration)
                    .SetEase(Ease.InOutQuint)
                    .AsyncWaitForCompletion()
                    .AsUniTask()
                    .ToCoroutine();

            ItemPut?.Invoke();
            _lastHoldable = holdable;
        }

        public void SetLastHoldableNull()
        {
            _lastHoldable = null;
        }
    }
}