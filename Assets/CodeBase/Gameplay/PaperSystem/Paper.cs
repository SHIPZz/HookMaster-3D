using System;
using _Project_legacy.Scripts.Papers;
using CodeBase.Gameplay.GameItems;
using CodeBase.Gameplay.PlayerSystem;
using CodeBase.Services.TriggerObserve;
using UnityEngine;

namespace CodeBase.Gameplay.PaperSystem
{
    public class Paper : GameItemAbstract, IHoldable
    {
        private TriggerObserver _triggerObserver;

        public bool IsOnEmployeeTable { get; private set; }
        public bool IsFinished { get; private set; }
        public Transform Transform => transform;
        public bool IsAccessed { get; set; }

        private void Awake() =>
            _triggerObserver = GetComponent<TriggerObserver>();

        private void OnEnable() =>
            _triggerObserver.TriggerEntered += OnPlayerEntered;

        private void OnDisable() =>
            _triggerObserver.TriggerEntered -= OnPlayerEntered;

        public void Destroy() => 
            DestroyImmediate(gameObject);

        private void OnPlayerEntered(Collider collider)
        {
            if (!collider.gameObject.TryGetComponent(out PlayerPaperContainer playerPaperContainer))
                return;

            IsAccessed = false;

            transform.SetParent(collider.transform);
        }
    }
}