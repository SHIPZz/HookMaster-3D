using System;
using System.Collections.Generic;
using CodeBase.Gameplay.PurchaseableSystem;
using CodeBase.Services.PurchaseableItemServices;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.AccessibleSystem
{
    public class AccessibleItemHandlerByPurchaseableItem : SerializedMonoBehaviour
    {
        [OdinSerialize] private List<IAccessible> _accessibles;
        [SerializeField] private PurchaseableItem _purchaseableItem;
        private PurchaseableItemService _purchaseableItemService;

        [Inject]
        private void Construct(PurchaseableItemService purchaseableItemService)
        {
            _purchaseableItemService = purchaseableItemService;
        }

        private void OnEnable()
        {
            _purchaseableItem.AccessChanged += OnAccessChanged;
        }

        private void Start()
        {
            if (!_purchaseableItemService.IsAccessible(_purchaseableItem.GameItemType))
            {
                _accessibles.ForEach(x => x.Block());
                return;
            }
            
            _accessibles.ForEach(x => x.UnLock());
        }

        private void OnDisable()
        {
            _purchaseableItem.AccessChanged -= OnAccessChanged;
        }

        private void OnAccessChanged(bool isAccessed)
        {
            if (!isAccessed)
                return;

            _accessibles.ForEach(x => x.UnLock());
        }
    }
}