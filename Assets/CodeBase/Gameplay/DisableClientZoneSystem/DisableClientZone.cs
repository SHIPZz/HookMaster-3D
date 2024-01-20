using System;
using CodeBase.Services.TriggerObserve;
using UnityEngine;

namespace CodeBase.Gameplay.DisableClientZoneSystem
{
    public class DisableClientZone : MonoBehaviour
    {
        private TriggerObserver _triggerObserver;

        private void Awake() => 
            _triggerObserver = GetComponent<TriggerObserver>();

        private void OnEnable() => 
            _triggerObserver.TriggerEntered += OnClientEntered;

        private void OnDisable() => 
            _triggerObserver.TriggerEntered -= OnClientEntered;

        private void OnClientEntered(Collider collider)
        {
            Destroy(collider.gameObject);
        }
    }
}