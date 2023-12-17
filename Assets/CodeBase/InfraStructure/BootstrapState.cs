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

        public BootstrapState(IWorldDataService worldDataService,
            IGameStateMachine gameStateMachine,
            WorldTimeService worldTimeService)
        {
            _worldTimeService = worldTimeService;
            _gameStateMachine = gameStateMachine;
            _worldDataService = worldDataService;
        }

        public async UniTaskVoid Enter()
        {
            await _worldDataService.Load();

            while (!_worldTimeService.GotTime)
            {
                await UniTask.Yield();
            }

            _gameStateMachine.ChangeState<LevelLoadState>();
        }
    }
}