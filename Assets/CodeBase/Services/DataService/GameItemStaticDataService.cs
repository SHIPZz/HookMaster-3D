using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Constant;
using CodeBase.Enums;
using CodeBase.Gameplay.RandomItemSystem;
using CodeBase.Gameplay.ShopItemSystem;
using CodeBase.SO.GameItem;
using UnityEngine;

namespace CodeBase.Services.DataService
{
    public class GameItemStaticDataService
    {
        private readonly Dictionary<ShopItemTypeId, ShopItemGameModel> _shopItemPrefabs;
        private readonly Dictionary<RandomItemTypeId, RandomItemGameModel> _randomItemPrefabs;
        private readonly Dictionary<Type, GameItemAbstractSO> _gameItemDatas;

        public GameItemStaticDataService()
        {
            _shopItemPrefabs = Resources.LoadAll<ShopItemGameModel>(AssetPath.ShopItems)
                .ToDictionary(x => x.ShopItemTypeId, x => x);

            _randomItemPrefabs = Resources.LoadAll<RandomItemGameModel>(AssetPath.RandomItems)
                .ToDictionary(x => x.RandomItemTypeId, x => x);

            _gameItemDatas = Resources.LoadAll<GameItemAbstractSO>(AssetPath.GameItemDatas)
                .ToDictionary(x => x.GetType(), x => x);
        }

        public T Get<T>() where T : GameItemAbstractSO => 
            (T)_gameItemDatas[typeof(T)];

        public ShopItemGameModel Get(ShopItemTypeId shopItemTypeId) =>
            _shopItemPrefabs[shopItemTypeId];

        public RandomItemGameModel Get(RandomItemTypeId randomItemTypeId) =>
            _randomItemPrefabs[randomItemTypeId];
    }
}