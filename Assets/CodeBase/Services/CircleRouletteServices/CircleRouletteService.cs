using System.Collections.Generic;
using CodeBase.Constant;
using CodeBase.Data;
using CodeBase.Extensions;
using CodeBase.Gameplay.GameItems;
using CodeBase.Services.DataService;
using CodeBase.Services.Factories.GameItem;
using CodeBase.Services.Providers.Location;
using CodeBase.Services.Time;
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
        private readonly GameStaticDataService _gameStaticDataService;
        private readonly WorldTimeService _worldTimeService;
        private List<CircleRouletteItem> _createdItems = new();

        public CircleRouletteService(IWorldDataService worldDataService,
            LocationProvider locationProvider,
            GameItemFactory gameItemFactory,
            GameStaticDataService gameStaticDataService, WorldTimeService worldTimeService)
        {
            _worldTimeService = worldTimeService;
            _gameStaticDataService = gameStaticDataService;
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

        public void SetLastPlayedTime(string id) => 
            _worldTimeService.SaveLastCircleRoulettePlayedTime(id);

        public bool CanPlay(string id) => 
            _worldTimeService.GetTimeDifferenceByLastCircleRoulettePlayTimeDays(id) >= TimeConstantValue.OneDay;

        public CircleRouletteItem Create()
        {
            var item = _gameItemFactory.Create<CircleRouletteItem>(_locationProvider.CircleRouletteSpawnPoint,
                _locationProvider.CircleRouletteSpawnPoint.position);
            _createdItems.Add(item);
            var data = _gameStaticDataService.GetSO<CircleRouletteSO>();
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