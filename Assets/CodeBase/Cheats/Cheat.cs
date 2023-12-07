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
            if (Input.GetKeyDown(KeyCode.F10))
            {
                WorldData worldData = _worldDataService.WorldData;
                worldData.TableData.BusyTableIds.Clear();
                worldData.PotentialEmployeeList.Clear();
                worldData.PlayerData.PurchasedEmployees.Clear();
                worldData.PlayerData.Money = 0;
                PlayerPrefs.DeleteKey("DataKey");
                PlayerPrefs.Save();
                _worldDataService.Save();
                Debug.Log("clear");
            }
        }
    }
}