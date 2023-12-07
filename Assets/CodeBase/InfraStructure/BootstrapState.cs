using CodeBase.Services.Profit;
using CodeBase.Services.Time;
using CodeBase.Services.WorldData;
using Cysharp.Threading.Tasks;

namespace CodeBase.InfraStructure
{
    public class BootstrapState : IState, IEnter
    {
        private readonly IWorldDataService _worldDataService;
        private readonly IGameStateMachine _gameStateMachine;
        private readonly WorldTimeService _worldTimeService;
        private readonly ProfitService _profitService;

        public BootstrapState(IWorldDataService worldDataService, 
            IGameStateMachine gameStateMachine,
            WorldTimeService worldTimeService,
            ProfitService profitService)
        {
            _profitService = profitService;
            _worldTimeService = worldTimeService;
            _gameStateMachine = gameStateMachine;
            _worldDataService = worldDataService;
        }

        public async void Enter()
        {
             await _worldDataService.Load();

            while (!_worldTimeService.GotTime)
            {
                await UniTask.Yield();
            }
            
            _profitService.Init();
            
            _gameStateMachine.ChangeState<LevelLoadState>();
        }
    }
}