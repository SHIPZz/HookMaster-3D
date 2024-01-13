using System.Collections.Generic;
using CodeBase.Enums;
using CodeBase.Gameplay.GameItems;
using CodeBase.Services.CircleRouletteServices;
using CodeBase.Services.Factories.ShopItems;
using CodeBase.Services.Mining;
using CodeBase.Services.WorldData;

namespace CodeBase.Services.ShopItemData
{
    public class GameItemService
    {
        private readonly IWorldDataService _worldDataService;
        private readonly GameItemFactory _gameItemFactory;
        private readonly MiningFarmService _miningFarmService;
        private Dictionary<GameItemType, GameItemAbstract> _createdItems = new();
        private readonly CircleRouletteService _circleRouletteService;

        public GameItemService(IWorldDataService worldDataService, GameItemFactory gameItemFactory,
            MiningFarmService miningFarmService,
            CircleRouletteService circleRouletteService)
        {
            _circleRouletteService = circleRouletteService;
            _miningFarmService = miningFarmService;
            _worldDataService = worldDataService;
            _gameItemFactory = gameItemFactory;
        }

        public void Init()
        {
            _miningFarmService.Init();
            _circleRouletteService.Init();
            // foreach (GameItemType gameItemType in _worldDataService.WorldData.ShopItemData.PurchasedShopItems)
            // {
            //     GameItemAbstract gameItemAbstract = Create<GameItemAbstract>(gameItemType);
            //     _createdItems[gameItemAbstract.GameItemType] = gameItemAbstract;
            // }
        }

        public T Create<T>(GameItemType gameItemType) where T : GameItemAbstract
        {
            switch (gameItemType)
            {
                case GameItemType.MiningFarm:
                    CreateMiningFarm();
                    break;

                case GameItemType.CircleRoulette:
                     CreateCircleRoulette();
                    break;
            }

            return null;
        }

        private void CreateMiningFarm()
        {
            _miningFarmService.CreateMiningFarm();
        }

        private void CreateCircleRoulette()
        {
             _circleRouletteService.Create();

        }
    }
}