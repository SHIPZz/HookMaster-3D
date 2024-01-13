using System.Collections.Generic;
using CodeBase.Data;
using CodeBase.Enums;
using CodeBase.Extensions;
using CodeBase.Gameplay.GameItems;
using CodeBase.Services.Factories.ShopItems;
using CodeBase.Services.Providers.Location;
using CodeBase.Services.WorldData;

namespace CodeBase.Services.CircleRouletteServices
{
    public class CircleRouletteService
    {
        private readonly IWorldDataService _worldDataService;
        private readonly LocationProvider _locationProvider;
        private readonly GameItemFactory _gameItemFactory;
        private List<CircleRouletteItem> _createdItems = new();

        public CircleRouletteService(IWorldDataService worldDataService, LocationProvider locationProvider, GameItemFactory gameItemFactory)
        {
            _worldDataService = worldDataService;
            _locationProvider = locationProvider;
            _gameItemFactory = gameItemFactory;
        }

        public void Init()
        {
            foreach (CircleRouletteItemData circleRouletteItemData in _worldDataService.WorldData.CircleRouletteItemDatas)
            {
               var item = _gameItemFactory.Create<CircleRouletteItem>(_locationProvider.CircleRouletteSpawnPoint,
                    circleRouletteItemData.Position.ToVector());
                
                _createdItems.Add(item);
            }
        }

        public CircleRouletteItem Create()
        {
            var item = _gameItemFactory.Create<CircleRouletteItem>(_locationProvider.CircleRouletteSpawnPoint, 
                _locationProvider.CircleRouletteSpawnPoint.position);
            _createdItems.Add(item);
            
            _worldDataService.WorldData.CircleRouletteItemDatas.Add(item.ToData());
            return item;
        }
    }
}