using System;
using System.Collections.Generic;
using CodeBase.Enums;
using CodeBase.Gameplay.GameItems;
using CodeBase.Services.CircleRouletteServices;
using CodeBase.Services.Mining;
using CodeBase.Services.RandomItems;

namespace CodeBase.Services.GameItemServices
{
    public class GameItemService
    {
        private readonly MiningFarmService _miningFarmService;
        private readonly CircleRouletteService _circleRouletteService;
        private readonly RandomItemService _randomItemService;
        private Dictionary<GameItemType, Action> _createActions = new();

        public event Action<GameItemAbstract> Created;

        public GameItemService(MiningFarmService miningFarmService, CircleRouletteService circleRouletteService,
            RandomItemService randomItemService)
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

            _createActions[GameItemType.MiningFarm] = CreateMiningFarm;
            _createActions[GameItemType.CircleRoulette] = CreateCircleRoulette;
        }

        public void Create(GameItemType gameItemType)
        {
            _createActions[gameItemType]?.Invoke();
        }

        private void CreateCircleRoulette()
        {
            CircleRouletteItem circleRoulette = _circleRouletteService.Create();
            Created?.Invoke(circleRoulette);
        }

        private void CreateMiningFarm()
        {
            MiningFarmItem miningFarm = _miningFarmService.CreateMiningFarm();
            Created?.Invoke(miningFarm);
        }
    }
}