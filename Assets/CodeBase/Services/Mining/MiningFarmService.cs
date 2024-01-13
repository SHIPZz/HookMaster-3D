using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Constant;
using CodeBase.Data;
using CodeBase.Enums;
using CodeBase.Extensions;
using CodeBase.Gameplay.GameItems;
using CodeBase.Gameplay.Wallet;
using CodeBase.Services.DataService;
using CodeBase.Services.Factories.ShopItems;
using CodeBase.Services.Providers.Location;
using CodeBase.Services.ShopItemData;
using CodeBase.Services.Time;
using CodeBase.Services.WorldData;
using CodeBase.SO.GameItem.MiningFarm;
using UnityEngine;

namespace CodeBase.Services.Mining
{
    public class MiningFarmService
    {
        private readonly IWorldDataService _worldDataService;
        private readonly WorldTimeService _worldTimeService;
        private readonly WalletService _walletService;
        private readonly GameItemFactory _gameItemFactory;

        private int _workingMinutes;
        private readonly LocationProvider _locationProvider;

        private List<MiningFarmItem> _createdFarms = new();

        public event Action Stopped;

        public MiningFarmService(WorldTimeService worldTimeService,
            IWorldDataService worldDataService,
            WalletService walletService,
            GameItemFactory gameItemFactory,
            LocationProvider locationProvider)
        {
            _locationProvider = locationProvider;
            _gameItemFactory = gameItemFactory;
            _walletService = walletService;
            _worldDataService = worldDataService;
            _worldTimeService = worldTimeService;
        }

        public void Init()
        {
            foreach (MiningFarmData miningFarmData in _worldDataService.WorldData.MiningFarmDatas)
            {
                var miningFarm = _gameItemFactory.Create<MiningFarmItem>(_locationProvider.MiningFarmSpawnPoint,
                    miningFarmData.Position.ToVector());
                _createdFarms.Add(miningFarm);
            }

            // if (_createdFarms.Count == 0)
            //     return;
            //
            // if (NeedClean())
            //     return;
            //
            // _workingMinutes = _worldDataService.WorldData.MiningFarmDatas.WorkingMinutes;
            //
            // var timeDifference = _worldTimeService.GetTimeDifferenceByLastMiningTimeInMinutes();
            // timeDifference = Mathf.Clamp(timeDifference, 0, TimeConstantValue.MinutesInTwoHour);
            //
            // if (HandleTimeDifference(timeDifference, createdMiningFarms))
            //     return;

            Stopped?.Invoke();
        }

        public MiningFarmItem CreateMiningFarm()
        {
            var item = _gameItemFactory.Create<MiningFarmItem>(_locationProvider.MiningFarmSpawnPoint,
                _locationProvider.MiningFarmSpawnPoint.position);
            
            _worldDataService.WorldData.MiningFarmDatas.Add(item.ToData());

            return item;
        }

        public void SetWorkingMinutes(int minutes)
        {
            if (minutes == TimeConstantValue.MinutesInTwoHour)
                Stopped?.Invoke();

            // _worldDataService.WorldData.MiningFarmDatas.WorkingMinutes = minutes;
        }

        public void SetProfit(int amount) =>
            _walletService.Set(ItemTypeId.Money, amount);

        public void SetNeedClean(bool needClean)
        {
            if (needClean)
                Stopped?.Invoke();

            // _worldDataService.WorldData.MiningFarmDatas.NeedClean = needClean;
        }

        public void ResetLastMiningTime() =>
            _worldTimeService.ResetLastMiningTime();

        private void ConfigureMiningFarms(List<MiningFarmItem> createdMiningFarms,
            MiningFarmSO miningFarmSO, bool needClean)
        {
            foreach (var miningFarm in createdMiningFarms)
            {
                miningFarm.SetProfitPerMinute(miningFarmSO.ProfitPerMinute);
                miningFarm.SetTemperatures(miningFarmSO.MinTemperature, miningFarmSO.MidTemperature,
                    miningFarmSO.MaxTemperature);
                miningFarm.SetNeedClean(needClean);
            }
        }

        private bool NeedClean()
        {
            // if (_worldDataService.WorldData.MiningFarmDatas.NeedClean)
            // {
            //     Stopped?.Invoke();
            //     return true;
            // }

            return false;
        }

        private bool HandleTimeDifference(int timeDifference,
            List<Gameplay.GameItems.MiningFarmItem> createdMiningFarms)
        {
            if (timeDifference == TimeConstantValue.MinutesInTwoHour)
            {
                createdMiningFarms.ForEach(
                    x => _walletService.Set(ItemTypeId.Money, x.ProfitPerMinute * timeDifference));
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

        public MiningFarmItem Get() =>
            _createdFarms.FirstOrDefault();
    }
}