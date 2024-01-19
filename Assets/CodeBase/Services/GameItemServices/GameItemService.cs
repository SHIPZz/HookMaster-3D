using System;
using CodeBase.Enums;
using CodeBase.Gameplay.GameItems;
using CodeBase.Services.CircleRouletteServices;
using CodeBase.Services.Mining;
using CodeBase.Services.RandomItems;

namespace CodeBase.Services.ShopItemData
{
    public class GameItemService
    {
        private readonly MiningFarmService _miningFarmService;
        private readonly CircleRouletteService _circleRouletteService;
        private readonly RandomItemService _randomItemService;

        public event Action<GameItemAbstract> Created;

        public GameItemService(MiningFarmService miningFarmService, CircleRouletteService circleRouletteService, RandomItemService randomItemService)
        {
            _randomItemService = randomItemService;
            _circleRouletteService = circleRouletteService;
            _miningFarmService = miningFarmService;
        }

        public void Init()
        {
            _miningFarmService.Init();
            _circleRouletteService.Init();
            _randomItemService.Init();
        }

        public T Create<T>(GameItemType gameItemType) where T : GameItemAbstract
        {
            GameItemAbstract gameItemAbstract = null;
            
            switch (gameItemType)
            {
                case GameItemType.MiningFarm:
                    gameItemAbstract =  CreateMiningFarm<T>();
                    Created?.Invoke(gameItemAbstract);
                    return (T)gameItemAbstract;

                case GameItemType.CircleRoulette:
                    gameItemAbstract = CreateCircleRoulette<T>();
                    Created?.Invoke(gameItemAbstract);
                    return (T)gameItemAbstract;
            }
            

            return null;
        }

        private T CreateMiningFarm<T>() where T : GameItemAbstract => 
            _miningFarmService.CreateMiningFarm() as T;

        private T CreateCircleRoulette<T>()  where T : GameItemAbstract => 
            _circleRouletteService.Create() as T;
    }
}