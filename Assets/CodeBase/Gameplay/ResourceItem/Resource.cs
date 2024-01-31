using System;
using CodeBase.Data;
using CodeBase.Gameplay.GameItems;
using CodeBase.Services.Wallet;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.ResourceItem
{
    internal class Resource : GameItemAbstract, IResource
    {
        [field: SerializeField] public ItemTypeId ItemTypeId { get; private set; }
        [field: SerializeField] public int Value { get; private set; }
        
        public event Action<Resource> Collected;
        
        [SerializeField] private Collider _collider;

        public Transform Anchor => transform;

        [Inject] private WalletService _walletService;

        public void MarkAsDetected()
        {
            _collider.enabled = false;
        }

        public void Collect()
        {
            Collected?.Invoke(this);
            _walletService.Set(ItemTypeId, Value);
            Destroy(gameObject);
        }
    }
}