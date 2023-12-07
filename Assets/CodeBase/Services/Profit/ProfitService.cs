using System;
using System.Linq;
using CodeBase.Data;
using CodeBase.Services.Time;
using CodeBase.Services.WorldData;
using UnityEngine;

namespace CodeBase.Services.Profit
{
    public class ProfitService
    {
        private const int AdditionalProfit = 2;
        private readonly IWorldDataService _worldDataService;
        private readonly WorldTimeService _worldTimeService;

        public ProfitService(IWorldDataService worldDataService, WorldTimeService worldTimeService)
        {
            _worldTimeService = worldTimeService;
            _worldDataService = worldDataService;
        }

        public void Init()
        {
            int timeDifference = _worldTimeService.GetTimeDifferenceByDay();

            PlayerData playerData = _worldDataService.WorldData.PlayerData;

            if (timeDifference == 0)
                return;
            
            foreach (int targetProfit in playerData.PurchasedEmployees.Select(employee =>
                         employee.Profit * AdditionalProfit))
            {
                playerData.Money += targetProfit;
            }

            Debug.Log(timeDifference);
        }
    }
}