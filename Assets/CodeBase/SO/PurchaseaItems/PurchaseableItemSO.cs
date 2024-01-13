using CodeBase.Enums;
using CodeBase.Gameplay.PurchaseableSystem;
using CodeBase.SO.GameItem;
using UnityEngine;

namespace CodeBase.SO.PurchaseaItems
{
    [CreateAssetMenu(fileName = nameof(PurchaseableItemSO), menuName = "Gameplay/GameItemSO/PurchaseableItemSO")]
    public class PurchaseableItemSO : GameItemAbstractSO
    {
        public GameItemType GameItemType;
        [Range(0, 100000)] public int Price;
        public PurchaseableItem Prefab;
    }
}