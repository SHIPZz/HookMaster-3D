using System;
using CodeBase.Gameplay.PurchaseableSystem;
using UnityEngine;

namespace CodeBase.Gameplay._3DPointers
{
    public class PurchaseableItemCirclePointerHandler : MonoBehaviour
    {
        [SerializeField] private CirclePointer _circlePointer;
        [SerializeField] private PurchaseableItem _purchaseableItem;

        private void Start()
        {
            if (!_purchaseableItem.IsAсcessible)
            {
                _circlePointer.Enable();
                return;
            }
            
            _circlePointer.Disable();
        }

        private void OnEnable()
        {
            _purchaseableItem.AccessChanged += OnAccessChanged;
        }

        private void OnDisable()
        {
            _purchaseableItem.AccessChanged -= OnAccessChanged;
        }

        private void OnAccessChanged(bool isAccessed)
        {
            if(!isAccessed)
                return;
            
            _circlePointer.Disable();
        }
    }
}