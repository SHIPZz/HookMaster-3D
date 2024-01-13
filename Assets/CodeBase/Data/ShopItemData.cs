using System;
using System.Collections.Generic;
using CodeBase.Enums;
using CodeBase.UI.Shop;

namespace CodeBase.Data
{
    [Serializable]
    public class ShopItemData
    {
        public List<GameItemType> PurchasedShopItems = new();
    }
}