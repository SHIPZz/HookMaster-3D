using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Constant;
using CodeBase.Enums;
using CodeBase.Gameplay.GameItems;
using CodeBase.Gameplay.RandomItemSystem;
using CodeBase.SO.GameItem;
using UnityEngine;

namespace CodeBase.Services.DataService
{
    public class GameStaticDataService
    {
        private readonly Dictionary<Type, GameItemAbstractSO> _gameItemDatas;
        private readonly Dictionary<Type, GameItemAbstract> _gameItemPrefabs;

        public GameStaticDataService()
        {
            _gameItemDatas = Resources.LoadAll<GameItemAbstractSO>(AssetPath.GameItemDatas)
                .ToDictionary(x => x.GetType(), x => x);

            _gameItemPrefabs = Resources.LoadAll<GameItemAbstract>(AssetPath.GameItems)
                .ToDictionary(x => x.GetType(), x => x);
        }

        public T GetSO<T>() where T : GameItemAbstractSO =>
            (T)_gameItemDatas[typeof(T)];

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

        public GameItemAbstract Get(GameItemType gameItemType) =>
            _gameItemPrefabs.Values.FirstOrDefault(x => x.GameItemType == gameItemType);

        public T Get<T>() where T : GameItemAbstract =>
            (T)_gameItemPrefabs[typeof(T)];
    }
}