using System;
using System.Collections.Generic;
using CodeBase.Gameplay.ObjectCreatorSystem;
using CodeBase.Services.TriggerObserve;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CodeBase.Gameplay.PaperSystem
{
    public class PaperCreatorTable : MonoBehaviour
    {
        [SerializeField] private ResourceCreator _resourceCreator;
        [SerializeField] private TriggerObserver _triggerObserver;
        [SerializeField] private int _maxCreatCount = 10;
        [SerializeField] private float _createDelay = 3f;

        public GameObject Pointer;
        public event Action PlayerApproached;

        private List<Paper> _papers = new();
        private int _createdCount;

        private async void OnEnable()
        {
            _triggerObserver.CollisionEntered += OnPlayerApproached;

            for (int i = 0; i < 5; i++)
            {
                _resourceCreator.Create();
            }

            while (true)
            {
                await UniTask.WaitForSeconds(_createDelay);

                _resourceCreator.Create();
                _createdCount++;

                if (_createdCount >= _maxCreatCount)
                {
                    await UniTask.Yield();
                }
            }
        }

        private void OnDisable()
        {
            _triggerObserver.CollisionEntered -= OnPlayerApproached;
        }

        private void OnPlayerApproached(Collision player)
        {
            Pointer.SetActive(false);
            _createdCount = 0;
            PlayerApproached?.Invoke();
        }
    }
}