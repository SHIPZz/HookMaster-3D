using System;
using CodeBase.Enums;
using UnityEngine;

namespace CodeBase.Gameplay.PurchaseableSystem
{
    public class PurchaseableItem : MonoBehaviour
    {
        public GameItemType GameItemType;
        public int Price;

        private bool _isAccessible;
        
        public bool IsAсcessible
        {
            get => _isAccessible;
            set
            {
                _isAccessible = value;
                AccessChanged?.Invoke(_isAccessible);
            }
        }

        public event Action<bool> AccessChanged;
    }
}