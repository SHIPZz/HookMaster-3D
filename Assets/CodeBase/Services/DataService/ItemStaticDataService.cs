using System.Collections.Generic;
using System.Linq;
using CodeBase.Constant;
using CodeBase.Enums;
using CodeBase.Gameplay.RandomItemSystem;
using CodeBase.Gameplay.ShopItemSystem;
using UnityEngine;

namespace CodeBase.Services.DataService
{
    public class ItemStaticDataService
    {
        private readonly Dictionary<ShopItemTypeId, ShopItemModel> _shopItemPrefabs;
        private readonly Dictionary<RandomItemTypeId, RandomItem> _randomItemPrefabs;

        public ItemStaticDataService()
        {
            _shopItemPrefabs = Resources.LoadAll<ShopItemModel>(AssetPath.ShopItems)
                .ToDictionary(x => x.ShopItemTypeId, x => x);
            
            _randomItemPrefabs = Resources.LoadAll<RandomItem>(AssetPath.RandomItems)
                .ToDictionary(x => x.RandomItemTypeId, x => x);
        }

        public ShopItemModel Get(ShopItemTypeId shopItemTypeId) => 
            _shopItemPrefabs[shopItemTypeId];

        public RandomItem Get(RandomItemTypeId randomItemTypeId) =>
            _randomItemPrefabs[randomItemTypeId];


    }
}