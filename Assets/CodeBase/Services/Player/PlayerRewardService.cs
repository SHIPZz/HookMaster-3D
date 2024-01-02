using CodeBase.Services.WorldData;
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
        }

        public void Initialize()
        {
            CanPlayRouletteCircle = _worldDataService.WorldData.PlayerRewardData.CanPlayRouletteCircle;
        }

        public void SetCanPlayRoulette(bool canPlay)
        {
            _worldDataService.WorldData.PlayerRewardData.CanPlayRouletteCircle = canPlay;
            CanPlayRouletteCircle = canPlay;
            _worldDataService.Save();
        }
    }
}