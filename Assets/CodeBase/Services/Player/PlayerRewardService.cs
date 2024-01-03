using CodeBase.Services.WorldData;
using UnityEngine;
using Zenject;

namespace CodeBase.Services.Player
{
    public class PlayerRewardService : IInitializable
    {
        public bool CanPlayRouletteCircle { get; private set; }
        
        private readonly IWorldDataService _worldDataService;

        public PlayerRewardService(IWorldDataService worldDataService)
        {
            _worldDataService = worldDataService;
            Debug.Log("INIT CONSTRUCT");
        }

        public void Initialize()
        {
            // CanPlayRouletteCircle = _worldDataService.WorldData.PlayerRewardData.CanPlayRouletteCircle;
            Debug.Log("INITIALIZE");
            CanPlayRouletteCircle = true;
        }

        public void SetCanPlayRoulette(bool canPlay)
        {
            _worldDataService.WorldData.PlayerRewardData.CanPlayRouletteCircle = canPlay;
            CanPlayRouletteCircle = canPlay;
            _worldDataService.Save();
        }
    }
}