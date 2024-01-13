using System.Collections.Generic;
using CodeBase.Enums;
using CodeBase.Gameplay.PurchaseableSystem;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace CodeBase.Services.Providers.PurchaseableItemProviders
{
    public class PurchaseableItemProvider : SerializedMonoBehaviour
    {
        [OdinSerialize] private Dictionary<GameItemType, PurchaseableItem> _purchaseableItems = new();

        public IReadOnlyDictionary<GameItemType, PurchaseableItem> PurchaseableItems => _purchaseableItems;
    }
}