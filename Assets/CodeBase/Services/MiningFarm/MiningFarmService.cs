using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Constant;
using CodeBase.Data;
using CodeBase.Enums;
using CodeBase.Gameplay.Wallet;
using CodeBase.Services.DataService;
using CodeBase.Services.ShopItemData;
using CodeBase.Services.Time;
using CodeBase.Services.WorldData;
using CodeBase.SO.GameItem.MiningFarm;
using UnityEngine;

namespace CodeBase.Services.MiningFarm
{
    public class MiningFarmService
    {
        private readonly IWorldDataService _worldDataService;
        private readonly WorldTimeService _worldTimeService;
        private readonly ShopItemService _shopItemService;
        private readonly WalletService _walletService;

        private int _workingMinutes;
        private GameItemStaticDataService _gameItemStaticDataService;

        public bool IsWorking { get; private set; }

        public event Action Stopped;

        public MiningFarmService(ShopItemService shopItemService,
            WorldTimeService worldTimeService,
            IWorldDataService worldDataService,
            WalletService walletService,
            GameItemStaticDataService gameItemStaticDataService)
        {
            _gameItemStaticDataService = gameItemStaticDataService;
            _walletService = walletService;
            _worldDataService = worldDataService;
            _worldTimeService = worldTimeService;
            _shopItemService = shopItemService;
        }

        public void Init()
        {
            List<Gameplay.ShopItemSystem.MiningFarm> createdMiningFarms =
                _shopItemService.GetAll<Gameplay.ShopItemSystem.MiningFarm>(ShopItemTypeId.MiningFarm).ToList();

            if (createdMiningFarms.Count == 0)
                return;

            var miningFarmSO = _gameItemStaticDataService.Get<MiningFarmSO>();
            ConfigureMiningFarms(createdMiningFarms, miningFarmSO, NeedClean());

            if (NeedClean())
                return;

            _workingMinutes = _worldDataService.WorldData.MiningFarmData.WorkingMinutes;
            
            var timeDifference = _worldTimeService.GetTimeDifferenceByLastMiningTimeInMinutes();
            timeDifference = Mathf.Clamp(timeDifference, 0, TimeConstantValue.MinutesInTwoHour);

            if (HandleTimeDifference(timeDifference, createdMiningFarms)) 
                return;

            Stopped?.Invoke();
        }

        public void SetWorkingMinutes(int minutes)
        {
            if (minutes == TimeConstantValue.MinutesInTwoHour)
                Stopped?.Invoke();

            _worldDataService.WorldData.MiningFarmData.WorkingMinutes = minutes;
        }

        public void SetProfit(int amount) =>
            _walletService.Set(ItemTypeId.Money, amount);

        public void SetNeedClean(bool needClean)
        {
            if (needClean)
                Stopped?.Invoke();

            _worldDataService.WorldData.MiningFarmData.NeedClean = needClean;
        }

        public void ResetLastMiningTime() =>
            _worldTimeService.ResetLastMiningTime();

        private void ConfigureMiningFarms(List<Gameplay.ShopItemSystem.MiningFarm> createdMiningFarms,
            MiningFarmSO miningFarmSO, bool needClean)
        {
            foreach (var miningFarm in createdMiningFarms)
            {
                miningFarm.SetProfitPerMinute(miningFarmSO.ProfitPerMinute);
                miningFarm.SetTemperatures(miningFarmSO.MinTemperature, miningFarmSO.MidTemperature, miningFarmSO.MaxTemperature);
                miningFarm.SetNeedClean(needClean);
            }
        }

        private bool NeedClean()
        {
            if (_worldDataService.WorldData.MiningFarmData.NeedClean)
            {
                Stopped?.Invoke();
                return true;
            }

            return false;
        }

        private bool HandleTimeDifference(int timeDifference, List<Gameplay.ShopItemSystem.MiningFarm> createdMiningFarms)
        {
            if (timeDifference == TimeConstantValue.MinutesInTwoHour)
            {
                createdMiningFarms.ForEach(x => _walletService.Set(ItemTypeId.Money, x.ProfitPerMinute * timeDifference));
                SetNeedClean(true);
                Stopped?.Invoke();
                return true;
            }

            if (_workingMinutes != TimeConstantValue.MinutesInTwoHour)
            {
                _workingMinutes = Mathf.Clamp(_workingMinutes + timeDifference, 0, TimeConstantValue.MinutesInTwoHour);
                createdMiningFarms.ForEach(x => x.Init(_workingMinutes, this));
                return true;
            }

            return false;
        }
    }
}