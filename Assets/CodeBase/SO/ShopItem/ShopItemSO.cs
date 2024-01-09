using CodeBase.Data;
using CodeBase.Enums;
using UnityEngine;

namespace CodeBase.SO.ShopItem
{
    [CreateAssetMenu(fileName = "ShopItemSO", menuName = "Gameplay/Data/ShopItemSO")]
    public class ShopItemSO : ScriptableObject
    {
        public ShopItemTypeId ShopItemTypeId;
        public ItemTypeId ItemTypeId;
        public int Price;
    }
}