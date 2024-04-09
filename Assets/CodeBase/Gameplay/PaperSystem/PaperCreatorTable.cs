using System;
using System.Collections;
using CodeBase.Gameplay.ObjectCreatorSystem;
using CodeBase.Services.TriggerObserve;
using UnityEngine;

namespace CodeBase.Gameplay.PaperSystem
{
    public class PaperCreatorTable : MonoBehaviour
    {
        [SerializeField] private ResourceCreator _resourceCreator;
        [SerializeField] private int _maxCreatCount = 10;
        [SerializeField] private float _createDelay = 3f;
        [SerializeField] private int _initialCreateCount = 5;
        [SerializeField] private TriggerObserver _triggerObserver;

        [field: SerializeField] public GameObject Pointer { get; private set; }

        public event Action PlayerApproached;

        private int _createdCount;

        private void OnEnable()
        {
            for (int i = 0; i < _initialCreateCount; i++)
            {
                _createdCount = _initialCreateCount;
                _resourceCreator.Create();
            }

            _triggerObserver.TriggerEntered += DisablePointer;
            StartCoroutine(CreateResourceCoroutine());
        }

        private void OnDisable() =>
            _triggerObserver.TriggerEntered -= DisablePointer;

        private IEnumerator CreateResourceCoroutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(_createDelay);

                _resourceCreator.Create();
                _createdCount++;

                if (_createdCount >= _maxCreatCount)
                    yield return new WaitUntil(() => _createdCount < _maxCreatCount);
            }
        }

        private void DisablePointer(Collider obj)
        {
            Pointer.SetActive(false);
            _createdCount = 0;
            PlayerApproached?.Invoke();
        }
    }
}