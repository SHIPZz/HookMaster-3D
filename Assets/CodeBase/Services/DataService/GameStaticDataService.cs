using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Constant;
using CodeBase.Enums;
using CodeBase.Gameplay.GameItems;
using CodeBase.Gameplay.GameItems.RandomItems;
using CodeBase.Gameplay.ObjectCreatorSystem;
using CodeBase.Gameplay.ResourceItem;
using CodeBase.SO;
using CodeBase.SO.GameItem;
using CodeBase.SO.GameItem.RandomItems;
using CodeBase.SO.InfoItems;
using UnityEngine;

namespace CodeBase.Services.DataService
{
    public class GameStaticDataService
    {
        private readonly Dictionary<Type, GameItemAbstractSO> _gameItemDatas;
        private readonly Dictionary<Type, GameItemAbstract> _gameItemPrefabs;
        private readonly Dictionary<GameItemType, Resource> _resourceItems;
        private readonly Dictionary<GameItemType, RandomItem> _randomItemPrefabs;
        private readonly Dictionary<GameItemType, RandomItemSO> _randomItemDatas;
        private readonly Dictionary<GameItemType, PopupAbstractSO> _popupDatas;
        private readonly Dictionary<InfoItemTypeId, InfoItemSO> _infoItemDatas;

        public GameStaticDataService()
        {
            _gameItemDatas = Resources.LoadAll<GameItemAbstractSO>(AssetPath.GameItemDatas)
                .ToDictionary(x => x.GetType(), x => x);

            _popupDatas = Resources.LoadAll<PopupAbstractSO>(AssetPath.GameItemDatas)
                .ToDictionary(x => x.GameItemType, x => x);
            
            _gameItemPrefabs = Resources.LoadAll<GameItemAbstract>(AssetPath.GameItems)
                .ToDictionary(x => x.GetType(), x => x);
            
            _resourceItems = Resources.LoadAll<Resource>(AssetPath.GameItems)
                .ToDictionary(x => x.GameItemType, x => x);
            
            _randomItemDatas = Resources.LoadAll<RandomItemSO>(AssetPath.RandomItemDatas)
                .ToDictionary(x => x.GameItemType, x => x);

            _randomItemPrefabs = Resources.LoadAll<RandomItem>(AssetPath.RandomItems)
                .ToDictionary(x => x.GameItemType, x => x);
            
            _infoItemDatas = Resources.LoadAll<InfoItemSO>(AssetPath.InfoItemDatas)
                .ToDictionary(x => x.InfoItemTypeId, x => x);
        }

        public InfoItemSO Get(InfoItemTypeId itemTypeId) => 
            _infoItemDatas[itemTypeId];
        
        public RandomItemSO GetRandomItemSO(GameItemType gameItemType) =>
            _randomItemDatas[gameItemType];

        public T GetSO<T>() where T : GameItemAbstractSO =>
            (T)_gameItemDatas[typeof(T)];

        public PopupAbstractSO GetSO(GameItemType gameItemType) =>
            _popupDatas[gameItemType];

        public RandomItem GetRandomItem(GameItemType gameItemType) =>
            _randomItemPrefabs[gameItemType];

        public List<RandomItem> GetAll<T>() where T : RandomItem
        {
            return _randomItemPrefabs.Values.ToList();
        }

        public List<T> GetAllSO<T>() where T : GameItemAbstractSO
        {
            var list = new List<T>();

            foreach (Type type in _gameItemDatas.Keys)
            {
                if (type == typeof(T))
                    list.Add((T)_gameItemDatas[type]);
            }

            return list;
        }
        
        public Resource GetResourceItem(GameItemType id) =>
            _resourceItems[id];

        public T Get<T>() where T : GameItemAbstract =>
            (T)_gameItemPrefabs[typeof(T)];
    }
}