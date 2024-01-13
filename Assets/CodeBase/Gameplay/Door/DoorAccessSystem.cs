using System;
using CodeBase.Gameplay.PurchaseableSystem;
using CodeBase.Services.TriggerObserve;
using UnityEngine;

namespace CodeBase.Gameplay.Door
{
    [RequireComponent(typeof(DoorSystem))]
    public class DoorAccessSystem : MonoBehaviour
    {
        private DoorSystem _doorSystem;
        private PurchaseableItem _purchaseableItem;

        private void Awake()
        {
            _doorSystem = GetComponent<DoorSystem>();
            _purchaseableItem = GetComponent<PurchaseableItem>();
        }

        private void Start()
        {
            _purchaseableItem.AccessChanged += OnAccessChanged;

            _doorSystem.enabled = _purchaseableItem.IsAсcessible;
        }

        private void OnDisable() => 
            _purchaseableItem.AccessChanged -= OnAccessChanged;

        private void OnAccessChanged(bool isAccessed)
        {
            if (isAccessed)
                _doorSystem.enabled = true;
        }
    }
}