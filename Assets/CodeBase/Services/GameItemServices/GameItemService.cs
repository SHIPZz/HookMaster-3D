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
        private readonly MiningFarmService _miningFarmService;
        private readonly CircleRouletteService _circleRouletteService;
        
        public GameItemService(MiningFarmService miningFarmService, CircleRouletteService circleRouletteService)
        {
            _circleRouletteService = circleRouletteService;
            _miningFarmService = miningFarmService;
        }

        public void Init()
        {
            _miningFarmService.Init();
            _circleRouletteService.Init();
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