using System.Collections.Generic;
using CodeBase.Data;
using CodeBase.Enums;
using CodeBase.Extensions;
using CodeBase.Gameplay.GameItems;
using CodeBase.Services.DataService;
using CodeBase.Services.Factories.ShopItems;
using CodeBase.Services.Providers.Location;
using CodeBase.Services.WorldData;
using CodeBase.SO.GameItem.CircleRoulette;
using UnityEngine;

namespace CodeBase.Services.CircleRouletteServices
{
    public class CircleRouletteService
    {
        private readonly IWorldDataService _worldDataService;
        private readonly LocationProvider _locationProvider;
        private readonly GameItemFactory _gameItemFactory;
        private readonly GameItemStaticDataService _gameItemStaticDataService;
        private List<CircleRouletteItem> _createdItems = new();

        public CircleRouletteService(IWorldDataService worldDataService,
            LocationProvider locationProvider,
            GameItemFactory gameItemFactory, GameItemStaticDataService gameItemStaticDataService)
        {
            _gameItemStaticDataService = gameItemStaticDataService;
            _worldDataService = worldDataService;
            _locationProvider = locationProvider;
            _gameItemFactory = gameItemFactory;
        }

        public void Init()
        {
            foreach (var circleRouletteItemData in _worldDataService.WorldData.CircleRouletteItemDatas.Values)
            {
                var item = _gameItemFactory.Create<CircleRouletteItem>(_locationProvider.CircleRouletteSpawnPoint,
                    circleRouletteItemData.Position.ToVector());
                
                _createdItems.Add(item);
            }

            SetValuesFromData(_createdItems, _worldDataService.WorldData.CircleRouletteItemDatas);
        }

        public CircleRouletteItem Create()
        {
            var item = _gameItemFactory.Create<CircleRouletteItem>(_locationProvider.CircleRouletteSpawnPoint,
                _locationProvider.CircleRouletteSpawnPoint.position);
            _createdItems.Add(item);
            var data = _gameItemStaticDataService.GetSO<CircleRouletteSO>();
            PrepareItem(item, data.MinWinValue, data.MaxWinValue, data.PlayTime, item.transform.position);

            CircleRouletteItemData targetItem = item.ToData();
            _worldDataService.WorldData.CircleRouletteItemDatas[targetItem.Id] = targetItem;
            return item;
        }

        private void SetValuesFromData(List<CircleRouletteItem> createdItems,
            Dictionary<string, CircleRouletteItemData> circleRouletteItemDatas)
        {
            foreach (CircleRouletteItem circleRouletteItem in createdItems)
            {
                CircleRouletteItemData targetData = circleRouletteItemDatas[circleRouletteItem.Id];
                PrepareItem(circleRouletteItem, targetData.MinWinValue, targetData.MaxWinValue, targetData.PlayTime,
                    targetData.Position.ToVector());
            }
        }

        private void PrepareItem(CircleRouletteItem item, int minWinValue, int maxWinValue, int playTime,
            Vector3 position)
        {
            item.MinWinValue = minWinValue;
            item.MaxWinValue = maxWinValue;
            item.PlayTime = playTime;
            item.transform.position = position;
        }
    }
}