using System;
using CodeBase.Enums;

namespace CodeBase.Data
{
    [Serializable]
    public class PurchaseableItemData
    {
        public GameItemType GameItemType;
        public int Price;
        public bool IsAccessible;
        public VectorData Position;
    }
}