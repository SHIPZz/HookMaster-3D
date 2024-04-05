using System;
using System.Threading;
using CodeBase.Gameplay.ObjectCreatorSystem;
using CodeBase.Services.TriggerObserve;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CodeBase.Gameplay.PaperSystem
{
    public class PaperCreatorTable : MonoBehaviour
    {
        [SerializeField] private ResourceCreator _resourceCreator;
        [SerializeField] private int _maxCreatCount = 10;
        [SerializeField] private float _createDelay = 3f;
        [SerializeField] private int _initialCreateCount = 5;

        public GameObject Pointer;
        public event Action PlayerApproached;

        private int _createdCount;
        private CancellationTokenSource _cancellationToken;

        private async void OnEnable()
        {
            for (int i = 0; i < _initialCreateCount; i++)
            {
                _createdCount = _initialCreateCount;
                _resourceCreator.Create();
            }

            while (true)
            {
                await UniTask.WaitForSeconds(_createDelay);

                _resourceCreator.Create();
                _createdCount++;

                if (_createdCount >= _maxCreatCount) 
                    await UniTask.WaitUntil(() => _createdCount < _maxCreatCount);
            }
        }

        private void OnCollisionEnter(Collision player)
        {
            Pointer.SetActive(false);
            _createdCount = 0;
            PlayerApproached?.Invoke();
        }
    }
}