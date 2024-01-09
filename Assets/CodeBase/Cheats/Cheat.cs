using CodeBase.Data;
using CodeBase.Services.WorldData;
using UnityEngine;
using Zenject;

namespace CodeBase.Cheats
{
    public class Cheat : ITickable
    {
        private readonly IWorldDataService _worldDataService;

        public Cheat(IWorldDataService worldDataService)
        {
            _worldDataService = worldDataService;
        }

        public void Tick()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                _worldDataService.WorldData.PlayerData.WalletResources[ItemTypeId.Money] = 30000;
                _worldDataService.Save();
            }
            
            if (Input.GetKeyDown(KeyCode.F10))
            {
                PlayerPrefs.DeleteAll();
                PlayerPrefs.Save();
                _worldDataService.Reset();
                Debug.Log("clear");
            }
        }
    }
}