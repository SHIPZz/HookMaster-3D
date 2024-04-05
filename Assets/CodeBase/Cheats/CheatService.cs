using CodeBase.Data;
using CodeBase.InfraStructure;
using CodeBase.Services.WorldData;
using UnityEngine;
using Zenject;

namespace CodeBase.Cheats
{
    public class CheatService : ITickable
    {
        private readonly IWorldDataService _worldDataService;
        private IGameStateMachine _gameStateMachine;

        public CheatService(IWorldDataService worldDataService, IGameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
            _worldDataService = worldDataService;
        }

        public void Tick()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                _worldDataService.WorldData.PlayerData.WalletResources[ItemTypeId.Money] = 30000;
                _worldDataService.Save();
            }
            
            if (Input.GetKeyDown(KeyCode.G))
            {
                PlayerPrefs.DeleteAll();
                PlayerPrefs.Save();
                _worldDataService.Reset();
                _gameStateMachine.ChangeState<BootstrapState>();
                Debug.Log("clear");
            }
        }
    }
}