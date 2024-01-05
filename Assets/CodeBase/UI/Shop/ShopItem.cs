using System;
using CodeBase.Enums;
using UnityEngine;

namespace CodeBase.UI.Shop
{
    [Serializable]
    public class ShopItem : MonoBehaviour
    {
        [field: SerializeField] public ShopItemTypeId ShopItemTypeId { get; private set; }
    }
}