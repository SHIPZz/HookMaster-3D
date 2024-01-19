using CodeBase.Services.Saves;
using CodeBase.Services.Time;
using CodeBase.Services.WorldData;
using Cysharp.Threading.Tasks;
using DG.Tweening;

namespace CodeBase.InfraStructure
{
    public class BootstrapState : IState, IEnter
    {
        private readonly IWorldDataService _worldDataService;
        private readonly IGameStateMachine _gameStateMachine;
        private readonly WorldTimeService _worldTimeService;
        private readonly SaveFacade _saveFacade;

        public BootstrapState(IWorldDataService worldDataService,
            IGameStateMachine gameStateMachine,
            WorldTimeService worldTimeService,
            SaveFacade saveFacade)
        {
            _saveFacade = saveFacade;
            _worldTimeService = worldTimeService;
            _gameStateMachine = gameStateMachine;
            _worldDataService = worldDataService;
        }

        public async void Enter()
        {
            DOTween.KillAll();
            DOTween.Clear();
            DOTween.Init();
            
            await _worldDataService.Load();

            while (!_worldTimeService.GotTime) 
                await UniTask.Yield();

            _saveFacade.InitServices();
            _gameStateMachine.ChangeState<LevelLoadState>();
        }
    }
}