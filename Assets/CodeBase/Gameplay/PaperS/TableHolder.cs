using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Gameplay.BurnableObjectSystem;
using CodeBase.Gameplay.PaperSystem;
using CodeBase.Gameplay.ResourceItem;
using CodeBase.Gameplay.TableSystem;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.PaperS
{
    internal class TableHolder : MonoBehaviour
    {
        [SerializeField] private Transform _holdablesRoot;
        [SerializeField] private Vector3 _offset = new(0, 0.06f, 0);
        [SerializeField] private Table _table;

        private readonly Stack<Paper> _papers = new();
        private Paper _lastPaper;
        private ResourceCollectionSettings _resourceCollectionSettings;

        public event Action ItemPut;

        public int ItemsCount => _papers.Count;

        public bool IsItemAdding { get; set; }

        public bool CanPut => !_table.IsFree;

        public bool CanTakePapers => 
            _papers.All(x => x.ReadyToTransfer) && !IsItemAdding;

        [Inject]
        private void Construct(ResourceCollectionSettings resourceCollectionSettings)
        {
            _resourceCollectionSettings = resourceCollectionSettings;
        }

        private void OnEnable() => 
            _table.Burned += DestroyItems;

        private void OnDisable() => 
            _table.Burned -= DestroyItems;

        private void DestroyItems(IBurnable burnable)
        {
            foreach (Paper holdable in _papers)
            {
                Destroy(holdable.gameObject);
            }

            _papers.Clear();
        }

        public Paper Take(Transform parent)
        {
            if (!_papers.TryPop(out Paper paper))
                return null;

            _lastPaper = _papers.TryPeek(out Paper targetPaper) ? targetPaper : null;

            paper.transform.SetParent(parent);
            return paper;
        }

        public IEnumerator PutCoroutine(Paper paper)
        {
            paper.transform.SetParent(_holdablesRoot, true);
            paper.transform.localRotation = Quaternion.identity;
            _papers.Push(paper);

            if (_lastPaper != null)
            {
                yield return paper.transform
                    .DOLocalMove(_lastPaper.transform.localPosition + _offset, _resourceCollectionSettings.PutTime)
                    .SetEase(_resourceCollectionSettings.PutEase)
                    .AsyncWaitForCompletion()
                    .AsUniTask().ToCoroutine();
                
                _lastPaper = paper;
                paper.ReadyToTransfer = true;
            }
            else
            {
                yield return paper.transform.DOLocalMove(Vector3.zero, _resourceCollectionSettings.PutTime)
                    .SetEase(_resourceCollectionSettings.PutEase)
                    .AsyncWaitForCompletion()
                    .AsUniTask()
                    .ToCoroutine();
                
                _lastPaper = paper;
                paper.ReadyToTransfer = true;
            }

            ItemPut?.Invoke();
        }
    }
}