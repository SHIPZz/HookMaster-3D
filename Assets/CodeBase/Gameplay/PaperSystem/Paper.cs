using System;
using CodeBase.Gameplay.GameItems;
using CodeBase.Gameplay.PlayerSystem;
using CodeBase.Services.TriggerObserve;
using UnityEngine;

namespace CodeBase.Gameplay.PaperSystem
{
    public class Paper : GameItemAbstract
    {
        private TriggerObserver _triggerObserver;
        public bool IsOnEmployeeTable { get; private set; }
        public bool IsFinished { get; private set; }

        private void Awake() => 
            _triggerObserver = GetComponent<TriggerObserver>();

        private void OnEnable() => 
            _triggerObserver.TriggerEntered += OnPlayerEntered;

        private void OnDisable() => 
            _triggerObserver.TriggerEntered -= OnPlayerEntered;

        public void SetOnEmployeeTable(bool isOnEmployeeTable) => 
            IsOnEmployeeTable = isOnEmployeeTable;

        public void SetFinished(bool isFinished) =>
            IsFinished = isFinished;

        private void OnPlayerEntered(Collider collider)
        {
            if (!collider.gameObject.TryGetComponent(out PlayerPaperContainer playerPaperContainer))
                return;
            
            transform.SetParent(collider.transform);
        }
    }
}